namespace Claims.Validation;
    public interface IClaimValidator
    {
        Task<IDictionary<string, string[]>> ValidateAsync(Claim claim);
    }

