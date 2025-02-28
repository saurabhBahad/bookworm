using Bookworm.Models;
using Bookworm.Repository;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Service.Impl;

public class GenreMasterServiceImpl : IGenreMasterService
{
    private readonly AppDbContext _appDbContext;

    public GenreMasterServiceImpl(AppDbContext context){
        _appDbContext = context;
    }

    public async Task<IEnumerable<GenreMaster>> GetAllGenreMasterAsync()
    {
        return await _appDbContext.GenreMasters.ToListAsync();
    }
}