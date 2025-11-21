using System;
using UnitToolkit.Cli.Common;
using UnitToolkit.Core.Services;

var unit = new UnitConverter();
var cur  = new CurrencyCalculator();
var pwd  = new PasswordGenerator();
var bmi  = new BmiCalculator();
var data = new DataSizeConverter();

while (true)
{
    Console.WriteLine("\n=== UnitToolkit CLI ===");
    Console.WriteLine("1) Units converter (temperature/length/mass)");
    Console.WriteLine("2) Currency calculator (amount * rate)");
    Console.WriteLine("3) Password / Random integer");
    Console.WriteLine("4) BMI calculator");
    Console.WriteLine("5) Data size converter (B, KB, MB, GB, TB, KiB, MiB, GiB, TiB)");
    Console.WriteLine("0) Exit");
    Console.Write("Select: ");
    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                Units();
                break;
            case "2":
                Currency();
                break;
            case "3":
                PasswordOrRandom();
                break;
            case "4":
                Bmi();
                break;
            case "5":
                DataSize();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Unknown option.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

void Units()
{
    Console.WriteLine("\n-- Units --");
    Console.WriteLine("Type: temperature | length | mass");
    var type = Input.ReadNonEmpty("Type: ").ToLowerInvariant();
    var value = Input.ReadDouble("Value: ");
    var from = Input.ReadNonEmpty("From unit (C/F/K | m/cm/km | kg/g/lb): ");
    var to   = Input.ReadNonEmpty("To unit: ");
    var result = unit.Convert(type, value, from, to);
    Console.WriteLine($"Result: {result} {to}");
}

void Currency()
{
    Console.WriteLine("\n-- Currency --");
    var amount = Input.ReadDouble("Amount: ");
    var rate   = Input.ReadDouble("Rate (FROM→TO): ");
    var result = cur.Convert(amount, rate);
    Console.WriteLine($"Converted: {result}");
}

void PasswordOrRandom()
{
    Console.WriteLine("\n-- Password / Random --");
    Console.WriteLine("1) Password  2) Random integer");
    Console.Write("Select: ");
    var sub = Console.ReadLine();

    if (sub == "1")
    {
        int len       = Input.ReadInt("Length (4..128): ", 4, 128);
        bool useUpper = Input.ReadYesNo("Use uppercase? (y/n): ");
        bool useDigits = Input.ReadYesNo("Use digits? (y/n): ");
        bool useSymbols = Input.ReadYesNo("Use symbols? (y/n): ");
        var pass = pwd.Generate(len, useUpper, useDigits, useSymbols);
        Console.WriteLine($"Password: {pass}");
    }
    else
    {
        int min = Input.ReadInt("Min: ");
        int max = Input.ReadInt("Max: ");
        var n = pwd.RandomInt(min, max);
        Console.WriteLine($"Random int: {n}");
    }
}

void Bmi()
{
    Console.WriteLine("\n-- BMI --");
    var h = Input.ReadDouble("Height (m or cm): ");
    var w = Input.ReadDouble("Weight (kg): ");
    var result = bmi.Calculate(h, w);
    Console.WriteLine($"BMI: {result.Value:F1} — {result.Category}");
    Console.WriteLine($"Advice: {result.Advice}");
}

void DataSize()
{
    Console.WriteLine("\n-- Data Size --");
    var value = Input.ReadDouble("Value: ");
    Console.WriteLine("Units: B, KB, MB, GB, TB, KiB, MiB, GiB, TiB");
    var from = Input.ReadNonEmpty("From: ");
    var to   = Input.ReadNonEmpty("To: ");
    var result = data.Convert(value, from, to);
    Console.WriteLine($"Result: {result} {to}");
}
