using BookingModule.Repositories;
using BookingModule.Services;
using DotNetCore.CAP;
using InventoryModule.Commands;
using Shared.Enums;
using PaymentModule.Commands;

namespace BookingModule.Subscribers;

public class RoomReservedSubscriber : ICapSubscribe
{
    private readonly IBookingRepository  _bookingRepository;
    private readonly ICapPublisher _capPublisher;

    public RoomReservedSubscriber(IBookingRepository bookingrepository , ICapPublisher capPublisher)
    {
        _bookingRepository = bookingrepository;
        _capPublisher = capPublisher;
    }
    [CapSubscribe("inventory.room.reserved.event")]
    public async Task HandleAsync(RoomReservedEvent command)
    {
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
        if (sagaState != null)
        {
            var booking = await _bookingRepository.GetBookingBySagaIdAsync(sagaState.saga_id);
            if (booking != null)
            {
                sagaState.status = SagaTypes.Completed;
                sagaState.current_step = "ProcessPayment";
                sagaState.last_updated_at = DateTime.UtcNow;
                await _bookingRepository.UpdateSagaStateAsync(sagaState);
                Console.WriteLine(sagaState.last_updated_at);
                await _capPublisher.PublishAsync("payment.process.payment.command", new ProcessPaymentCommand(
                    command.SagaId,
                    booking.id,
                    booking.total_price,
                    booking.currency,
                    booking.payment_method,
                    " ", /// check empty fields
                    " ",
                    booking.guest_email,
                    booking.guest_phone,
                    "",
                    sagaState.metadata
                     
                ));
            }

         
        }
    }
}