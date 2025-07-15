using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoAuction.Domain.Repositories
{
    public interface IAuctionRepository
    {
        Task AddAuctionAsync(Auction auction, CancellationToken cancellationToken = default);

        Task<Auction> GetAuctionByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Auction>> GetAllAuctionsAsync(bool active = true, CancellationToken cancellationToken = default);
    }
}
