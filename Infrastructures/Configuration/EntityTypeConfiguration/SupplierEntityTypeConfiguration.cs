using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class SupplierEntityTypeConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(cp => cp.Id);

            builder.Property(cp => cp.CompanyName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(cp => cp.Address)
                   .HasMaxLength(250);

            builder.Property(cp => cp.ContactPerson)
                   .HasMaxLength(100);

            builder.Property(cp => cp.PhoneNumber)
                    .HasConversion(
                        v => v.Value,
                        v => new PhoneNumber(v)
                    )
                   .HasMaxLength(20);

            builder.Property(cp => cp.Email)
                   .HasConversion(
                       v => v.Value,
                       v => new Email(v)
                   )
                  .HasMaxLength(50);

            builder.Property(cp => cp.TaxId)
                   .HasMaxLength(50);

            builder.HasIndex(cp => cp.UserId).IsUnique();
        }
    }
}
