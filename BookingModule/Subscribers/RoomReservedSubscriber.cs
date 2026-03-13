using BookingModule.Repositories;
using BookingModule.Services;
using DotNetCore.CAP;
using InventoryModule.Commands;
using Shared.Enums;

namespace BookingModule.Subscribers;

public class RoomReservedSubscriber : ICapSubscribe
{
    private readonly IBookingRepository  _bookingRepository;

    public RoomReservedSubscriber(IBookingRepository bookingrepository)
    {
        _bookingRepository = bookingrepository;
    }
    [CapSubscribe("inventory.room.reserved.event")]
    public async Task HandleAsync(RoomReservedEvent command)
    {
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
        if (sagaState != null)
        {
            sagaState.status = SagaTypes.Completed;
            sagaState.current_step = "ProcessPayment";
            sagaState.last_updated_at = DateTime.UtcNow;

            await _bookingRepository.UpdateSagaStateAsync(sagaState);
        }
    }
}