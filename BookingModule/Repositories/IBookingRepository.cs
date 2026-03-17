using BookingModule.Commands;
using BookingModule.Models;
using Microsoft.AspNetCore.Http;

namespace BookingModule.Repositories;

public interface IBookingRepository
{
    Task<bool> StartSaga(SagaStatesModel data , BookingModel booking);
    Task<SagaStatesModel> GetSagaStateBySagaIdAsync(Guid saga_id);
    Task<bool> UpdateSagaStateAsync (SagaStatesModel data);
    Task<BookingModel> GetBookingBySagaIdAsync(Guid saga_id);
}