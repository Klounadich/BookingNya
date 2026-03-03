using BookingModule.Commands;
using BookingModule.Infrastructure;
using BookingModule.Models;
using BookingModule.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace BookingModule.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    public async Task<StartSagaResult> StartBooking(BookingRequestCommand data)
    {

        SagaStatesModel sagaState = new SagaStatesModel
        {
            current_step = "ReserveRoom",
        };
        await _bookingRepository.StartSaga(sagaState);
        return new StartSagaResult
        (
            sagaState.id, 
            sagaState.status, 
            "Saga initiated successfully."
        );


    }
    
}