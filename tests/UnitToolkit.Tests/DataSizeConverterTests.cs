using System;
using UnitToolkit.Core.Services;
using Xunit;

namespace UnitToolkit.Tests;

public class DataSizeConverterTests
{
    private readonly DataSizeConverter _d = new();

    [Theory]
    [InlineData(1536, "B", "KB", 1.536)]
    [InlineData(1, "GiB", "MB", 1073.741824)]
    [InlineData(1, "GB", "MiB", 953.67431640625)]
    public void Convert_Works(double value, string from, string to, double expected)
    {
        var actual = _d.Convert(value, from, to);
        Assert.True(Math.Abs(actual - expected) < 1e-9, $"Expected {expected}, got {actual}");
    }

    [Fact]
    public void Convert_BytesToKilobytes()
    {
        var actual = _d.Convert(1000, "B", "KB");
        Assert.Equal(1.0, actual, 10);
    }

    [Fact]
    public void Convert_KilobytesToBytes()
    {
        var actual = _d.Convert(1, "KB", "B");
        Assert.Equal(1000.0, actual, 10);
    }

    [Fact]
    public void Convert_MegabytesToMebibytes()
    {
        // 1 MB = 1,000,000 bytes
        // 1 MiB = 1,048,576 bytes
        // 1 MB = 0.9537 MiB
        var actual = _d.Convert(1, "MB", "MiB");
        Assert.True(Math.Abs(actual - 0.9537) < 0.01);
    }

    // Negative test cases
    [Fact]
    public void Convert_ThrowsForUnknownFromUnit()
    {
        Assert.Throws<ArgumentException>(() => _d.Convert(100, "XYZ", "KB"));
    }

    [Fact]
    public void Convert_ThrowsForUnknownToUnit()
    {
        Assert.Throws<ArgumentException>(() => _d.Convert(100, "KB", "XYZ"));
    }

    [Fact]
    public void Convert_IsCaseInsensitive()
    {
        var result1 = _d.Convert(1, "kb", "mb");
        var result2 = _d.Convert(1, "KB", "MB");
        Assert.Equal(result1, result2);
    }
}
