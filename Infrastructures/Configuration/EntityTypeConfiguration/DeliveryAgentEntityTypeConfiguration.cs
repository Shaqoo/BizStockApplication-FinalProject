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
    public class DeliveryAgentEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryAgent>
    {
        public void Configure(EntityTypeBuilder<DeliveryAgent> builder)
        {
            builder.ToTable("DeliveryAgents");

            builder.HasKey(da => da.Id);

             
            builder.OwnsOne(da => da.Email, email =>
            {
                email.Property(e => e.Value)
                     .HasColumnName("Email")
                     .IsRequired()
                     .HasMaxLength(150);
                email.HasIndex(e => e.Value).IsUnique();
            });

            builder.Property(da => da.VehicleNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(da => da.ContactNumber)
                   .HasMaxLength(20);

            builder.Property(da => da.AvailabilityStatus)
                   .HasConversion<string>() 
                   .IsRequired();

            builder.Metadata
                   .FindNavigation(nameof(DeliveryAgent.Assignments))!
                   .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany<DeliveryAssignment>()
                   .WithOne(a => a.DeliveryAgent)
                   .HasForeignKey(a => a.DeliveryAgentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(da => da.Reviews)
                   .WithOne(r => r.DeliveryAgent)
                   .HasForeignKey(r => r.DeliveryAgentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
