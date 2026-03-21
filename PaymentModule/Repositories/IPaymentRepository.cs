
using PaymentModule.Models;

namespace PaymentModule.Repositories;

public interface IPaymentRepository
{
  public Task<bool> SavePaymentTransaction(PaymentsModel model);
  
}