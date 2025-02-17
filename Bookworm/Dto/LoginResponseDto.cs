
using Bookworm.Models;

namespace Bookworm.Dto;

public class LoginResponseDto
{
    public CustomerMaster CustomerMaster { get; set; }
    public string AccessToken { get; set; } = string.Empty;
}