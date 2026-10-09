using Claims.Validation;
using Xunit;

namespace Claims.Tests;

    public class CoverValidatorTests
    {
    private static readonly DateTime Today = DateTime.UtcNow.Date;        
    private readonly CoverValidator _validator = new(TimeProvider.System);  

    [Fact]
    public void ValidCover_HasNoErrors()
    {
        var cover = new Cover { StartDate = Today.AddDays(1), EndDate = Today.AddMonths(11) };

        var errors = _validator.Validate(cover);

        Assert.Empty(errors);
    }

    [Fact]
    public void StartDateInPast_IsInvalid()
    {
        var cover = new Cover { StartDate = Today.AddDays(-1), EndDate = Today.AddMonths(1) };

        var errors = _validator.Validate(cover);

        Assert.True(errors.ContainsKey("StartDate"));
    }

    [Fact]
    public void PeriodLongerThanOneYear_IsInvalid()
    {
        var cover = new Cover { StartDate = Today.AddDays(1), EndDate = Today.AddDays(1).AddYears(1).AddDays(1) };

        var errors = _validator.Validate(cover);

        Assert.True(errors.ContainsKey("EndDate"));
    }
}

