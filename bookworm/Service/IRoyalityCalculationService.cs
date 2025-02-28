using Bookworm.Models;

namespace Bookworm.Service
{
    public interface IRoyalityCalculationService
    {
        RoyaltyCalculation calculateRoyality(int cartDetailId);
    }
}
