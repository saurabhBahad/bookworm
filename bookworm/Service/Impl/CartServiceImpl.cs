
using bookworm.Dto;
using Bookworm.Models;
using Bookworm.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System        ;
namespace Bookworm.Service.Impl;

public class CarServiceImpl : ICartService
{
    private readonly AppDbContext _context;

    CartHelper cartHelper = new CartHelper();

    public CarServiceImpl(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<List<CartDetail>> GetDetailsAsync(int customerId)
    {
        var cart = await _context.CartMasters
                                 .Where(c => c.CustomerId == customerId && c.IsActive)
                                 .FirstOrDefaultAsync();
        if (cart == null)
        {
            throw new System.Exception("No active cart found for the customer.");
        }

        return _context.CartDetails
                             .Where(cd => cd.CartId == cart.CartId)
                             .Include(cd => cd.Product)
                             .ToList();
    }

    public async Task AddProductAsync(CartHelper cartHelper)
    {
        var customer = _context.CustomerMasters.AsQueryable().Include(c => c.Shelf)
                                         .Where(c => c.CustomerId == cartHelper.CustId).First();
        if (customer == null)
        {
            throw new System.Exception("Customer not found.");
        }
        var Shelf = customer.Shelf; 
        if (Shelf == null){
            throw new System.Exception("shelf not found");
        }
        var prod = _context.ShelfDetails.Where(s => s.ShelfId == Shelf.ShelfId && s.ProductId == cartHelper.ProdId && s.RentId == null);

        if (prod.Any()) {
            throw new System.Exception("Product already in the shelf.");
        }
        var cart = await _context.CartMasters
                                 .Where(c => c.CustomerId == cartHelper.CustId && c.IsActive)
                                 .FirstOrDefaultAsync();

        if (cart == null)
        {
            cart = new CartMaster
            {
                CustomerId = customer.CustomerId,
                IsActive = true,
                Cost = 0
            };

            await _context.CartMasters.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        var product = await _context.ProductMasters
                                   .FirstOrDefaultAsync(p => p.ProductId == cartHelper.ProdId);
        if (product == null)
        {
            throw new System.Exception("Product not found.");
        }

        var existingProduct = await _context.CartDetails
                                             .FirstOrDefaultAsync(cd => cd.CartId == cart.CartId && cd.ProductId == product.ProductId);
        if (existingProduct != null)
        {
            throw new System.Exception("Product already in the cart.");
        }

        double? productCost;
        if (cartHelper.IsRented )
        {
            if(!product.IsRentable){
                throw new System.Exception("Product is not rentable.");
            }
            if (product.MinRentDays > cartHelper.NoOfDays){
                throw new System.Exception("Minimum rent days not met.");
            }
            productCost = (double)((product.RentPerDay ?? 0) * cartHelper.NoOfDays);
        }
        else
        {
            double basePrice = product.ProductBasePrice ?? 0;

            // Convert DateTime.Now to DateOnly for comparison
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (product.ProductOffPriceExpiryDate.HasValue && product.ProductOffPriceExpiryDate.Value > today)
            {
                basePrice = product.ProductOfferPrice ?? basePrice;
            }

            productCost = basePrice;
        }

        cart.Cost += productCost;

        var cartDetail = new CartDetail
        {
            CartId = cart.CartId,
            ProductId = product.ProductId,
            IsRented = cartHelper.IsRented,
            RentNoOfDays = cartHelper.IsRented ? cartHelper.NoOfDays : 0
        };

        await _context.CartDetails.AddAsync(cartDetail);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveProductAsync(int customerId, int productId)
    {
        var cart = await _context.CartMasters
                                 .Where(c => c.CustomerId == customerId && c.IsActive)
                                 .FirstOrDefaultAsync();

        if (cart == null)
        {
            throw new SystemException("No active cart found for the customer.");
        }

        var cartDetail = await _context.CartDetails
                                       .FirstOrDefaultAsync(cd => cd.CartId == cart.CartId && cd.ProductId == productId);
        if (cartDetail == null)
        {
            throw new SystemException("Product not found in the cart.");
        }

        var product = await _context.ProductMasters
                                   .FirstOrDefaultAsync(p => p.ProductId == productId);
        if (product == null)
        {
            throw new SystemException("Product not found.");
        }

        double? productCost;
        if (cartDetail.IsRented)
        {
            productCost = (double)((product.RentPerDay ?? 0) * cartDetail.RentNoOfDays);
        }
        else
        {
            double basePrice = product.ProductBasePrice ?? 0;

            // Convert DateTime.Now to DateOnly for comparison
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (product.ProductOffPriceExpiryDate.HasValue && product.ProductOffPriceExpiryDate.Value > today)
            {
                basePrice = product.ProductOfferPrice ?? basePrice;
            }

            productCost = basePrice;
        }

        cart.Cost -= productCost;

        _context.CartDetails.Remove(cartDetail);
        await _context.SaveChangesAsync();
    }

    public async Task updateCartDetailAsync(CartDetail cartDetail)
    {
        if (cartDetail == null)
        {
            throw new ArgumentNullException(nameof(cartDetail));
        }

        var existingCartDetail = await _context.CartDetails
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(cd => cd.CartDetailsId == cartDetail.CartDetailsId);

        if (existingCartDetail == null)
        {
            throw new System.Exception("Cart detail not found.");
        }

        var cart = await _context.CartMasters
                                 .FirstOrDefaultAsync(c => c.CartId == cartDetail.CartId);

        if (cart == null)
        {
            throw new System.Exception("Cart not found.");
        }

        var product = await _context.ProductMasters
                                    .FirstOrDefaultAsync(p => p.ProductId == cartDetail.ProductId);

        if (product == null)
        {
            throw new System.Exception("Product not found.");
        }
        cartDetail.Product = product;
        double? oldProductCost;
        if (existingCartDetail.IsRented)
        {
            oldProductCost = (double)((product.RentPerDay ?? 0) * existingCartDetail.RentNoOfDays);
        }
        else
        {
            double basePrice = product.ProductBasePrice ?? 0;

            // Convert DateTime.Now to DateOnly for comparison
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (product.ProductOffPriceExpiryDate.HasValue && product.ProductOffPriceExpiryDate.Value > today)
            {
                basePrice = product.ProductOfferPrice ?? basePrice;
            }

            oldProductCost = basePrice;
        }

        double? newProductCost;
        if (cartDetail.IsRented)
        {
            if (!product.IsRentable)
            {
                throw new System.Exception("Product is not rentable.");
            }
            if (product.MinRentDays > cartDetail.RentNoOfDays)
            {
                throw new System.Exception("Minimum rent days not met.");
            }
            newProductCost = (double)((product.RentPerDay ?? 0) * cartDetail.RentNoOfDays);
        }
        else
        {
            double basePrice = product.ProductBasePrice ?? 0;

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (product.ProductOffPriceExpiryDate.HasValue && product.ProductOffPriceExpiryDate.Value > today)
            {
                basePrice = product.ProductOfferPrice ?? basePrice;
            }

            newProductCost = basePrice;
        }

        cart.Cost = cart.Cost - oldProductCost + newProductCost;
        _context.CartMasters.Update(cart);
        _context.CartDetails.Update(cartDetail);
        await _context.SaveChangesAsync();
    }
}
