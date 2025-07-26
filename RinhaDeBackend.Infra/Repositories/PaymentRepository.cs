using Microsoft.EntityFrameworkCore;
using RinhaDeBackend.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RinhaDeBackend.Infra.Repositories
{
    public class PaymentRepository
    {
        private AppDbContext DbContext { get; }
        private DbSet<ProcessPaymentDto> Payments { get; }

        public PaymentRepository(AppDbContext dbContext)
        {
            DbContext = dbContext;
            Payments = dbContext.Payments;
        }

        public async Task<Result<ProcessPaymentDto>> InsertAsync(ProcessPaymentDto dto)
        {
            try
            {
                await Payments.AddAsync(dto);
                await DbContext.SaveChangesAsync();

                return Result<ProcessPaymentDto>.Success(dto);
            }
            catch (Exception ex) {
                return Result<ProcessPaymentDto>.Error(ex.Message);
            }
        }
    }
}
