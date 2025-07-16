using AutoAuction.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using AutoAuction.Application.DTOs;
using System.Net;
using AutoAuction.Domain;

namespace AutoAuction.IntegrationTests.API
{
    public class InventoryApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public InventoryApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact()]
        public async Task AddVehicle_ValidVehicle_ShouldReturnSuccess()
        {
            // Arrange
            var vehicleDto = new VehicleDto
            {
                Id = Guid.NewGuid().ToString(),
                Type = VehicleType.Sedan,
                Manufacturer = "Toyota",
                Model = "Corolla",
                Year = 2020,
                StartingBid = 15000,
                NumberOfDoors=5
            };

            var content = new StringContent(JsonConvert.SerializeObject(vehicleDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/inventory/vehicles", content, TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Vehicle added successfully", responseString);
        }

        [Fact]
        public async Task SearchVehicles_ValidQuery_ShouldReturnSuccess()
        {
            // Arrange & Act
            var response = await _client.GetAsync("/api/inventory/vehicles?type=Sedan&manufacturer=Toyota&model=Corolla&year=2020", TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Vehicles retrieved successfully", responseString);
        }
    }
}
