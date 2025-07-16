using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoAuction.Domain.Repositories
{
    public interface IAuctionRepository
    {
        Task AddAuctionAsync(Auction auction, CancellationToken cancellationToken = default);

        Task<Auction> GetAuctionByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Auction>> GetAllAuctionsAsync(bool isActive, CancellationToken cancellationToken = default);

        Task PlaceBidAsync(int auctionId, string bidderId, decimal bidAmount, CancellationToken cancellationToken = default);

        Task StartAuctionForVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken = default);

        Task CloseAuctionAsync(int auctionId, CancellationToken cancellationToken = default);

    }
}
