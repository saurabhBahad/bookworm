namespace Bookworm.Controllers;

using bookworm.Dto;
using Bookworm.Models;
using Bookworm.Service;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<List<CartDetail>>> GetCartDetailsAsync(int customerId)
    {
        try
        {
            var cartDetails = await _cartService.GetDetailsAsync(customerId);
            //Console.WriteLine(cartDetails.Any());
            return Ok(cartDetails);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{customerId}/Product/{productId}")]
    public async Task<IActionResult> RemoveCartDetailAsync(int customerId, int productId)
    {
        try
        {
            await _cartService.RemoveProductAsync(customerId, productId);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Cart")]
    public async Task<IActionResult> AddProductToCartAsync([FromBody] CartHelper cartHelper)
    {
        Console.WriteLine(cartHelper);
        try
        {
            await _cartService.AddProductAsync(cartHelper);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Update")]
    public async Task<IActionResult> updateProductDetails([FromBody]CartDetail cartDetail)
    {
        if (cartDetail == null)
        {
            return BadRequest("cart detail is empty");
        }
        await _cartService.updateCartDetailAsync(cartDetail);
        return Ok();
    }


}