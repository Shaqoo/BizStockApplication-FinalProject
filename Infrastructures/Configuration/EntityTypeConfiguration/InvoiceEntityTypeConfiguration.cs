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
    public class InvoiceEntityTypeConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.InvoiceNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(i => i.InvoiceNumber)
                   .IsUnique();

            builder.Property(i => i.CustomerId)
                   .IsRequired();

            builder.Property(i => i.DueDate);

            builder.Property(i => i.SubTotal)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.Discount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.Tax)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Ignore(i => i.TotalAmount);  

            builder.HasOne(i => i.Customer)
                   .WithMany(c => c.Invoices) 
                   .HasForeignKey(i => i.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.SalesOrder)
                   .WithOne(so => so.Invoice) 
                   .HasForeignKey<Invoice>(i => i.SalesOrderId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(i => i.Payments)
                   .WithOne(p => p.Invoice)
                   .HasForeignKey(p => p.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
