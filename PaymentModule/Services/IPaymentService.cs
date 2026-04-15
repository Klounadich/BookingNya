using PaymentModule.Commands;

namespace PaymentModule.Services;

public interface IPaymentService
{
    public Task<PaymentResult> ProcessPaymentAsync(ProcessPaymentCommand data);
    
}