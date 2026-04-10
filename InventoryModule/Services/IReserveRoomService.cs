using InventoryModule.Commands;

namespace InventoryModule.Services;

public interface IReserveRoomService
{
    public Task<RoomReserveResult> ReserveRoomAsync(ReserveRoomCommand command);
    public Task<FreeRoomsResponse> CheckFreeRoomsAsync(RequestRoomFIltresCommand command);
}