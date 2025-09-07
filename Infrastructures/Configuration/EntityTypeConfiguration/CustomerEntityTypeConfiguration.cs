using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);


            builder.Property(u => u.Email)
                 .HasConversion(
                     v => v.Value,
                     v => new Email(v)
                 )
                 .HasColumnName("Email")
                 .IsRequired()
                 .HasMaxLength(100);

            builder.Property(c => c.CustomerTypeId)
                   .IsRequired();

            builder.Property(c => c.BusinessName)
                   .HasMaxLength(200);

            builder.Property(c => c.Address)
                   .HasMaxLength(300);

            builder.Property(c => c.State)
                   .HasMaxLength(100);

            builder.Property(c => c.TaxId)
                   .HasMaxLength(50);

            
            builder.HasOne(c => c.CustomerType)
                   .WithMany(ct => ct.Customers)  
                   .HasForeignKey(c => c.CustomerTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.ChatThreads)
                   .WithOne(ct => ct.Customer)
                   .HasForeignKey(ct => ct.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.SalesOrders)
                   .WithOne(so => so.Customer)
                   .HasForeignKey(so => so.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Invoices)
                   .WithOne(i => i.Customer)
                   .HasForeignKey(i => i.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}

