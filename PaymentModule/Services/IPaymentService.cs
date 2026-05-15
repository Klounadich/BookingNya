using PaymentModule.Commands;

namespace PaymentModule.Services;

public interface IPaymentService
{
    public Task<PaymentResult> ProcessPaymentAsync(ProcessPaymentCommand data);

    public Task<bool> WriteMoney(Guid sagaId);
    
    public Task<bool> MoneyBack(Guid sagaId);


}