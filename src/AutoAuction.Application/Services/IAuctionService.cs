using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoAuction.Domain;

namespace AutoAuction.Application
{
    public interface IAuctionService
    {
        Task StartAuctionAsync(string vehicleId, CancellationToken cancellationToken = default);

        Task PlaceBidAsync(int auctionId, string bidderId, decimal bidAmount, CancellationToken cancellationToken = default);

        Task CloseAuctionAsync(int auctionId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Auction>> GetActiveAuctionsAsync(bool active = true, CancellationToken cancellationToken = default);
    }
}
