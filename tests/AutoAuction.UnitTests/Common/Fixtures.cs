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
            return new Hatchback(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                2020, // Set a valid year
                5000m, // Set a valid starting bid
                5 // Set a valid number of doors
            );
        }

        public static Sedan GetSedan()
        {
            return new Sedan(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                2020, // Set a valid year
                5000m, // Set a valid starting bid
                4 // Set a valid number of doors
            );
        }

        public static SUV GetSUV()
        {
            return new SUV(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                2020, // Set a valid year
                5000m, // Set a valid starting bid
                4 // Set a valid number of doors
            );
        }

        public static Truck GetTruck()
        {
            return new Truck(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                2020, // Set a valid year
                5000m, // Set a valid starting bid
                2 // Set a valid number of doors
            );
        }
    }
}
