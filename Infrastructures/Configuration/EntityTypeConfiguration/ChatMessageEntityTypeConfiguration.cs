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
    public class ChatMessageEntityTypeConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.ToTable("ChatMessages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ChatThreadId)
                   .IsRequired();

            builder.Property(m => m.SenderId)
                   .IsRequired();

            builder.Property(m => m.Message)
                   .HasMaxLength(1000);

            builder.Property(m => m.AudioUrl)
                   .HasMaxLength(500);

            builder.Property(m => m.PictureUrl)
                   .HasMaxLength(500);

            builder.Property(m => m.IsRead)
                   .IsRequired();

            builder.Property(m => m.SentAt)
                   .IsRequired();

            builder.HasOne(m => m.ChatThread)
                   .WithMany(t => t.Messages)
                   .HasForeignKey(m => m.ChatThreadId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Sender)
                   .WithMany(u => u.ChatMessages) 
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.RepliedToMessage)
                   .WithMany()
                   .HasForeignKey(m => m.RepliedToMessageId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(m => m.Reactions)
                   .WithOne(r => r.Message)
                   .HasForeignKey(r => r.MessageId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
