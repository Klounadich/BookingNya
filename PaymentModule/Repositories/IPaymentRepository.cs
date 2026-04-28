
using PaymentModule.Models;

namespace PaymentModule.Repositories;

public interface IPaymentRepository
{
  public Task<bool> SavePaymentTransaction(PaymentsModel model);
  public Task<PaymentsModel?> GetTransaction(Guid sagaId);
  
  public Task<bool> UpdateTransaction(PaymentsModel model);
  
}