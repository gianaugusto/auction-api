using AutoAuction.API;
using AutoAuction.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using AutoAuction.Application.DTOs;
using System.Net;
using AutoAuction.Domain;
using System.Collections.Generic;

namespace AutoAuction.IntegrationTests.API
{
    public class AuctionApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DefaultContext _context;
        private readonly IntegrationTestSeeder _seeder;

        public AuctionApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            _seeder = new IntegrationTestSeeder(_context);
        }

        [Fact()]
        public async Task StartAuction_ValidDto_ShouldReturnSuccess()
        {
            // Arrange
             var vehicle = await _seeder.AddVehicleAsync();

            var startAuctionDto = new StartAuctionDto
            {
                VehicleId = vehicle.Id
            };

            var content = new StringContent(JsonConvert.SerializeObject(startAuctionDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auction", content, TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Auction started successfully", responseString);
        }

        [Fact()]
        public async Task StartAuction_NullDto_ShouldReturnBadRequest()
        {
            // Arrange & Act
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/auction", content, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("The VehicleId field is required", responseString);
        }

        [Fact()]
        public async Task PlaceBid_ValidDto_ShouldReturnSuccess()
        {
            // Arrange
            await _seeder.AddVehiclesAsync();
            await _seeder.AddAuctionsAsync();
            var auction = await _seeder.GetAnyActiveAuctionsAsync();

            var placeBidDto = new PlaceBidDto
            {
                BidderId = Guid.NewGuid().ToString(),
                BidAmount = auction.CurrentHighestBid + 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(placeBidDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/auction/{auction.Id}/bids", content, TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Bid placed successfully", responseString);
        }

        [Fact()]
        public async Task PlaceBid_ValidDto_ShouldIncreaseHighestBid()
        {
            // Arrange
            await _seeder.AddVehiclesAsync();
            await _seeder.AddAuctionsAsync();
            var auction = await _seeder.GetAnyActiveAuctionsAsync();
            var initialHighestBid = auction.CurrentHighestBid;

            var placeBidDto = new PlaceBidDto
            {
                BidderId = Guid.NewGuid().ToString(),
                BidAmount = initialHighestBid + 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(placeBidDto), Encoding.UTF8, "application/json");

            // Act
            await _client.PostAsync($"/api/auction/{auction.Id}/bids", content, TestContext.Current.CancellationToken);
            var updatedAuction = await _seeder.GetAuctionsByIdAsync(auction.Id);

            // Assert
            Assert.True(updatedAuction.CurrentHighestBid > initialHighestBid);
        }

        [Fact()]
        public async Task PlaceBid_InvalidAuctionId_ShouldReturnBadRequest()
        {
            // Arrange
            var placeBidDto = new PlaceBidDto
            {
                BidderId = Guid.NewGuid().ToString(),
                BidAmount = 1000m
            };

            var content = new StringContent(JsonConvert.SerializeObject(placeBidDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/auction/-1/bids", content, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Auction ID must be greater than zero", responseString);
        }

        [Fact()]
        public async Task CloseAuction_ValidId_ShouldReturnSuccess()
        {
            // Arrange
            await _seeder.AddVehiclesAsync();
            await _seeder.AddAuctionsAsync();
            var auction = await _seeder.GetAnyActiveAuctionsAsync();

            // Act
            var response = await _client.PostAsync($"/api/auction/{auction.Id}/close", null, TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Auction closed successfully", responseString);
        }

        [Fact()]
        public async Task CloseAuction_InvalidId_ShouldReturnBadRequest()
        {
            // Arrange & Act
            var response = await _client.PostAsync($"/api/auction/-1/close", null, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Auction ID must be greater than zero", responseString);
        }

        [Fact()]
        public async Task GetActiveAuctions_ShouldReturnSuccess()
        {
            // Arrange 
            await _seeder.AddVehiclesAsync();
            await _seeder.AddAuctionsAsync();

            // Arrange & Act
            var response = await _client.GetAsync("/api/auction/active", TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Active auctions retrieved successfully", responseString);
        }
    }
}
