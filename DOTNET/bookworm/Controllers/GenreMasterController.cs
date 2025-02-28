using Bookworm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookworm.Controllers;

[ApiController]
[Route("api/genre")]
public class GenreMasterController : ControllerBase{
    private readonly Service.IGenreMasterService _genreMasterService;

    public GenreMasterController(Service.IGenreMasterService genreMasterService){
        _genreMasterService = genreMasterService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreMaster>>> GetAllGenreMasterAsync(){
        var genreMaster = await _genreMasterService.GetAllGenreMasterAsync();
        return Ok(genreMaster); 
    }

    



}