using System;
using System.Globalization;

namespace UnitToolkit.Cli.Common;

public static class Input
{
    public static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var x))
                return x;
            Console.WriteLine("Invalid number. Use dot for decimals.");
        }
    }

    public static int ReadInt(string prompt, int? min = null, int? max = null)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (int.TryParse(s, out var x) && (min is null || x >= min) && (max is null || x <= max))
                return x;
            Console.WriteLine("Invalid integer.");
        }
    }

    public static bool ReadYesNo(string prompt)
    {
        Console.Write(prompt);
        var s = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
        return s is "y" or "yes" or "true" or "t";
    }

    public static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = (Console.ReadLine() ?? "").Trim();
            if (s.Length > 0) return s;
            Console.WriteLine("Value must not be empty.");
        }
    }
}
