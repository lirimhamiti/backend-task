namespace Claims.Validation
{
    public class CoverValidator : ICoverValidator
    {
        private readonly TimeProvider _timeProvider;

        public CoverValidator(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public IDictionary<string, string[]> Validate(Cover cover)
        {
            var errors = new Dictionary<string, string[]>();
            var today = _timeProvider.GetUtcNow().UtcDateTime.Date;
            var start = cover.StartDate.Date;
            var end = cover.EndDate.Date;

            if (start < today)
            {
                errors[nameof(Cover.StartDate)] = ["StartDate cannot be in the past"];
            }

            if (end <= start)
            {
                errors[nameof(Cover.EndDate)] = ["EndDate must be after StartDate"];
            }
            else if (end > start.AddYears(1))
            {
                errors[nameof(Cover.EndDate)] = ["Total insurance period cannot exceed 1 year"];
            }

            return errors;
        }
    }
}
