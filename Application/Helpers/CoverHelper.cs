using Shared.Classes;

namespace Application.Helpers;

public static class CoverHelper
{
    private const int premiumBase = 1250;
    public static decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var multiplier = GetMultiplier(coverType);

        var premiumPerDay = premiumBase * multiplier;
        var insuranceLength = (endDate - startDate).TotalDays;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            var basePremium = premiumPerDay;

            if (i < 30)
            {
                totalPremium += basePremium;
                continue;
            }
            else if (i < 180 && i >= 30)
            {
                var discount = coverType == CoverType.Yacht ? basePremium * 0.05m : basePremium * 0.02m;
                totalPremium += basePremium - discount;
                continue;
            }
            else
            {
                var firstDiscount = coverType == CoverType.Yacht ? basePremium * 0.05m : basePremium * 0.02m;
                var secondDiscount = coverType == CoverType.Yacht ? (basePremium - firstDiscount) * 0.03m : (basePremium - firstDiscount) * 0.01m;

                totalPremium += basePremium - firstDiscount - secondDiscount;
            }
        }

        return totalPremium;
    }

    private static decimal GetMultiplier(CoverType coverType)
    {
        switch (coverType)
        {
            case CoverType.Yacht:
              return 1.1m;
            case CoverType.PassengerShip:
               return 1.2m;
            case CoverType.Tanker:
               return 1.5m;
            default:
                return 1.3m;
        }
    }
}
