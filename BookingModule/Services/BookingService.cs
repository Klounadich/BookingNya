using BookingModule.Commands;
using BookingModule.Infrastructure;
using BookingModule.Models;
using BookingModule.Repositories;
using InventoryModule.Commands;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using MediatR;
using DotNetCore.CAP;

namespace BookingModule.Services;

public class BookingService : IBookingService
{
    private readonly ICapPublisher _capPublisher;   
    private readonly IBookingRepository _bookingRepository;
    private readonly IMediator _mediator;
    

    public BookingService(IBookingRepository bookingRepository , IMediator mediator , ICapPublisher capPublisher)
    {
        _bookingRepository = bookingRepository;
        _mediator = mediator;
        _capPublisher = capPublisher;
    }
    public async Task<StartSagaResult> StartBooking(BookingRequestCommand data)
    {

        SagaStatesModel sagaState = new SagaStatesModel
        {
            current_step = "ReserveRoom",
        };
        await _bookingRepository.StartSaga(sagaState);
        await _capPublisher.PublishAsync( "inventory.reserve.room.command",new ReserveRoomCommand(sagaState.saga_id , data.room_id, data.check_in, data.check_out));
        return new StartSagaResult
        (
            sagaState.saga_id, 
            sagaState.status, 
            "Saga initiated successfully."
        );


    }
    
}