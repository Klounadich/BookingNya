using InventoryModule.Commands;

namespace InventoryModule.Services;

public interface IReserveRoomService
{
    public Task<RoomReserveResult> ReserveRoomAsync(ReserveRoomCommand command);
}