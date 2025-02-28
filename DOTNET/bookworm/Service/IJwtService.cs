
using Bookworm.Models;

namespace Bookworm.Service;

public interface IJwtService
{
    string GenerateAccessToken(CustomerMaster customerMaster);
}