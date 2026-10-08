using Claims.Auditing;
using Microsoft.AspNetCore.Mvc;
using Claims.Data;
using MongoDB.Driver.Linq;

namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ILogger<ClaimsController> _logger;
        private readonly ClaimsContext _claimsContext;
        private readonly Auditer _auditer;

        public ClaimsController(ILogger<ClaimsController> logger, ClaimsContext claimsContext, AuditContext auditContext)
        {
            _logger = logger;
            _claimsContext = claimsContext;
            _auditer = new Auditer(auditContext);
        }

        [HttpGet]
        public async Task<IEnumerable<Claim>> GetAsync()
        {
            return await _claimsContext.Claims.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Claim claim)
        {
            claim.Id = Guid.NewGuid().ToString();
            _claimsContext.Claims.Add(claim);
            await _claimsContext.SaveChangesAsync();
            _auditer.AuditClaim(claim.Id, "POST");
            return Ok(claim);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            _auditer.AuditClaim(id, "DELETE");
            var claim = await _claimsContext.Claims.SingleOrDefaultAsync(cl => cl.Id == id);
            if (claim != null)
            {
                _claimsContext.Claims.Remove(claim);
                await _claimsContext.SaveChangesAsync();
            }
        }

        [HttpGet("{id}")]
        public async Task<Claim> GetAsync(string id)
        {
            var claim = await _claimsContext.Claims.SingleOrDefaultAsync(cl => cl.Id == id);
            return claim;
        }
    }

   
   
}
