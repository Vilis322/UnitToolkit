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

    [Fact]
    public void Calculate_Underweight()
    {
        var bmi = new BmiCalculator();
        var result = bmi.Calculate(1.75, 50); // BMI ~16.3
        Assert.True(result.Value < 18.5);
        Assert.Equal("Underweight", result.Category);
        Assert.NotEmpty(result.Advice);
    }

    [Fact]
    public void Calculate_Obese()
    {
        var bmi = new BmiCalculator();
        var result = bmi.Calculate(1.70, 95); // BMI ~32.9
        Assert.True(result.Value >= 30);
        Assert.Equal("Obese", result.Category);
        Assert.NotEmpty(result.Advice);
    }

    // Negative test cases
    [Fact]
    public void Calculate_ThrowsForZeroHeight()
    {
        var bmi = new BmiCalculator();
        Assert.Throws<ArgumentException>(() => bmi.Calculate(0, 70));
    }

    [Fact]
    public void Calculate_ThrowsForNegativeHeight()
    {
        var bmi = new BmiCalculator();
        Assert.Throws<ArgumentException>(() => bmi.Calculate(-1.70, 70));
    }

    [Fact]
    public void Calculate_ThrowsForZeroWeight()
    {
        var bmi = new BmiCalculator();
        Assert.Throws<ArgumentException>(() => bmi.Calculate(1.70, 0));
    }

    [Fact]
    public void Calculate_ThrowsForNegativeWeight()
    {
        var bmi = new BmiCalculator();
        Assert.Throws<ArgumentException>(() => bmi.Calculate(1.70, -70));
    }

    [Fact]
    public void Calculate_AutoConvertsCentimetersToMeters()
    {
        var bmi = new BmiCalculator();
        var resultCm = bmi.Calculate(170, 65);
        var resultM = bmi.Calculate(1.70, 65);

        Assert.True(Math.Abs(resultCm.Value - resultM.Value) < 0.01);
    }
}
