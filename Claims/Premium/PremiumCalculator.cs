namespace Claims.Premium
{
    public class PremiumCalculator : IPremiumCalculator
    {
        public decimal Calculate(CoverType coverType, DateTime startDate, DateTime endDate)
        {
            var insuranceDays = (endDate.Date - startDate.Date).Days;
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

            for (var i = 0; i<insuranceDays; i++)
            {
                if (i < 30) totalPremium += premiumRateDay;

                else if (i < 180)
                {
                    totalPremium += coverType == CoverType.Yacht ? premiumRateDay * 0.95m : premiumRateDay * 0.98m;
                }

                else
                {
                    totalPremium += coverType == CoverType.Yacht ? premiumRateDay * 0.92m : premiumRateDay * 0.97m;
                }
            }


            return totalPremium;
        }
    }
}
