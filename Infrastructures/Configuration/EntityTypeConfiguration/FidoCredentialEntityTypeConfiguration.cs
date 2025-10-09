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
    public class FidoCredentialEntityTypeConfiguration : IEntityTypeConfiguration<FidoCredential>
    {
        public void Configure(EntityTypeBuilder<FidoCredential> builder)
        {
            builder.ToTable("FidoCredentials");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                   .IsRequired();

            builder.Property(c => c.CredentialId)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasIndex(c => c.CredentialId)
                   .IsUnique();

            builder.Property(c => c.PublicKey)
                   .IsRequired()
                   .HasMaxLength(2048);

            builder.Property(c => c.SignatureCounter)
                   .IsRequired();

            builder.Property(c => c.AuthenticatorAAGUID)
                   .IsRequired()
                   .HasMaxLength(128);

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.HasOne(c => c.User)
                   .WithMany(u => u.FidoCredentials)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.CredentialId)
                   .IsUnique();

        }
    }
}
