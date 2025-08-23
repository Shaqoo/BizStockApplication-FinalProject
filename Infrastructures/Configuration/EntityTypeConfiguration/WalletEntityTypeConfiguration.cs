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
    public class WalletEntityTypeConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
             builder.ToTable("Wallets");

             builder.HasKey(w => w.Id);

             builder.Property(u => u.Balance)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

            builder.Property(u => u.IsActive)
                .IsRequired();

            builder.Metadata
               .FindNavigation(nameof(Wallet.Transactions))!
               .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(w => w.User)
               .WithOne(u => u.Wallet)  
               .HasForeignKey<Wallet>(w => w.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
