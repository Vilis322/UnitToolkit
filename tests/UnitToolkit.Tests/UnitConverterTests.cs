using System;
using UnitToolkit.Core.Services;
using Xunit;

namespace UnitToolkit.Tests;

public class UnitConverterTests
{
    private readonly UnitConverter _u = new();

    [Theory]
    [InlineData(0, "C", "F", 32)]
    [InlineData(100, "C", "K", 373.15)]
    [InlineData(32, "F", "C", 0)]
    public void Temperature_Conversions(double value, string from, string to, double expected)
    {
        var actual = _u.Convert("temperature", value, from, to);
        Assert.True(Math.Abs(actual - expected) < 1e-6, $"Expected {expected}, got {actual}");
    }

    [Theory]
    [InlineData(1.7, "m", "cm", 170)]
    [InlineData(300, "cm", "m", 3)]
    [InlineData(5, "km", "m", 5000)]
    public void Length_Conversions(double value, string from, string to, double expected)
    {
        var actual = _u.Convert("length", value, from, to);
        Assert.True(Math.Abs(actual - expected) < 1e-9, $"Expected {expected}, got {actual}");
    }

    [Theory]
    [InlineData(1, "kg", "g", 1000)]
    [InlineData(2.5, "kg", "lb", 2.5 / 0.45359237)]
    [InlineData(10, "lb", "kg", 10 * 0.45359237)]
    public void Mass_Conversions(double value, string from, string to, double expected)
    {
        var actual = _u.Convert("mass", value, from, to);
        Assert.True(Math.Abs(actual - expected) < 1e-9, $"Expected {expected}, got {actual}");
    }

    // Negative test cases
    [Fact]
    public void Convert_ThrowsForUnknownType()
    {
        Assert.Throws<ArgumentException>(() => _u.Convert("unknown", 100, "m", "cm"));
    }

    [Fact]
    public void Convert_ThrowsForUnknownTemperatureUnit()
    {
        Assert.Throws<ArgumentException>(() => _u.Convert("temperature", 100, "X", "C"));
    }

    [Fact]
    public void Convert_ThrowsForUnknownLengthUnit()
    {
        Assert.Throws<ArgumentException>(() => _u.Convert("length", 100, "m", "mile"));
    }

    [Fact]
    public void Convert_ThrowsForUnknownMassUnit()
    {
        Assert.Throws<ArgumentException>(() => _u.Convert("mass", 100, "kg", "ton"));
    }

    // Test enum-based API
    [Fact]
    public void Convert_WithEnumType_Works()
    {
        var actual = _u.Convert(ConversionType.Temperature, 0, "C", "F");
        Assert.True(Math.Abs(actual - 32) < 1e-6);
    }

    [Fact]
    public void Convert_HasUnitCollections()
    {
        Assert.Contains("C", _u.TemperatureUnits);
        Assert.Contains("m", _u.LengthUnits);
        Assert.Contains("kg", _u.MassUnits);
    }
}
