using BookingModule.Commands;
using BookingModule.Models;
using Microsoft.AspNetCore.Http;

namespace BookingModule.Repositories;

public interface IBookingRepository
{
    Task<bool> StartSaga(SagaStatesModel data);
    
}