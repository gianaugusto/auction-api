using AutoAuction.Domain;

namespace AutoAuction.UnitTests.Common
{
    public static class Fixtures
    {
        public static Auction CreateTestAuction()
        {
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            return new Auction(vehicle);
        }
    }
}
