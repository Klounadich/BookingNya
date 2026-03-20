
using PaymentModule.Infrastructure;
using PaymentModule.Models;


namespace PaymentModule.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }
        
    public async Task<bool> SavePaymentTransaction(PaymentsModel model)
    {
        await _context.Payments.AddAsync(model);
        if (await _context.SaveChangesAsync() > 0)
        {
            return true;
        }
        return false;
    }
}