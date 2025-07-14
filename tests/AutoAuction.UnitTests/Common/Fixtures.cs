using AutoAuction.Domain;

namespace AutoAuction.UnitTests.Common
{
    public static class Fixtures
    {
        public static Hatchback GetHatchback()
        {
            return new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
        }

        public static Sedan GetSedan()
        {
            return new Sedan("V002", "Honda", "Civic", 2019, 6000m, 4);
        }

        public static SUV GetSUV()
        {
            return new SUV("V003", "Ford", "Explorer", 2018, 7000m, 7);
        }

        public static Truck GetTruck()
        {
            return new Truck("V004", "Chevrolet", "Silverado", 2017, 8000m, 1.5m);
        }
    }
}
