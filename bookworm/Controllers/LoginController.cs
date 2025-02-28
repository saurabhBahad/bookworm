
using Bookworm.Dto;
using Bookworm.Models;
using Bookworm.Service;
using Microsoft.AspNetCore.Mvc;

namespace Bookworm.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ICustomerMasterService _customerMasterService;
    private readonly IJwtService _jwtService;

    public LoginController(ICustomerMasterService customerMasterService, IJwtService jwtService)
    {
        _customerMasterService = customerMasterService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<CustomerMaster>> Register([FromBody] CustomerMaster customer)
    {
        Console.WriteLine(customer);
        if (await _customerMasterService.CheckUserExists(customer))
        {
            return BadRequest("User email already exists");
        }
        else {
            CustomerMaster customerMaster = await _customerMasterService.RegisterUser(customer);
            return Ok(customerMaster);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto ){

        CustomerMaster customerMaster=await _customerMasterService.LoginUser(loginDto);

        if (customerMaster == null) {
            return BadRequest("Invalid email or password");
        }

        string accessToken = _jwtService.GenerateAccessToken(customerMaster);

        return Ok(new LoginResponseDto(){CustomerMaster = customerMaster, AccessToken=accessToken});

    }
}