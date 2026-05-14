using AuthModule.Commands;
using AuthModule.Services;
using DotNetCore.CAP;
using Npgsql.Replication.PgOutput.Messages;
using PaymentModule.Commands;
using PaymentModule.Models;
using PaymentModule.Repositories;
using Shared.Enums;

namespace PaymentModule.Services;

public class Mock
{
    private readonly IAuthService  _authService;

    public Mock(IAuthService authService)
    {
         _authService = authService;
    }
    public record CheckResult(Guid? Id, decimal Amount , string Currency, string Status);
    public  CheckResult PaymentCheck(decimal amount, string currency ,string method , string userId)
    {
        if (amount <= 0)
        {
            return new CheckResult(Guid.Empty, 0, currency, "Invalid amount");
        }

        if (method == "BookingPay")
        {
            
            var balance = _authService.GetBalanceAsync(new GetBalanceQuery(userId))
                .GetAwaiter()
                .GetResult();
            if (amount <= balance)
            {
                return new CheckResult(Guid.NewGuid(), amount, currency, "Success");
            }
            return new CheckResult(Guid.Empty, 0, currency, "Invalid amount");
        }
        else
        {
            return new CheckResult(Guid.NewGuid(), amount, currency, "Success");
        }
        
    }
    
    
}

public class PaymentService : IPaymentService
{
    private readonly Mock _mock;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IAuthService  _authService;

    public PaymentService(Mock mock , IPaymentRepository paymentRepository , IAuthService authService)
    {
        _paymentRepository = paymentRepository;
        _mock = mock;
        _authService = authService;
    }
    public async Task<PaymentResult> ProcessPaymentAsync(ProcessPaymentCommand data)
    {
      
        var checkvalue =  _mock.PaymentCheck(data.Amount, data.Currency , data.PaymentMethod , data.CustomerId); //mock of external api for payment
        if (checkvalue.Status == "Success")
        {
            PaymentsModel booking = new PaymentsModel
            {
                id = Guid.NewGuid(),
                saga_id = data.SagaId,
                amount = checkvalue.Amount,
                booking_id = data.BookingId,
                transaction_id = checkvalue.Id.ToString(),
                payment_method = data.PaymentMethod,
                currency = checkvalue.Currency,
                status = PaymentStatus.Authorized,
                authorized_at = DateTime.UtcNow,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                customer_id = data.CustomerId,
                customer_email = data.CustomerEmail,
            };
                
            if (await _paymentRepository.SavePaymentTransaction(booking))
            {
                return new PaymentResult(true, "");
            }
        }
        return new PaymentResult(false, checkvalue.Status);
    }

    public async Task<bool> WriteMoney(Guid sagaId )
    {
        var transaction = await _paymentRepository.GetTransaction(sagaId);
        if (transaction !=null)
        {
            if (transaction.payment_method == "BookingPay")
            {
               await _authService.WriteMoneyFromBalance(transaction.customer_id , transaction.amount);
            }
            transaction.status = PaymentStatus.Captured;
            transaction.updated_at = DateTime.UtcNow;
           return await _paymentRepository.UpdateTransaction(transaction);
            
        }
        return false;
        
    }

    public async Task<bool> MoneyBack(Guid sagaId)
    {
        var transaction = await _paymentRepository.GetTransaction(sagaId);
        if (transaction != null)
        {
            if (transaction.payment_method == "BookingPay" && transaction.status == PaymentStatus.Captured)
            {
                await _authService.RefundMoneyToBalance(transaction.customer_id , transaction.amount);
            }
            
            transaction.status = PaymentStatus.Refunded;
            transaction.updated_at = DateTime.UtcNow;
            transaction.refunded_at = DateTime.UtcNow;
            return await _paymentRepository.UpdateTransaction(transaction);
        }
        return false;
    }
}