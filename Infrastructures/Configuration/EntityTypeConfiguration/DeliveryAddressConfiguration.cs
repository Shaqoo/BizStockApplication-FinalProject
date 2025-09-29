namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DeliveryAddressConfiguration : IEntityTypeConfiguration<DeliveryAddress>
    {
        public void Configure(EntityTypeBuilder<DeliveryAddress> builder)
        {
            builder.ToTable("DeliveryAddresses");

           
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Street)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(d => d.Landmark)
                   .HasMaxLength(250)
                   .IsUnicode(false)   
                   .IsRequired(false);

            builder.Property(d => d.PostalCode)
                   .HasMaxLength(20)
                   .IsRequired(false);

            builder.Property(d => d.IsDefault)
                   .HasDefaultValue(false);


            builder.HasOne(d => d.Customer)
                   .WithMany(c => c.DeliveryAddresses)   
                   .HasForeignKey(d => d.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(d => d.State)
                   .WithMany() 
                   .HasForeignKey(d => d.StateId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne(d => d.Lga)
                   .WithMany() 
                   .HasForeignKey(d => d.LgaId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasIndex(d => new { d.CustomerId, d.IsDefault })
                   .HasDatabaseName("IX_Customer_DefaultDeliveryAddress");

            builder.HasIndex(d => new { d.CustomerId, d.IsDefault })
                     .HasDatabaseName("IX_DeliveryAddress_UniqueDefault")
                     .HasFilter("\"IsDefault\" = true")
                     .IsUnique();

        }
    }

}
