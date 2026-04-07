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
        service.GetFreeRooms(requestId);
        var response = await requestTracker.WaitForResponseAsync(requestId, TimeSpan.FromSeconds(3));
        if (response is null)
        {
            return Results.Problem("Request timed out" , statusCode: 408);
        }
        return Results.Ok(response);
    }
}