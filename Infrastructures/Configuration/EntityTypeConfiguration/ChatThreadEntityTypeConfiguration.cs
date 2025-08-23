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
    public class ChatThreadEntityTypeConfiguration : IEntityTypeConfiguration<ChatThread>
    {
        public void Configure(EntityTypeBuilder<ChatThread> builder)
        {
            builder.ToTable("ChatThreads");

            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.CustomerId)
                   .IsRequired();

            builder.Property(ct => ct.AssignedAgentId)
                   .IsRequired(false);

            builder.Property(ct => ct.Status)
                   .HasConversion<string>() 
                   .IsRequired();

            builder.HasOne(ct => ct.Customer)
                   .WithMany(c => c.ChatThreads)
                   .HasForeignKey(ct => ct.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ct => ct.AssignedAgent)
                   .WithMany() 
                   .HasForeignKey(ct => ct.AssignedAgentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(ct => ct.Messages)
                   .WithOne(m => m.ChatThread)
                   .HasForeignKey(m => m.ChatThreadId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
