using Bookworm.Models;

namespace Bookworm.Service
{
    public interface IMyShelfService
    {
        Task<List<ShelfDetail>> GetShelfDetailListAsync();
        Task<List<ShelfDetail>> GetShelfDetailAsync(int shelfDetailId);
        Task<List<ShelfDetail>> AddShelfDetailAsync(int cartDetailId);

        Task UpdateShelfDetailsAsync(int cartId);
    }
}
