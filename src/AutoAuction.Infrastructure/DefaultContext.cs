using AutoAuction.Domain;
using AutoAuction.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace AutoAuction.Infrastructure
{
    public class DefaultContext : DbContext
    {
        public DbSet<Auction> Auctions { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AuctionMapping());
            modelBuilder.ApplyConfiguration(new VehicleMapping());
        }
    }
}
