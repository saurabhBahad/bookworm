
using Bookworm.Dto;
using Bookworm.Models;
using Bookworm.Repository;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Service.Impl;

public class CustomerMasterServiceImpl : ICustomerMasterService
{
    private readonly AppDbContext _appDbContext;

    public CustomerMasterServiceImpl(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<bool> CheckUserExists(CustomerMaster c)
    {
        return await _appDbContext.CustomerMasters.AnyAsync(cust => cust.Email == c.Email);
    }

    public async Task<CustomerMaster> LoginUser(LoginDto loginDto)
    {
        CustomerMaster? customerMaster = await _appDbContext.CustomerMasters.FirstOrDefaultAsync(cust => cust.Email == loginDto.Email);
        if (customerMaster == null)
        {
            return null;
        }
        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, customerMaster.Password)) return null;
        return customerMaster;
    }

    public async Task<CustomerMaster> RegisterUser(CustomerMaster customer)
    {
        if (customer.Dob == null)
        {
            return null; // Handle null DOB scenario if needed
        }

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        int age = today.Year - customer.Dob.Value.Year;

        if (customer.Dob > today.AddYears(-age))
            age--;

        if (age < 18)
        {
            return null;
        }

        MyShelf myShelf = new MyShelf();
        myShelf.NoOfBooks = 0;

        customer.Age = age;

        var savedMyShelf = await _appDbContext.MyShelves.AddAsync(myShelf);
        await _appDbContext.SaveChangesAsync();

        customer.Shelf = savedMyShelf.Entity;

        customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);

        var savedCustomer = await _appDbContext.CustomerMasters.AddAsync(customer);
        await _appDbContext.SaveChangesAsync();

        return savedCustomer.Entity;
    }
}