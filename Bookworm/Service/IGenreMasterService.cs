
using Bookworm.Models;

namespace Bookworm.Service
{
    public interface IGenreMasterService
    {
        Task<IEnumerable<GenreMaster>> GetAllGenreMasterAsync();
        
    }
}