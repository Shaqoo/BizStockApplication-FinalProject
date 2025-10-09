using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable("Users")
                .HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.SearchVector)
            .HasColumnType("tsvector")
            .HasComputedColumnSql(
                "to_tsvector('english', coalesce(\"FullName\", '') || ' ' || coalesce(\"Email\", ''))",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();


            builder.Property(u => u.Email)
                .HasConversion(
                    v => v.Value,            
                    v => new Email(v)        
                )
                .HasColumnName("Email")      
                .IsRequired()
                .HasMaxLength(100);


            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(400);

            builder.Property(u => u.HashSalt)
                .IsRequired()
                .HasMaxLength(400);

            builder.Property(u => u.IsEmailVerified)
                .IsRequired();

            builder.Property(u => u.IsPhoneNumberVerified)
                .IsRequired();

            builder.Property(u => u.TwoFactorSecret)
                .HasConversion(
                    v => v.Secret,
                    v => new TwoFactorSecret(v)
                )
                .HasMaxLength(500);

            builder.Property(u => u.PhoneNumber)
                .HasConversion(
                    v => v.Value,
                    v => new PhoneNumber(v)
                )
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(u => u.DateCreated)
                .IsRequired();

            builder.Property(u => u.DateOfBirth)
                .HasConversion(
                    v => v.Value.ToString("yyyy-MM-dd"),
                    v => new DateOfBirth(DateTime.Parse(v))
                )
                .HasMaxLength(30);

            builder.Property(u => u.Gender)
               .HasConversion<string>() 
               .IsRequired();

            builder.Property(u => u.ProfilePictureUrl)
                .HasMaxLength(500);

            builder.HasMany(u => u.ChatMessages)
                .WithOne(cm => cm.Sender)
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Reactions)
                .WithOne(r => r.ReactedBy)
                .HasForeignKey(r => r.ReactedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata
                .FindNavigation(nameof(User.FidoCredentials))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(a => a.Email)
                  .IsUnique();

            builder.HasIndex(a => a.SearchVector)
                    .HasMethod("GIN");

            builder.Metadata
            .FindNavigation(nameof(User.UserRoles))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);


            builder.Navigation(u => u.RecoveryCodes).Metadata.SetField("_recoveryCodes");
            builder.Navigation(u => u.RecoveryCodes).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
