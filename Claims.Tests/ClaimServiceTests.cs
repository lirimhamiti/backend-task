using Claims.Auditing;
using Claims.Repositories;
using Claims.Services;
using Claims.Validation;
using Moq;
using Xunit;

namespace Claims.Tests;

    public class ClaimServiceTests
    {
    private readonly Mock<IClaimRepository> _repository = new();
    private readonly Mock<IAuditer> _auditer = new();
    private readonly Mock<IClaimValidator> _validator = new();
    private readonly ClaimService _service;

    public ClaimServiceTests()
    {
        _service = new ClaimService(_repository.Object, _auditer.Object, _validator.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidClaim_IsSavedAndAudited()
    {

        _validator.Setup(v => v.ValidateAsync(It.IsAny<Claim>()))
                  .ReturnsAsync(new Dictionary<string, string[]>());
        var claim = new Claim { CoverId = "cover-1", Name = "Fire" };

        var created = await _service.CreateAsync(claim);

        _repository.Verify(r => r.AddAsync(claim), Times.Once);              
        _auditer.Verify(a => a.AuditClaim(created.Id, "POST"), Times.Once);  
    }

    [Fact]
    public async Task CreateAsync_InvalidClaim_IsNotSaved()
    {
        _validator.Setup(v => v.ValidateAsync(It.IsAny<Claim>()))
                  .ReturnsAsync(new Dictionary<string, string[]> { ["DamageCost"] = ["Too high"] });
        var claim = new Claim { CoverId = "cover-1", Name = "Fire" };

        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync(claim));

        _repository.Verify(r => r.AddAsync(It.IsAny<Claim>()), Times.Never);  
    }
}

