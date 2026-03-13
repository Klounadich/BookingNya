using InventoryModule.Commands;
using InventoryModule.Models;
using InventoryModule.Repositories;

namespace InventoryModule.Services;

public class ReserveRoomService: IReserveRoomService
{
    private readonly IInventoryRepository  _inventoryRepository;

    public ReserveRoomService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    public async Task<RoomReserveResult> ReserveRoomAsync(ReserveRoomCommand command)
    {
        if (await _inventoryRepository.IsRoomAvailableAsync(command.roomId))
        {
            Guid id = Guid.NewGuid();
            RoomReservationModel request = new RoomReservationModel
            {
                id = id,
                room_id = command.roomId,
                saga_id = command.sagaId,
                booking_id = command.bookingId,
                guest_name = command.guest_name,
                check_in = command.check_in,
                check_out = command.check_out,
                reservation_reference = "" ,//фикс
                cancellation_reason = "" //фикс
            };
            if (await _inventoryRepository.ReserveRoomAsync(request))
            {
                return new RoomReserveResult(id , "Room reservation successful");
            }
        };
        return new RoomReserveResult(Guid.Empty , "Room reservation failed");
        
    }
}