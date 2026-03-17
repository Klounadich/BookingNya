using PaymentModule.Commands;

namespace PaymentModule.Services;

public interface IPaymentService
{
    public Task<bool> ProcessPaymentAsync(ProcessPaymentCommand data);
}