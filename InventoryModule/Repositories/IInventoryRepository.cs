using InventoryModule.Commands;
using InventoryModule.Models;

namespace InventoryModule.Repositories;

public interface IInventoryRepository
{
    public Task<bool> IsRoomAvailableAsync(string roomId);
    public Task<bool> ReserveRoomAsync(RoomReservationModel data);
}