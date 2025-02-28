
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bookworm.Models;
using Microsoft.IdentityModel.Tokens;

namespace Bookworm.Service.Impl;

public class JwtServiceImpl : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtServiceImpl(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(CustomerMaster customerMaster)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, customerMaster.CustomerId.ToString()),
            new Claim(ClaimTypes.Name, customerMaster.Email),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}