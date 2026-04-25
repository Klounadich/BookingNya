using BookingModule.Commands;
using BookingModule.Services;
using DotNetCore.CAP;
using InventoryModule.Infrastructure;
using InventoryModule.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace BookingNya.Endpoints;

public static class BookingEndpoint
{
    public static void MapBookingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/").WithTags("Hotel");
        group.MapPost("/hotel" ,FreeRooms);
        group.MapGet("/rooms/{roomId}/photos/{index}", DownloadImage);

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

    public static async Task<IResult> DownloadImage (string roomId, int index , InventoryDbContext context) 
    {
        var room = await context.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.id == roomId);
        if (room?.pictures == null || index < 0 || index >= room.pictures.Count)
        {
            return Results.NotFound();
        }
        var mimeType = "image/jpeg"; 
        var photoBytes = room.pictures[index];
        
        return Results.File(photoBytes, mimeType); 
    }
}