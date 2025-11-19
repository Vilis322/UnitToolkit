using System;
using UnitToolkit.Core.Services;
using Xunit;

namespace UnitToolkit.Tests;

public class BmiCalculatorTests
{
    [Fact]
    public void Calculate_Normal_FromCm()
    {
        var bmi = new BmiCalculator();
        var result = bmi.Calculate(170, 65); // 170 cm, 65 kg
        Assert.True(Math.Abs(result.Value - 22.491) < 1e-3, $"Unexpected BMI: {result.Value}");
        Assert.Equal("Normal", result.Category);
    }

    [Fact]
    public void Calculate_Overweight_FromMeters()
    {
        var bmi = new BmiCalculator();
        var result = bmi.Calculate(1.75, 90); // 1.75 m, 90 kg
        Assert.True(result.Value >= 25 && result.Value < 30, $"Unexpected BMI: {result.Value}");
        Assert.Equal("Overweight", result.Category);
    }
}
