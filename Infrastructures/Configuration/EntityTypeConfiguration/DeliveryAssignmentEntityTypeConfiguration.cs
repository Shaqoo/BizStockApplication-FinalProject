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
    public class DeliveryAssignmentEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryAssignment>
    {
        public void Configure(EntityTypeBuilder<DeliveryAssignment> builder)
        {
            builder.ToTable("DeliveryAssignments");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.SalesOrderId)
                   .IsRequired();

            builder.Property(d => d.DeliveryAgentId);

            builder.Property(d => d.DeliveredAt);

            builder.Property(d => d.Status)
                   .HasConversion<string>() 
                   .IsRequired();

            builder.Property(d => d.DeliveryFee)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(d => d.Note)
                   .HasMaxLength(1000);

            builder.HasOne(d => d.SalesOrder)
                   .WithOne(so => so.DeliveryAssignment) 
                   .HasForeignKey<DeliveryAssignment>(d => d.SalesOrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.DeliveryAgent)
                   .WithMany(a => a.Assignments) 
                   .HasForeignKey(d => d.DeliveryAgentId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasIndex(d => new { d.DeliveryAgentId, d.Status });
        }
    }
}
 