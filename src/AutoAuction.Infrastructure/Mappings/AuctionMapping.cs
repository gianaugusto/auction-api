using AutoAuction.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoAuction.Infrastructure.Mappings
{
    public class AuctionMapping : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.CurrentHighestBid).IsRequired();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.RowVersion).IsRowVersion().IsConcurrencyToken();

            builder.HasOne(a => a.Vehicle)
                   .WithMany()
                   .HasForeignKey("VehicleId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsMany(a => a.Bids, b =>
            {
                b.WithOwner().HasForeignKey("AuctionId");

                b.HasKey("AuctionId", "BidderId", "BidTime");
                b.Property(b => b.BidderId).IsRequired().HasMaxLength(50);
                b.Property(b => b.BidAmount).IsRequired();
                b.Property(b => b.BidTime).IsRequired();
            });
        }
    }
}
