
using InventoryModule.Commands;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace InventoryModule.Repositories;

public class InventoryRepository: IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }
    public async Task<bool> IsRoomAvailableAsync(string roomId)
    {
        
        var checkroom = await _context.Rooms.Where(x => x.id == roomId).Select((x=> x.status == RoomStatus.Available)).FirstOrDefaultAsync();
        return checkroom;
    }

    public async Task<bool> ReserveRoomAsync(RoomReservationModel data)
    {
        _context.RoomReservations.Add(data);
        if(await _context.SaveChangesAsync() >0)
        {
            return true;
        }
        return false;
    }

    public async Task<FreeRoomsResponse> FreeRoomsAsync(Guid RequestId)
    {
       
       var availableRooms = await _context.Rooms
           .Where(x => x.status == RoomStatus.Available)
           .Select(room => new FreeRoomsCommand(
               room.id,
               room.type,
               room.capacity,
               room.price_per_night,
               room.floor,
               room.description,
               room.amenities
           ))
           .AsNoTracking()
           .ToListAsync();
       Console.WriteLine("FreeRooms not available");
        return new FreeRoomsResponse(
            availableRooms,
            availableRooms.Count,
            DateTime.UtcNow,
            RequestId
            );
    
    }
}