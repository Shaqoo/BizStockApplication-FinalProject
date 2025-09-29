using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class WalletTransactionEntityTypeConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.ToTable("WalletTransactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(t => t.Type)
                   .HasConversion<string>()  
                   .IsRequired();

            builder.Property(t => t.Reference)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(t => t.Description)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(t => t.DateCreated)
                   .IsRequired();

            builder.HasOne(t => t.Wallet)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(t => t.WalletId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
