using UnitToolkit.Core.Services;
using Xunit;

namespace UnitToolkit.Tests;

public class CurrencyCalculatorTests
{
    [Theory]
    [InlineData(100, 1.1, 110)]
    [InlineData(0, 1.5, 0)]
    [InlineData(-50, 2.0, -100)]
    public void Convert_Works(double amount, double rate, double expected)
    {
        var c = new CurrencyCalculator();
        var actual = c.Convert(amount, rate);
        Assert.Equal(expected, actual, 10);
    }
}
