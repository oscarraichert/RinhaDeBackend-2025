using Microsoft.EntityFrameworkCore;
using RinhaDeBackend.Domain;

namespace RinhaDeBackend.Infra.Repositories
{
    public class PaymentRepository
    {
        private AppDbContext DbContext { get; }
        private DbSet<Payment> Payments { get; }

        public PaymentRepository(AppDbContext dbContext)
        {
            DbContext = dbContext;
            Payments = dbContext.Payments;
        }

        public async Task<Result<Payment, string>> InsertAsync(Payment dto)
        {
            try
            {
                await Payments.AddAsync(dto);
                await DbContext.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Result<List<Payment>, string> ReadAll()
        {
            try
            {
                return Payments.ToList();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
