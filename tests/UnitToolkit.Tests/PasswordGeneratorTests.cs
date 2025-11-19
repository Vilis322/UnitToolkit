using System.Linq;
using UnitToolkit.Core.Services;
using Xunit;

namespace UnitToolkit.Tests;

public class PasswordGeneratorTests
{
    private const string Lower   = "abcdefghijklmnopqrstuvwxyz";
    private const string Upper   = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits  = "0123456789";
    private const string Symbols = "!@#$%^&*()-_=+[]{};:,.<>/?";

    [Theory]
    [InlineData(12, true,  true,  false)]
    [InlineData(16, false, true,  true )]
    [InlineData(8,  false, false, false)]
    public void Generate_HasCorrectLength_AndAllowedChars(int len, bool useUpper, bool useDigits, bool useSymbols)
    {
        var gen = new PasswordGenerator();
        var pwd = gen.Generate(len, useUpper, useDigits, useSymbols);

        Assert.Equal(len, pwd.Length);

        var allowed = Lower
                    + (useUpper ? Upper : "")
                    + (useDigits ? Digits : "")
                    + (useSymbols ? Symbols : "");
        
        if (allowed.Length == 0) allowed = Lower;

        Assert.All(pwd, ch => Assert.Contains(ch, allowed));
    }

    [Fact]
    public void RandomInt_InRange()
    {
        var gen = new PasswordGenerator();
        int min = -5, max = 5;
        for (int i = 0; i < 1000; i++)
        {
            int x = gen.RandomInt(min, max);
            Assert.InRange(x, min, max);
        }
    }

    // Negative test cases
    [Fact]
    public void Generate_ThrowsForTooShortPassword()
    {
        var gen = new PasswordGenerator();
        Assert.Throws<ArgumentException>(() => gen.Generate(3, true, true, false));
    }

    [Fact]
    public void Generate_AcceptsMinimumLength()
    {
        var gen = new PasswordGenerator();
        var pwd = gen.Generate(4, true, true, false);
        Assert.Equal(4, pwd.Length);
    }

    [Fact]
    public void Generate_AcceptsMaximumLength()
    {
        var gen = new PasswordGenerator();
        var pwd = gen.Generate(256, true, true, false);
        Assert.Equal(256, pwd.Length);
    }

    [Fact]
    public void Generate_ThrowsForTooLongPassword()
    {
        var gen = new PasswordGenerator();
        Assert.Throws<ArgumentException>(() => gen.Generate(257, true, true, false));
    }

    [Fact]
    public void Generate_WorksWithOnlyLowercase()
    {
        var gen = new PasswordGenerator();
        var pwd = gen.Generate(10, false, false, false);
        // Should work since lowercase is always included by default
        Assert.Equal(10, pwd.Length);
        Assert.All(pwd, ch => Assert.Contains(ch, Lower));
    }

    [Theory]
    [InlineData(10, true, false, false)]  // Upper only
    [InlineData(10, false, true, false)]  // Digits only
    [InlineData(10, false, false, true)]  // Symbols only
    [InlineData(10, true, true, true)]    // All enabled
    public void Generate_WorksWithDifferentFlagCombinations(int len, bool upper, bool digits, bool symbols)
    {
        var gen = new PasswordGenerator();
        var pwd = gen.Generate(len, upper, digits, symbols);
        Assert.Equal(len, pwd.Length);
    }
}
