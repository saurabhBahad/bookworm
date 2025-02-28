using Bookworm.Models;
using Bookworm.Service;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/languages")]
public class LanguageMasterController : ControllerBase
{
    private readonly ILanguageMasterService _languageMasterService;

    public LanguageMasterController(ILanguageMasterService languageMasterService)
    {
        _languageMasterService = languageMasterService;
        
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LanguageMaster>>> GetAllLanguageMasterAsync()
    {
        var LanguageMaster = await _languageMasterService.GetAllLanguageMasterAsync();
        return Ok(LanguageMaster); 
        
    }
}
