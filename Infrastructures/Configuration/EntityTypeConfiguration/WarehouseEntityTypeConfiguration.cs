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
    public class WarehouseEntityTypeConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(w => w.Location)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(w => w.IsActive)
                   .IsRequired();

            
            builder.HasMany(w => w.Items)
                   .WithOne(i => i.Warehouse) 
                   .HasForeignKey(i => i.WarehouseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
