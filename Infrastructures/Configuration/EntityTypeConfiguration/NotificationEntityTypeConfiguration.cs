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
    public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.RecipientId)
                   .IsRequired();

            builder.Property(n => n.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(n => n.Message)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(n => n.IsRead)
                   .IsRequired();

            builder.Property(n => n.LinkUrl)
                   .HasMaxLength(500);

            builder.HasOne(n => n.Recipient)
                   .WithMany(u => u.Notifications) 
                   .HasForeignKey(n => n.RecipientId)
                   .OnDelete(DeleteBehavior.Cascade);

             
            builder.HasIndex(n => new { n.RecipientId, n.IsRead });

        }
    }
}
