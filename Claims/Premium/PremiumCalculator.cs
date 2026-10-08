namespace Claims.Premium
{
    public class PremiumCalculator : IPremiumCalculator
    {
        public decimal Calculate(CoverType coverType, DateTime startDate, DateTime endDate)
        {
            var baseDayRate = 1250m;
            var totalPremium = 0m;


            var multiplier = 1.3m;
            if(coverType == CoverType.Yacht)
            {
                multiplier = 1.1m;
            }
            if (coverType == CoverType.PassengerShip)
            {
                multiplier = 1.2m;
            }
            if (coverType == CoverType.Tanker)
            {
                multiplier = 1.5m;
            }

            var premiumRateDay = baseDayRate * multiplier;
            var insuranceDays = (endDate- startDate).TotalDays;

            for (var i = 0; i<insuranceDays; i++)
            {
                if (i < 30) totalPremium += premiumRateDay;

                if (i >= 30 && i < 180)
                {
                    if (coverType == CoverType.Yacht) { totalPremium += premiumRateDay * 0.95m; }
                    else { totalPremium += premiumRateDay * 0.98m; }
                }

                else
                {
                    if (coverType == CoverType.Yacht) { totalPremium += premiumRateDay * 0.97m; }
                    else { totalPremium += premiumRateDay * 0.99m; }
                }
            }


            return totalPremium;
        }
    }
}
