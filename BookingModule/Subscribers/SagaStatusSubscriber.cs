using BookingModule.Repositories;
using BookingModule.Services;
using DotNetCore.CAP;
using InventoryModule.Commands;
using Microsoft.AspNetCore.SignalR;
using Shared.Enums;
using Shared.SignalR;

namespace BookingModule.Subscribers;

public class SagaStatusSubscriber : ICapSubscribe
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHubContext<SagaProcessHub> _hubContext;
    private readonly ISagaOrchestrator _sagaOrchestrator;

    public SagaStatusSubscriber(
        IBookingRepository bookingRepository,
        IHubContext<SagaProcessHub> hubContext,
        ISagaOrchestrator sagaOrchestrator)
    {
        _bookingRepository = bookingRepository;
        _hubContext = hubContext;
        _sagaOrchestrator = sagaOrchestrator;
    }

    [CapSubscribe("booking.reserve.room.started.event")]
    public async Task Handle(ReserveRoomStartedCommand command)
    {
        await _sagaOrchestrator.HandleStepSuccessAsync(
            command.saga_id,
            "ReserveRoom",
            "Starting Reservation."
        );
    }
}