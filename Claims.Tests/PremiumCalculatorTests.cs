using Claims.Premium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Claims.Tests;

    public class PremiumCalculatorTests
    {

    private readonly PremiumCalculator _calculator = new();
    private static readonly DateTime Start = new(2030, 1, 1);

    [Theory]
    [InlineData(CoverType.Yacht, 10, 13750)]          
    [InlineData(CoverType.PassengerShip, 10, 15000)]  
    [InlineData(CoverType.Tanker, 10, 18750)]         
    [InlineData(CoverType.ContainerShip, 10, 16250)]  
    [InlineData(CoverType.BulkCarrier, 10, 16250)]    
    public void First30Days_AreFullPrice_WithTypeMultiplier(CoverType type, int days, double expected)
    {
        var premium = _calculator.Calculate(type, Start, Start.AddDays(days));

        Assert.Equal((decimal)expected, premium);
    }

    [Theory]
    [InlineData(CoverType.Yacht, 180, 237187.5)]   
    [InlineData(CoverType.Tanker, 180, 331875)]    
    public void Days31To180_AreDiscounted(CoverType type, int days, double expected)
    {
        var premium = _calculator.Calculate(type, Start, Start.AddDays(days));

        Assert.Equal((decimal)expected, premium);
    }

    [Theory]
    [InlineData(CoverType.Yacht, 365, 471212.5)]   
    [InlineData(CoverType.Tanker, 365, 668343.75)] 
    public void DaysAfter180_HaveAdditionalDiscount(CoverType type, int days, double expected)
    {
        var premium = _calculator.Calculate(type, Start, Start.AddDays(days));

        Assert.Equal((decimal)expected, premium);
    }

    [Fact]
    public void EndDateBeforeOrEqualToStartDate_ReturnsZero()
    {
        Assert.Equal(0m, _calculator.Calculate(CoverType.Yacht, Start, Start));
        Assert.Equal(0m, _calculator.Calculate(CoverType.Yacht, Start, Start.AddDays(-5)));
    }

    [Fact]
    public void TimeOfDay_IsIgnored()
    {
        var premium = _calculator.Calculate(CoverType.Yacht, Start.AddHours(23), Start.AddDays(10).AddHours(1));

        Assert.Equal(13750m, premium);
    }

}

