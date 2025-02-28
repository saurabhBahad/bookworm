using bookworm.Dto;
using Bookworm.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookworm.Service;

public interface ICartService
{
//     Task<ActionResult<IEnumerable<CartDetail>>> GetCartDetails(int id);
        Task<List<CartDetail>> GetDetailsAsync(int customerId);
        Task AddProductAsync(CartHelper cartHelper);
        Task RemoveProductAsync(int customerId, int productId);
        Task updateCartDetailAsync(CartDetail cartDetail);
}