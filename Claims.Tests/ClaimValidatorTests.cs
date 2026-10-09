using Claims.Repositories;
using Claims.Validation;
using Moq;
using Xunit;

namespace Claims.Tests;

    public class ClaimValidatorTests
    {

    private readonly ClaimValidator _validator;

    public ClaimValidatorTests()
    {
        var cover = new Cover { Id = "test-cover", StartDate = new(2030, 1, 1), EndDate = new(2030, 6, 30) };
        var coverRepository = new Mock<ICoverRepository>();
        coverRepository.Setup(r => r.GetByIdAsync("test-cover")).ReturnsAsync(cover);

        _validator = new ClaimValidator(coverRepository.Object);
    }

    [Fact]
    public async Task ValidClaim_HasNoErrors()
    {
        var claim = new Claim { CoverId = "test-cover", Created = new(2030, 3, 1), DamageCost = 5000, Name = "Fire" };

        var errors = await _validator.ValidateAsync(claim);

        Assert.Empty(errors);
    }

    [Fact]
    public async Task DamageMoreThan100000_IsInvalid()
    {
        var claim = new Claim { CoverId = "test-cover", Created = new(2030, 3, 1), DamageCost = 150_000, Name = "Crash" };

        var errors = await _validator.ValidateAsync(claim);

        Assert.True(errors.ContainsKey("DamageCost"));
    }

    [Fact]
    public async Task DateOutsideCoverPeriod_IsInvalid()
    {
        var claim = new Claim { CoverId = "test-cover", Created = new(2030, 8, 1), DamageCost = 5000, Name = "Fire" };

        var errors = await _validator.ValidateAsync(claim);

        Assert.True(errors.ContainsKey("Created"));
    }
}

