using InventoryModule.Commands;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Repositories;

public class InventoryRepository: IInventoryRepository
{
    private readonly InventoryDbContext _db;

    public InventoryRepository(InventoryDbContext db)
    {
        _db = db;
    }
    public async Task<bool> IsRoomAvailableAsync(string roomId)
    {
        var checkroom = await _db.RoomAvailabilities.Where(x => x.room_id == roomId).Select(x => x.is_available)
            .SingleOrDefaultAsync();
        return checkroom;
    }

    public async Task<bool> ReserveRoomAsync(RoomReservationModel data)
    {
        _db.RoomReservations.Add(data);
        if(await _db.SaveChangesAsync() >0)
        {
            return true;
        }
        return false;
    }
}