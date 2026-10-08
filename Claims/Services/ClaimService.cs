using Claims.Auditing;
using Claims.Repositories;
using Claims.Validation;

namespace Claims.Services;

    public class ClaimService : IClaimService
{
    private readonly IClaimRepository _claimrepository;
    private readonly IAuditer _auditer;
    private readonly IClaimValidator _validator;

    public ClaimService(IClaimRepository claimrepository, IAuditer auditer, IClaimValidator validator)
    {
        _claimrepository = claimrepository;
        _auditer = auditer;
        _validator = validator;
    }

    public Task<IEnumerable<Claim>> GetAllAsync()
    {
        return _claimrepository.GetAllClaimsAsync();
    }

    public Task <Claim?> GetByIdAsync(string id)
    {
        return _claimrepository.GetByIdAsync(id);
    }

    public async Task<Claim> CreateAsync(Claim claim)
    {
        var errors = await _validator.ValidateAsync(claim);
        if (errors.Count > 0) throw new ValidationException(errors);

        claim.Id = Guid.NewGuid().ToString();
        await _claimrepository.AddAsync(claim);
        _auditer.AuditClaim(claim.Id, "POST");
        return claim;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleted = await _claimrepository.DeleteAsync(id);
        if (deleted)
        {
            _auditer.AuditClaim(id, "DELETE");
        }
        return deleted;
    }
}

