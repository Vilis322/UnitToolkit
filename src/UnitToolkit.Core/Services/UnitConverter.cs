using System;
using System.Collections.Generic;

namespace UnitToolkit.Core.Services;

public enum ConversionType
{
    Temperature,
    Length,
    Mass
}

public class UnitConverter
{
    public IReadOnlyList<string> TemperatureUnits { get; } = new[] { "C", "F", "K" };
    public IReadOnlyList<string> LengthUnits { get; } = new[] { "m", "cm", "km" };
    public IReadOnlyList<string> MassUnits { get; } = new[] { "kg", "g", "lb" };

    /// <summary>
    /// Convert a numeric value between units within a given family.
    /// </summary>
    /// <param name="type">Conversion type (Temperature, Length, or Mass).</param>
    /// <param name="value">Source numeric value.</param>
    /// <param name="from">Source unit symbol (e.g., "C", "m", "kg").</param>
    /// <param name="to">Target unit symbol (e.g., "F", "cm", "lb").</param>
    /// <returns>Converted numeric value in the target unit.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the unit symbols are unknown.
    /// </exception>
    public double Convert(ConversionType type, double value, string from, string to)
    {
        from = from.Trim().ToLowerInvariant();
        to   = to.Trim().ToLowerInvariant();

        return type switch
        {
            ConversionType.Temperature => ConvertTemperature(value, from, to),
            ConversionType.Length      => ConvertLength(value, from, to),
            ConversionType.Mass        => ConvertMass(value, from, to),
            _ => throw new ArgumentException("Unknown conversion type.")
        };
    }

    /// <summary>
    /// Convert a numeric value between units within a given family
    /// (temperature/length/mass). String-based wrapper for CLI compatibility.
    /// </summary>
    /// <param name="type">
    /// Conversion family. Accepted aliases: "temperature"|"temp",
    /// "length"|"len", "mass".
    /// </param>
    /// <param name="value">Source numeric value.</param>
    /// <param name="from">Source unit symbol (e.g., "C", "m", "kg").</param>
    /// <param name="to">Target unit symbol (e.g., "F", "cm", "lb").</param>
    /// <returns>Converted numeric value in the target unit.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the family or unit symbols are unknown.
    /// </exception>
    public double Convert(string? type, double value, string from, string to)
    {
        type = type?.Trim().ToLowerInvariant();
        from = from.Trim().ToLowerInvariant();
        to   = to.Trim().ToLowerInvariant();

        return type switch
        {
            "temperature" or "temp" => ConvertTemperature(value, from, to),
            "length" or "len"       => ConvertLength(value, from, to),
            "mass"                  => ConvertMass(value, from, to),
            _ => throw new ArgumentException("Unknown conversion type.")
        };
    }

    private static double ConvertTemperature(double v, string from, string to)
    {
        double c = from switch
        {
            "c" => v,
            "f" => (v - 32) * 5.0 / 9.0,
            "k" => v - 273.15,
            _   => throw new ArgumentException("Unknown temperature unit.")
        };
        return to switch
        {
            "c" => c,
            "f" => c * 9.0 / 5.0 + 32,
            "k" => c + 273.15,
            _   => throw new ArgumentException("Unknown temperature unit.")
        };
    }

    private static double ConvertLength(double v, string from, string to)
    {
        double m = from switch
        {
            "m"  => v,
            "cm" => v / 100.0,
            "km" => v * 1000.0,
            _    => throw new ArgumentException("Unknown length unit.")
        };
        return to switch
        {
            "m"  => m,
            "cm" => m * 100.0,
            "km" => m / 1000.0,
            _    => throw new ArgumentException("Unknown length unit.")
        };
    }

    private static double ConvertMass(double v, string from, string to)
    {
        double kg = from switch
        {
            "kg" => v,
            "g"  => v / 1000.0,
            "lb" => v * 0.45359237,
            _    => throw new ArgumentException("Unknown mass unit.")
        };
        return to switch
        {
            "kg" => Math.Round(kg, 2),
            "g"  => kg * 1000.0,
            "lb" => Math.Round(kg / 0.45359237, 2),
            _    => throw new ArgumentException("Unknown mass unit.")
        };
    }
}
