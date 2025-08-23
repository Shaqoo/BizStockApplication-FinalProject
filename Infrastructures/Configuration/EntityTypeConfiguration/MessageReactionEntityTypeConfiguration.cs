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
    public class MessageReactionEntityTypeConfiguration : IEntityTypeConfiguration<MessageReaction>
    {
        public void Configure(EntityTypeBuilder<MessageReaction> builder)
        {
            builder.ToTable("MessageReactions");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.MessageId)
                   .IsRequired();

            builder.Property(r => r.ReactedByUserId)
                   .IsRequired();

            builder.Property(r => r.Emoji)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(r => r.ReactedAt)
                   .IsRequired();

            builder.HasOne(r => r.Message)
                   .WithMany(m => m.Reactions) 
                   .HasForeignKey(r => r.MessageId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.ReactedBy)
                   .WithMany(u => u.Reactions) 
                   .HasForeignKey(r => r.ReactedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

           
            builder.HasIndex(r => new { r.MessageId, r.ReactedByUserId, r.Emoji })
                   .IsUnique();

        }
    }
}
