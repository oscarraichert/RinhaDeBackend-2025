using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RinhaDeBackend.Domain;

namespace RinhaDeBackend.Infra
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            
        }

        public DbSet<ProcessPaymentDto> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
