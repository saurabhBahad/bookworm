
using Bookworm.Dto;
using Bookworm.Models;

namespace Bookworm.Service;

public interface ICustomerMasterService
{
    Task<bool> CheckUserExists(CustomerMaster customerMaster);
    Task<CustomerMaster> LoginUser(LoginDto loginDto);
    Task<CustomerMaster> RegisterUser(CustomerMaster customer);
}