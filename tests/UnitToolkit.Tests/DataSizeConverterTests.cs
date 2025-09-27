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
}
