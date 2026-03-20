using BookingModule.Repositories;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using PaymentModule.Commands;
using Shared.Enums;
using Shared.SignalR;

namespace BookingModule.Subscribers;

public class PaymentProcessedSubscriber : ICapSubscribe
{
 private readonly IHubContext<SagaProcessHub> _hubContext;
 private readonly IBookingRepository  _bookingRepository;

 public PaymentProcessedSubscriber(IBookingRepository bookingRepository , IHubContext<SagaProcessHub> hubContext)
 {
  _bookingRepository = bookingRepository;
  _hubContext = hubContext;
 }
 [CapSubscribe("payment.processed.event")]
 public async Task HandleAsync(PaymentProcessed command)
 {
  var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
  if (!sagaState.Equals(null))
  {
   await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaProgress",
    command.SagaId, 
    "ProcessPayment",  
    "Completed",    
    "Payment completed." 
   ); 

   sagaState.status = SagaTypes.Completed;
   sagaState.current_step = "SendConfirmation";
   sagaState.last_updated_at = DateTime.UtcNow;

   if (await _bookingRepository.UpdateSagaStateAsync(sagaState))
   {
    await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaProgress",
     command.SagaId, 
     "Notification",  
     "Starting",    
     "Send notification." 
    ); 
   // await _capPublisher.PublishAsync("notification.send.confirmation.command", new SendConfirmationCommand(...
   }

   
         
  }
 }

 [CapSubscribe("payment.failed.event")]
 public async Task FailedHandleAsync(PaymentProcessed command)
 {
  var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
  if (!sagaState.Equals(null))
  {
   await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaError",
    command.SagaId,
    "ProcessPayment",
    "Failed",
    $"Payment failed   ({command.Error})."
   );

   sagaState.status = SagaTypes.Failed;
   sagaState.last_updated_at = DateTime.UtcNow;
   sagaState.error_message = command.Error;

   await _bookingRepository.UpdateSagaStateAsync(sagaState);

  }
 }
}