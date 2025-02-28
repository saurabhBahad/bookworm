using Bookworm.Models;
using Bookworm.Repository;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Service.Impl
{
    public class InvoiceMasterService : IInvoiceMasterService
    {
        private readonly AppDbContext _context;

        public InvoiceMasterService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceDetail>> GenerateInvoice(int custId)
        {
            var cart = await _context.CartMasters
                                     .Include(c => c.CartDetails)
                                     .ThenInclude(cd => cd.Product)
                                     .FirstOrDefaultAsync(c => c.CustomerId == custId && c.IsActive);

            if (cart == null)
            {
                throw new System.Exception("No active cart found for the customer.");
            }

            var invoice = new Invoice
            {
                CustomerId = custId,
                CartId = cart.CartId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Amount = (decimal?)cart.Cost
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            var invoiceDetails = new List<InvoiceDetail>();

            foreach (var cartDetail in cart.CartDetails)
            {
                var invoiceDetail = new InvoiceDetail
                {
                    InvoiceId = invoice.InvoiceId,
                    ProductId = cartDetail.ProductId,
                    BasePrice = cartDetail.Product.ProductBasePrice,
                    RentNoOfDays = cartDetail.IsRented ? cartDetail.RentNoOfDays : null,
                    SalePrice = cartDetail.IsRented ? null : cartDetail.Product.ProductOfferPrice ?? cartDetail.Product.ProductBasePrice,
                    TranType = cartDetail.IsRented ? "Rent" : "Sale"
                };

                invoiceDetails.Add(invoiceDetail);
            }

            await _context.InvoiceDetails.AddRangeAsync(invoiceDetails);
            await _context.SaveChangesAsync();

            cart.IsActive = false;
            _context.CartMasters.Update(cart);
            await _context.SaveChangesAsync();

            return invoiceDetails;
        }

        public async Task<List<InvoiceDetail>> GetInvoiceDetail(int id)
        {
            var invoiceDetails = await _context.InvoiceDetails
                                               .Where(detail => detail.InvoiceId == id)
                                               .Include(detail => detail.Product)
                                               .ToListAsync();

            if (invoiceDetails == null || !invoiceDetails.Any())
            {
                throw new System.Exception("Invoice details not found.");
            }

            return invoiceDetails;
        }
    }
}
