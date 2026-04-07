using BookingModule.Services;
using DotNetCore.CAP;


namespace BookingNya.Endpoints;

public static class BookingEndpoit
{
    public static void MapBookingEndpont(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/").WithTags("Hotel");
        group.MapPost("/hotel" ,FreeRooms);
        
    }

    public async static Task<IResult> FreeRooms(IRequestTracker requestTracker , IBookingService service)
    {
        var requestId = Guid.NewGuid();
        
        try
        {
            var response = await requestTracker.WaitForResponseAsync(requestId, TimeSpan.FromSeconds(3));
            service.GetFreeRooms(requestId);
            return Results.Ok(response);
        }
        catch (TimeoutException)
        {
            return Results.Problem("Request timed out", statusCode: 408);
        }
    }
}