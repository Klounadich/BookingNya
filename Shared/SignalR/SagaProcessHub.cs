using Microsoft.AspNetCore.SignalR;

namespace Shared.SignalR;

public class SagaProcessHub:Hub
{
    public async Task JoinSagaGroup(string sagaId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sagaId);
    }

    public async Task LeaveSagaGroup(string sagaId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sagaId);
    }
    public async Task SendSagaUpdate(string sagaId, string step, string status, string message)
    {
        await Clients.Group(sagaId).SendAsync("ReceiveSagaProgress", sagaId, step, status, message);
    }

    public async Task ErrorSendSagaUpdate(string sagaId, string step, string errorcode, string message)
    {
        await Clients.Group(sagaId).SendAsync("ReceiveSagaError", sagaId, step, errorcode, message);
    }
}