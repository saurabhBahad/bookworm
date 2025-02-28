using Bookworm.Models;

namespace Bookworm.Service
{
    public interface ILanguageMasterService     
    {
        Task<IEnumerable<LanguageMaster>> GetAllLanguageMasterAsync();
    }
}