using Claims.Repositories;

namespace Claims.Validation
{
    public class ClaimValidator : IClaimValidator
    {

        public const decimal MaxDamageCostValue = 100_000m;

        private readonly ICoverRepository _coverRepository;

        public ClaimValidator(ICoverRepository coverRepository)
        {
            _coverRepository = coverRepository;
        }

        public async Task<IDictionary<string, string[]>> ValidateAsync(Claim claim)
        {
            var errors = new Dictionary<string, string[]>();

            if (claim.DamageCost > MaxDamageCostValue)
            {
                errors[nameof(Claim.DamageCost)] = [$"DamageCost cannot be higher than {MaxDamageCostValue}."];
            }

            var cover = await _coverRepository.GetByIdAsync(claim.CoverId);
            if (cover is null)
            {
                errors[nameof(Claim.CoverId)] = ["Cover does not exist"];
            }
            else if (claim.Created.Date < cover.StartDate.Date || claim.Created.Date > cover.EndDate.Date)
            {
                errors[nameof(Claim.Created)] = ["Created date must be within the period of the related cover"];
            }

            return errors;
        }
    }
}
