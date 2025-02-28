using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Service;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Service.Impl;

public class LanguageMasterServiceImpl : ILanguageMasterService
{
    private readonly AppDbContext _appDbContext;

    public LanguageMasterServiceImpl(AppDbContext context){
        _appDbContext = context;
    }

    public async Task<IEnumerable<LanguageMaster>> GetAllLanguageMasterAsync()
    {
        return await _appDbContext.LanguageMasters.ToListAsync();
    }
}

