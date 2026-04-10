using BookingModule.Commands;
using BookingModule.Services;
using DotNetCore.CAP;
using MediatR;


namespace BookingNya.Endpoints;

public static class BookingEndpoint
{
    public static void MapBookingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/").WithTags("Hotel");
        group.MapPost("/hotel" ,FreeRooms);
        
    }

    public async static Task<IResult> FreeRooms(IRequestTracker requestTracker , IMediator mediator, RoomFiltresCommand command)
    {
        try
        {
           var response =  await mediator.Send(command );
            return Results.Ok(response);
        }
        catch (TimeoutException)
        {
            return Results.Problem("Request timed out", statusCode: 408);
        }
    }
}