using Bookworm.Models;
using Bookworm.Repository;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Service.Impl
{
    public class RoyalityCalculationService : IRoyalityCalculationService
    {
        private readonly AppDbContext _context;

        public RoyalityCalculationService(AppDbContext context)
        {
            _context = context;
        }

        public RoyaltyCalculation calculateRoyality(int cartDetailId)
        {
            var cartDetail = _context.CartDetails
                                     .Include(cd => cd.Product)
                                     .FirstOrDefault(cd => cd.CartDetailsId == cartDetailId);

            if (cartDetail == null)
            {
                throw new System.Exception("Cart detail not found.");
            }

            var product = cartDetail.Product;
            if (product == null)
            {
                throw new System.Exception("Product not found.");
            }

            var productBeneficiary = _context.ProductBeneficiaries
                                             .Include(pb => pb.Beneficiary)
                                             .FirstOrDefault(pb => pb.ProductId == product.ProductId);

            if (productBeneficiary == null)
            {
                throw new System.Exception("Product beneficiary not found.");
            }

            // Determine the effective price
            double effectivePrice = product.ProductBasePrice ?? 0;
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (product.ProductOfferPrice.HasValue && product.ProductOffPriceExpiryDate.HasValue && product.ProductOffPriceExpiryDate.Value > today)
            {
                effectivePrice = product.ProductOfferPrice.Value;
            }

            var salesPrice = cartDetail.IsRented ? (product.RentPerDay ?? 0) * (cartDetail.RentNoOfDays ?? 0) : effectivePrice;
            var royaltyPercentage = productBeneficiary.Percentage ?? 0;
            var royaltyOnBasePrice = effectivePrice * (royaltyPercentage / 100);

            var royaltyCalculation = new RoyaltyCalculation
            {
                BasePrice = effectivePrice,
                RoyaltyOnBasePrice = royaltyOnBasePrice,
                RoyaltyTranDate = DateOnly.FromDateTime(DateTime.Now),
                SalesPrice = salesPrice,
                TransactionType = cartDetail.IsRented ? "Rent" : "Sale",
                BeneficiaryMasterBenId = productBeneficiary.BeneficiaryId,
                ProductProductId = product.ProductId
            };

            _context.RoyaltyCalculations.Add(royaltyCalculation);
            _context.SaveChanges();

            return royaltyCalculation;
        }
    }
}
