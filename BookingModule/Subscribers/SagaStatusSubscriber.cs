using BookingModule.Repositories;
using DotNetCore.CAP;
using InventoryModule.Commands;
using Shared.Enums;

namespace BookingModule.Subscribers;

public class SagaStatusSubscriber : ICapSubscribe
{
    private readonly ICapPublisher _capPublisher;
    private readonly IBookingRepository  _bookingRepository;

    public SagaStatusSubscriber(IBookingRepository bookingRepository , ICapPublisher capPublisher)
    {
        _bookingRepository = bookingRepository;
        _capPublisher = capPublisher;
    }
    [CapSubscribe("booking.reserve.room.started.event")]
    public async Task Handle(ReserveRoomStartedCommand command)
    {
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.saga_id);
        if (sagaState != null)
        {
            sagaState.status = SagaTypes.Running;
            sagaState.current_step = "ReserveRoom";
            sagaState.last_updated_at = DateTime.UtcNow;

            await _bookingRepository.UpdateSagaStateAsync(sagaState);
           // await _capPublisher.PublishAsync("payment.process.payment.command", new ProcessPaymentCommand(...));
        }
    }
}
