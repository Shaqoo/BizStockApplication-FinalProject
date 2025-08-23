using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class DeliveryFeeRuleEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryFeeRule>
    {
        public void Configure(EntityTypeBuilder<DeliveryFeeRule> builder)
        {
            builder.ToTable("DeliveryFeeRules");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(r => r.Zone)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(r => r.FlatRate)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(r => r.MinOrderAmountForFree)
                   .HasColumnType("decimal(18,2)");

            builder.Property(r => r.IsActive)
                   .IsRequired();

            builder.Property(r => r.CreatedAt)
                   .IsRequired();

            builder.Property(r => r.Note)
                   .HasMaxLength(1000);

        }
    }
}
