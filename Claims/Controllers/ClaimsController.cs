using Claims.Auditing;
using Microsoft.AspNetCore.Mvc;
using Claims.Data;
using MongoDB.Driver.Linq;
using Claims.Services;
using Claims.Contracts;

namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
     
        private readonly IClaimService _claimService;

        public ClaimsController(IClaimService claimService)
        {
        _claimService = claimService;
        }

        [HttpGet]
        public async Task<ActionResult<Claim>> GetAsync()
        {
            return Ok(await _claimService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Claim>> GetAsync(string id)
        {
            var claim = await _claimService.GetByIdAsync(id);
            return claim != null ? Ok(claim) : NotFound();
        }


        [HttpPost]
        public async Task<ActionResult<Claim>> CreateAsync(CreateClaimRequest request)
        {
            var created = await _claimService.CreateAsync(request.ToClaim());
            return Created($"/Claims/{created.Id}", created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var deleted = await _claimService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }


    }

   
   
}
