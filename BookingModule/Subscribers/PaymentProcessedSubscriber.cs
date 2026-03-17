using BookingModule.Repositories;
using DotNetCore.CAP;
using PaymentModule.Commands;
using Shared.Enums;

namespace BookingModule.Subscribers;

public class PaymentProcessedSubscriber : ICapSubscribe
{
 private readonly IBookingRepository  _bookingRepository;

 public PaymentProcessedSubscriber(IBookingRepository bookingRepository)
 {
  _bookingRepository = bookingRepository;
 }
 [CapSubscribe("payment.processed.event")]
 public async Task HandleAsync(PaymentProcessed command)
 {
  var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
  if (sagaState != null)
  {
   sagaState.status = SagaTypes.Completed;
   sagaState.current_step = "SendConfirmation";
   sagaState.last_updated_at = DateTime.UtcNow;

   if (await _bookingRepository.UpdateSagaStateAsync(sagaState))
   {
   // await _capPublisher.PublishAsync("notification.send.confirmation.command", new SendConfirmationCommand(...
   }
         
  }
 }
}