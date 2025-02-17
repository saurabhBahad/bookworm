using Bookworm.Models;
using Bookworm.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookworm.Service.Impl
{
    public class MyShelfService : IMyShelfService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MyShelfService> _logger;

        public MyShelfService(AppDbContext context, ILogger<MyShelfService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ShelfDetail>> AddShelfDetailAsync(int custId)
        {
            _logger.LogInformation($"Attempting to find active cart for customer ID: {custId}");

            var cart = await _context.CartMasters
                                    .Include(c => c.CartDetails)
                                    .FirstOrDefaultAsync(c => c.CustomerId == custId && c.IsActive);

            if (cart == null)
            {
                _logger.LogError($"Cart not found for customer ID: {custId}");
                throw new System.Exception("Cart not found.");
            }

            var cartDetails = await _context.CartDetails
                            .Include(cd => cd.Product)
                            .Include(cd => cd.Cart)
                            .Where(cd => cd.CartId == cart.CartId)
                            .ToListAsync();

            if (cartDetails == null || !cartDetails.Any())
            {
                _logger.LogError($"Cart details not found for cart ID: {cart.CartId}");
                throw new System.Exception("Cart details not found.");
            }

            var shelfDetails = new List<ShelfDetail>();

            foreach (var cartDetail in cartDetails)
            {
                RentDetail? rentDetail = null;
                if (cartDetail.IsRented)
                {
                    rentDetail = new RentDetail
                    {
                        ProductId = cartDetail.ProductId,
                        RentStartDate = DateOnly.FromDateTime(DateTime.Now),
                        RentEndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(cartDetail.RentNoOfDays ?? 0)),
                        RentStatus = "Active",
                        CustomerId = cartDetail.Cart?.CustomerId
                    };

                    await _context.RentDetails.AddAsync(rentDetail);
                    await _context.SaveChangesAsync();
                }

                var shelfDetail = new ShelfDetail
                {
                    ShelfId = cartDetail.CartId,
                    ProductId = cartDetail.ProductId,
                    BasePrice = cartDetail.Product?.ProductBasePrice,
                    TranType = cartDetail.IsRented ? "Rent" : "Sale",
                    RentId = rentDetail?.RentId
                };

                await _context.ShelfDetails.AddAsync(shelfDetail);
                await _context.SaveChangesAsync();

                shelfDetails.Add(shelfDetail);
            }

            return shelfDetails;
        }

        public async Task<List<ShelfDetail>> GetShelfDetailAsync(int shelfId)
        {
            var shelfDetails = await _context.ShelfDetails
                             .Include(sd => sd.Product)
                             .Include(sd => sd.Shelf)
                             .Where(sd => sd.ShelfId == shelfId)
                             .ToListAsync();

            if (shelfDetails == null || !shelfDetails.Any())
            {
                throw new System.Exception("Shelf details not found.");
            }

            return shelfDetails;
        }

        public async Task<List<ShelfDetail>> GetShelfDetailListAsync()
        {
            return await _context.ShelfDetails
                                 .Include(sd => sd.Product)
                                 .Include(sd => sd.Shelf)
                                 .ToListAsync();
        }

        public async Task UpdateShelfDetailsAsync(int customerId)
        {
            // Console.WriteLine("inside udpate shelf details");
            var cart = await _context.CartMasters
                                     .Include(c => c.CartDetails)
                                     .ThenInclude(cd => cd.Product)
                                     .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.IsActive);

            if (cart == null)
            {
                throw new System.Exception("Cart not found.");
            }

            var cartDetails = await _context.CartDetails
                                            .Include(cd => cd.Product)
                                            .Where(cd => cd.CartId == cart.CartId)
                                            .ToListAsync();

            foreach (var cartDetail in cartDetails)
            {
                Console.WriteLine(cartDetail.ProductId);

                var shelfDetail = await _context.ShelfDetails
                                                .FirstOrDefaultAsync(sd => sd.ProductId == cartDetail.ProductId);

                if (shelfDetail != null)
                {
                    if (cartDetail.IsRented)
                    {
                        if (shelfDetail.TranType == "Rent")
                        {
                            var rentDetail = await _context.RentDetails
                                                           .FirstOrDefaultAsync(rd => rd.RentId == shelfDetail.RentId);
                            if (rentDetail != null)
                            {
                                rentDetail.RentEndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(cartDetail.RentNoOfDays ?? 0));
                                _context.RentDetails.Update(rentDetail);
                            }
                            else
                            {
                                throw new System.Exception("Rent detail not found.");
                            }
                        }
                        else
                        {
                            shelfDetail.TranType = "Purchased";
                            _context.ShelfDetails.Update(shelfDetail);
                        }
                    }
                }
                else
                {
                    shelfDetail = new ShelfDetail
                    {
                        ProductId = cartDetail.ProductId,
                        TranType = cartDetail.IsRented ? "Rent" : "Purchased",
                    };
                    await _context.ShelfDetails.AddAsync(shelfDetail);
                }
            }

            // Save all changes at once
            await _context.SaveChangesAsync();
        }
    }
}
