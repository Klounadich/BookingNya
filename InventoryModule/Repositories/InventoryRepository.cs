
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

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
        var checkroom = await _context.RoomAvailabilities.Where(x => x.room_id == roomId).Select(x => x.is_available)
            .SingleOrDefaultAsync();
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
}