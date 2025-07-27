using Microsoft.EntityFrameworkCore;
using RinhaDeBackend.Domain;

namespace RinhaDeBackend.Infra.Repositories
{
    public class PaymentRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public PaymentRepository(IDbContextFactory<AppDbContext> dbContext)
        {
            _dbContextFactory = dbContext;
        }

        public async Task<Result<Payment, string>> InsertAsync(Payment dto)
        {
            try
            {
                using var context = await _dbContextFactory.CreateDbContextAsync();
                await context.Payments.AddAsync(dto);
                await context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<Result<List<Payment>, string>> ReadAll(DateTime? from, DateTime? to)
        {
            try
            {
                from ??= DateTime.MinValue;
                to ??= DateTime.MaxValue;

                using var context = await _dbContextFactory.CreateDbContextAsync();
                return await context.Payments.Where(x=> x.requestedAt >= from && x.requestedAt <= to) .ToListAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
