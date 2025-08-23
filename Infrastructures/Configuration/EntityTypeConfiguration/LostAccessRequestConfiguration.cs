using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EntityTypeConfiguration
{
    public class LostAccessRequestConfiguration : IEntityTypeConfiguration<LostAccessRequest>
    {
        public void Configure(EntityTypeBuilder<LostAccessRequest> builder)
        {
            builder.ToTable("LostAccessRequests");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserIdentifier)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.AlternateEmail)
                .HasMaxLength(255);

            builder.Property(x => x.AlternatePhone)
                .HasMaxLength(20);

            builder.Property(x => x.ProblemDescription)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.AdminNotes)
                .HasMaxLength(1000);

            builder.Property(x => x.SubmittedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);
        }
    }
}
