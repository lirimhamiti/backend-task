using Claims.Auditing;
using Microsoft.AspNetCore.Mvc;
using Claims.Data;
using Claims.Services;
using Claims.Contracts;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
 private readonly ICoverService _coverService;

    public CoversController(ICoverService cover)
    {
        _coverService = cover;
    }

    [HttpPost("compute")]
    public async Task<ActionResult> ComputePremiumAsync(CoverType coverType, DateTime startDate, DateTime endDate)
    {
        return Ok(_coverService.ComputePremium(startDate, endDate, coverType));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
    {
        return Ok(await _coverService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id)
    {
        var cover = await _coverService.GetByIdAsync(id);
        return cover is null ? NotFound() : Ok(cover);
    }

    [HttpPost]
    public async Task<ActionResult<Cover>> CreateAsync(CreateCoverRequest request)
    {
        var created = await _coverService.CreateAsync(request.ToCover());
        return Created($"/Covers/{created.Id}", created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var deleted = await _coverService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

   
   
}
