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
}
