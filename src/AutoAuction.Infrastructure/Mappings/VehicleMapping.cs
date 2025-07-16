using AutoAuction.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoAuction.Infrastructure.Mappings
{
    public class VehicleMapping : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasDiscriminator<string>("VehicleType")
                .HasValue<Hatchback>("Hatchback")
                .HasValue<Sedan>("Sedan")
                .HasValue<SUV>("SUV")
                .HasValue<Truck>("Truck");

            builder.Property(v => v.Id)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.Manufacturer)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Year)
                .IsRequired();

            builder.Property(v => v.StartingBid)
                .IsRequired();
        }
    }
}
