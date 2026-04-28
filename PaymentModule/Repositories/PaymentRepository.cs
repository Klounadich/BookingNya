
using Microsoft.EntityFrameworkCore;
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

    public async Task<PaymentsModel?> GetTransaction(Guid sagaId)
    {
       return await _context.Payments.Where(x => x.saga_id == sagaId).AsNoTracking().SingleAsync();
    }

    public async Task<bool> UpdateTransaction(PaymentsModel model)
    {
        _context.Payments.Update(model);
        return await _context.SaveChangesAsync() > 0;
    }
}