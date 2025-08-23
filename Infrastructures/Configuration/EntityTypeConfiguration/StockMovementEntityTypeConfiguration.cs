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
    public class StockMovementEntityTypeConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.ToTable("StockMovements");

            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.QuantityChanged)
                   .IsRequired();

            builder.Property(sm => sm.Reason)
                   .HasMaxLength(300)
                   .IsRequired();

            builder.Property(sm => sm.MovementType)
                   .HasConversion<string>() 
                   .IsRequired();

            builder.HasOne(sm => sm.WarehouseItem)
                   .WithMany() 
                   .HasForeignKey(sm => sm.WarehouseItemId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sm => sm.PerformedByUser)
                   .WithMany() 
                   .HasForeignKey(sm => sm.PerformedByUserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
