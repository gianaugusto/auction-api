using Microsoft.AspNetCore.Mvc;

namespace AutoAuction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Auction API is running");
        }
    }
}
