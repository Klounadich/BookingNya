using System.Collections.Concurrent;
using InventoryModule.Commands;
using Microsoft.Extensions.Logging;

namespace BookingModule.Services;

public interface IRequestTracker
{
    Task<FreeRoomsResponse> WaitForResponseAsync(Guid requestId, TimeSpan timeout);
    void SetResponse(Guid requestId, FreeRoomsResponse response);
    void SetError(Guid requestId, Exception exception);
}

public class RequestTracker : IRequestTracker
{
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<FreeRoomsResponse>> _pendingRequests = new();
    private readonly ILogger<RequestTracker> _logger;

    public RequestTracker(ILogger<RequestTracker> logger)
    {
        _logger = logger;
    }

    public Task<FreeRoomsResponse> WaitForResponseAsync(Guid requestId, TimeSpan timeout)
    {
        var tcs = new TaskCompletionSource<FreeRoomsResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
        
        if (!_pendingRequests.TryAdd(requestId, tcs))
        {
            throw new InvalidOperationException($"Request {requestId} already exists");
        }

        
        var cts = new CancellationTokenSource(timeout);
        cts.Token.Register(() =>
        {
            if (_pendingRequests.TryRemove(requestId, out var pendingTcs))
            {
                _logger.LogWarning("Request {RequestId} timed out after {Timeout}", requestId, timeout);
                pendingTcs.TrySetException(new TimeoutException($"Request {requestId} timed out"));
            }
        });

        return tcs.Task;
    }

    public void SetResponse(Guid requestId, FreeRoomsResponse response)
    {
        if (_pendingRequests.TryRemove(requestId, out var tcs))
        {
            _logger.LogInformation("Response received for request {RequestId}", requestId);
            tcs.TrySetResult(response);
        }
        else
        {
            _logger.LogWarning("No pending request found for {RequestId}", requestId);
        }
    }

    public void SetError(Guid requestId, Exception exception)
    {
        if (_pendingRequests.TryRemove(requestId, out var tcs))
        {
            _logger.LogError(exception, "Error for request {RequestId}", requestId);
            tcs.TrySetException(exception);
        }
    }
}