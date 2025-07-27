using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RinhaDeBackend.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RinhaDeBackend.Infra.Configs
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(b => b.correlationId);
            builder.Property(b => b.amount);
            builder.Property(b => b.requestedAt);
            builder.Property(b => b.processedOnFallback);
        }
    }
}
