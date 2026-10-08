using Claims.Data;
using Microsoft.EntityFrameworkCore;

namespace Claims.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly ClaimsContext _context;

        public ClaimRepository(ClaimsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Claim>> GetAllClaimsAsync()
        {
            return await _context.Claims.ToListAsync();
        }

        public async Task AddAsync(Claim claim)
        {
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
        }

        public async Task<Claim?> GetByIdAsync(string id)
        {
            return await _context.Claims.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var claim = await GetByIdAsync(id);
            if(claim != null)
            {
                _context.Claims.Remove(claim);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
