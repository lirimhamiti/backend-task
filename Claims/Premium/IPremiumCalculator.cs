namespace Claims.Premium;

    public interface IPremiumCalculator
    {
        decimal Calculate(CoverType coverType, DateTime startDate, DateTime endDate);
    }

