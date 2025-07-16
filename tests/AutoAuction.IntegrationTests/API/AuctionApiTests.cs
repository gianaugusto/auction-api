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
using System.Collections.Generic;

namespace AutoAuction.IntegrationTests.API
{
    public class AuctionApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuctionApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task StartAuction_ValidDto_ShouldReturnSuccess()
        {
            // Arrange
            var startAuctionDto = new StartAuctionDto
            {
                VehicleId = Guid.NewGuid().ToString()
            };

            var content = new StringContent(JsonConvert.SerializeObject(startAuctionDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auction", content, TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Auction started successfully", responseString);
        }

        [Fact]
        public async Task StartAuction_NullDto_ShouldReturnBadRequest()
        {
            // Act
            var response = await _client.PostAsync("/api/auction", null, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("StartAuctionDto cannot be null", responseString);
        }

        [Fact]
        public async Task PlaceBid_ValidDto_ShouldReturnSuccess()
        {
            // Arrange
            var auctionId = 1; // Assuming there's at least one auction with ID 1
            var placeBidDto = new PlaceBidDto
            {
                BidderId = Guid.NewGuid().ToString(),
                BidAmount = 1000m
            };

            var content = new StringContent(JsonConvert.SerializeObject(placeBidDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/auction/{auctionId}/bids", content, TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Bid placed successfully", responseString);
        }

        [Fact]
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

        [Fact]
        public async Task CloseAuction_ValidId_ShouldReturnSuccess()
        {
            // Arrange
            var auctionId = 1; // Assuming there's at least one auction with ID 1

            // Act
            var response = await _client.PostAsync($"/api/auction/{auctionId}/close", null, TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Auction closed successfully", responseString);
        }

        [Fact]
        public async Task CloseAuction_InvalidId_ShouldReturnBadRequest()
        {
            // Act
            var response = await _client.PostAsync($"/api/auction/-1/close", null, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Auction ID must be greater than zero", responseString);
        }

        [Fact]
        public async Task GetActiveAuctions_ShouldReturnSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/auction/active", TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("Active auctions retrieved successfully", responseString);
        }
    }
}
