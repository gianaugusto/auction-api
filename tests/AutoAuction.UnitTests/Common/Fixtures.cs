using AutoAuction.Domain;
using AutoFixture;

namespace AutoAuction.UnitTests.Common
{
    public static class Fixtures
    {
        private static readonly IFixture Fixture = new Fixture();

        public static T Create<T>() where T : class
        {
            return Fixture.Create<T>();
        }

        public static Hatchback GetHatchback()
        {
            return Create<Hatchback>();
        }

        public static Sedan GetSedan()
        {
            return Create<Sedan>();
        }

        public static SUV GetSUV()
        {
            return Create<SUV>();
        }

        public static Truck GetTruck()
        {
            return Create<Truck>();
        }
    }
}
