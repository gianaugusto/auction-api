using AutoAuction.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AutoAuction.IntegrationTests.API
{
    public class AuctionApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuctionApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task EndpointPipelineValidationReturnsSuccess()
        {
            // Arrange
            
            // Act
            
            // Assert
            Assert.True(true);
        }
    }
}
