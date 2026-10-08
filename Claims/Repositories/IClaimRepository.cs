namespace Claims.Repositories
{
    public interface IClaimRepository
    {
        Task<IEnumerable<Claim>> GetAllClaimsAsync();
        Task AddAsync(Claim claim);
        Task<bool> DeleteAsync(string id);

        Task<Claim?> GetByIdAsync(string id);
    }
}
