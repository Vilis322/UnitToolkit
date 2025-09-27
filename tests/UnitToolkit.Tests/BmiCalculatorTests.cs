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
        var (val, cat, _) = bmi.Calculate(170, 65); // 170 cm, 65 kg
        Assert.True(Math.Abs(val - 22.491) < 1e-3, $"Unexpected BMI: {val}");
        Assert.Equal("Normal", cat);
    }

    [Fact]
    public void Calculate_Overweight_FromMeters()
    {
        var bmi = new BmiCalculator();
        var (val, cat, _) = bmi.Calculate(1.75, 90); // 1.75 m, 90 kg
        Assert.True(val >= 25 && val < 30, $"Unexpected BMI: {val}");
        Assert.Equal("Overweight", cat);
    }
}
