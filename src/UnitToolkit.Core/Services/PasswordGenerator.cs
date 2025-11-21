using System;
using System.Security.Cryptography;
using System.Text;

namespace UnitToolkit.Core.Services;

public class PasswordGenerator
{
    private const int MaxPasswordLength = 256;

    /// <summary>
    /// Generate a random password of the specified length
    /// from the selected character classes.
    /// </summary>
    /// <param name="length">Password length (must be between 4 and 256).</param>
    /// <param name="useUpper">Include uppercase A–Z.</param>
    /// <param name="useDigits">Include digits 0–9.</param>
    /// <param name="useSymbols">Include common symbols.</param>
    /// <returns>Generated password string.</returns>
    /// <exception cref="ArgumentException">If length is not between 4 and 256.</exception>
    /// <exception cref="InvalidOperationException">
    /// If all character classes are disabled and pool becomes empty.
    /// </exception>
    public string Generate(int length, bool useUpper, bool useDigits, bool useSymbols)
    {
        if (length < 4 || length > MaxPasswordLength)
            throw new ArgumentException($"Password length must be between 4 and {MaxPasswordLength}.");

        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string symbols = "!@#$%^&*()-_=+[]{};:,.<>/?";
        
        var pool = new StringBuilder(lower);
        if (useUpper) pool.Append(upper);
        if (useDigits) pool.Append(digits);
        if (useSymbols) pool.Append(symbols);

        string chars = pool.ToString();

        if (chars.Length == 0)
            throw new InvalidOperationException("Character pool is empty.");
        
        var result = new StringBuilder();
        var bytes = RandomNumberGenerator.GetBytes(length);

        foreach (var b in bytes)
        {
            result.Append(chars[b % chars.Length]);
        }

        return result.ToString();
    }
    
    public int RandomInt(int minInclusive, int maxInclusive)
    {
        if (minInclusive > maxInclusive)
            (minInclusive, maxInclusive) = (maxInclusive, minInclusive);

        return RandomNumberGenerator.GetInt32(minInclusive, maxInclusive + 1);
    }
}