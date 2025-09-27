using System;
using System.Collections.Generic;

namespace UnitToolkit.Core.Services;

/// <summary>
/// Converts data sizes between SI units (KB, MB, GB, TB = 10^3 steps)
/// and IEC units (KiB, MiB, GiB, TiB = 2^10 steps). Case-insensitive.
/// </summary>
public class DataSizeConverter
{
    private static readonly Dictionary<string, double> ToBytes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["B"]   = 1d,
        ["KB"]  = 1e3,          ["MB"]  = 1e6,          ["GB"]  = 1e9,          ["TB"]  = 1e12,
        ["KiB"] = 1024d,        ["MiB"] = Math.Pow(1024, 2), ["GiB"] = Math.Pow(1024, 3), ["TiB"] = Math.Pow(1024, 4),
    };

    /// <summary>
    /// Convert a numeric value between data size units.
    /// </summary>
    /// <param name="value">Source numeric value.</param>
    /// <param name="from">Source unit (B, KB, MB, GB, TB, KiB, MiB, GiB, TiB).</param>
    /// <param name="to">Target unit (same set).</param>
    /// <returns>Converted value in target unit.</returns>
    /// <exception cref="ArgumentException">If an unknown unit is provided.</exception>
    public double Convert(double value, string from, string to)
    {
        if (!ToBytes.TryGetValue(from, out var f))
            throw new ArgumentException($"Unknown unit: {from}", nameof(from));
        if (!ToBytes.TryGetValue(to, out var t))
            throw new ArgumentException($"Unknown unit: {to}", nameof(to));

        var bytes = value * f;
        return bytes / t;
    }
}
