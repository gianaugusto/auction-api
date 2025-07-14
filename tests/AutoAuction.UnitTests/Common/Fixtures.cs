using AutoAuction.Domain;

namespace AutoAuction.UnitTests.Common
{
    public static class Fixtures
    {
        public static Auction CreateTestAuction()
        {
            return new Auction
            {
                Name = "Test Auction",
                StartingBid = 100m,
                CurrentBid = 100m,
                IsActive = true
            };
        }
    }
}
