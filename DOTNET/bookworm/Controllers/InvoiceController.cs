using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceMasterService _invoiceMasterService;
        private readonly IMyShelfService _myShelfService;
        private readonly IRoyalityCalculationService _royalityCalculationService;
        private readonly AppDbContext _context;

        public InvoiceController(
            IInvoiceMasterService invoiceMasterService,
            IMyShelfService myShelfService,
            IRoyalityCalculationService royalityCalculationService,
            AppDbContext context)
        {
            _invoiceMasterService = invoiceMasterService;
            _myShelfService = myShelfService;
            _royalityCalculationService = royalityCalculationService;
            _context = context;
        }

        [HttpGet("generate-invoice/{customerId}")]
        public async Task<ActionResult<List<InvoiceDetail>>> GenerateInvoice(int customerId)
        {
            if (customerId == 0)
            {
                return BadRequest("Invalid customer id.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var invoiceDetails = await _invoiceMasterService.GenerateInvoice(customerId);

                foreach (var detail in invoiceDetails)
                {
                    var cartDetail = await _context.CartDetails
                                                   .Include(cd => cd.Product)
                                                   .FirstOrDefaultAsync(cd => cd.ProductId == detail.ProductId && cd.Cart != null && cd.Cart.CustomerId == customerId);

                    if (cartDetail != null)
                    {
                        _royalityCalculationService.calculateRoyality(cartDetail.CartDetailsId);
                    }
                }

                await _myShelfService.AddShelfDetailAsync(customerId);

                // Deactivate the old cart
                var oldCart = await _context.CartMasters
                                            .FirstOrDefaultAsync(cm => cm.CustomerId == customerId && cm.IsActive);
                if (oldCart != null)
                {
                    oldCart.IsActive = false;
                    _context.CartMasters.Update(oldCart);
                }

                var newCart = new CartMaster
                {
                    CustomerId = customerId,
                    IsActive = true,
                    Cost = 0
                };
                await _context.CartMasters.AddAsync(newCart);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return Ok(invoiceDetails);
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
