using Bookworm.Models;
using Bookworm.Service;
using Microsoft.AspNetCore.Mvc;

namespace Bookworm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyShelfController : ControllerBase
    {
        private readonly IMyShelfService _myShelfService;

        public MyShelfController(IMyShelfService myShelfService)
        {
            _myShelfService = myShelfService;
        }

        [HttpGet("shelf-details")]
        public async Task<ActionResult<List<ShelfDetail>>> GetShelfDetailList()
        {
            var shelfDetails = await _myShelfService.GetShelfDetailListAsync();
            return Ok(shelfDetails);
        }

        [HttpGet("shelf-detail/{id}")]
        public async Task<ActionResult<ShelfDetail>> GetShelfDetail(int id)
        {
            var shelfDetail = await _myShelfService.GetShelfDetailAsync(id);
            if (shelfDetail == null)
            {
                return NotFound();
            }
            return Ok(shelfDetail);
        }

        [HttpPost("add-shelf-detail")]
        public async Task<ActionResult<ShelfDetail>> AddShelfDetail(int cartDetailId)
        {
            var shelfDetail = await _myShelfService.AddShelfDetailAsync(cartDetailId);
            return CreatedAtAction(nameof(GetShelfDetail), new { id = shelfDetail }, shelfDetail);
        }

        [HttpPost("update-shelf-details/{cartId}")]
        public async Task<IActionResult> UpdateShelfDetails(int cartId)
        {
            await _myShelfService.UpdateShelfDetailsAsync(cartId);
            return NoContent();
        }
    }
}
