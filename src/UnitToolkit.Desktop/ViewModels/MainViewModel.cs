using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UnitToolkit.Core.Services;

namespace UnitToolkit.Desktop.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly UnitConverter _unitConverter;
    private readonly CurrencyCalculator _currencyCalculator;
    private readonly PasswordGenerator _passwordGenerator;
    private readonly BmiCalculator _bmiCalculator;
    private readonly DataSizeConverter _dataSizeConverter;

    public MainViewModel(
        UnitConverter unitConverter,
        CurrencyCalculator currencyCalculator,
        PasswordGenerator passwordGenerator,
        BmiCalculator bmiCalculator,
        DataSizeConverter dataSizeConverter)
    {
        _unitConverter = unitConverter;
        _currencyCalculator = currencyCalculator;
        _passwordGenerator = passwordGenerator;
        _bmiCalculator = bmiCalculator;
        _dataSizeConverter = dataSizeConverter;

        // Initialize unit type collections
        ConversionTypes = new List<string> { "Temperature", "Length", "Mass" };
        SelectedConversionType = ConversionTypes[0];
        UpdateUnitLists();

        // Initialize data size units
        DataSizeUnits = new List<string> { "B", "KB", "MB", "GB", "TB", "KiB", "MiB", "GiB", "TiB" };
        SelectedDataFromUnit = DataSizeUnits[1]; // KB
        SelectedDataToUnit = DataSizeUnits[2];   // MB
    }

    // Units Converter Properties
    [ObservableProperty]
    private List<string> conversionTypes = new();

    [ObservableProperty]
    private string selectedConversionType = string.Empty;

    [ObservableProperty]
    private List<string> availableUnits = new();

    [ObservableProperty]
    private string selectedFromUnit = string.Empty;

    [ObservableProperty]
    private string selectedToUnit = string.Empty;

    [ObservableProperty]
    private string unitValue = string.Empty;

    [ObservableProperty]
    private string unitResult = string.Empty;

    // Currency Properties
    [ObservableProperty]
    private string currencyAmount = string.Empty;

    [ObservableProperty]
    private string currencyRate = string.Empty;

    [ObservableProperty]
    private string currencyResult = string.Empty;

    // Password Properties
    [ObservableProperty]
    private string passwordLength = "12";

    [ObservableProperty]
    private bool useUppercase = true;

    [ObservableProperty]
    private bool useDigits = true;

    [ObservableProperty]
    private bool useSymbols = false;

    [ObservableProperty]
    private string passwordResult = string.Empty;

    // Random Integer Properties
    [ObservableProperty]
    private string randomMin = string.Empty;

    [ObservableProperty]
    private string randomMax = string.Empty;

    [ObservableProperty]
    private string randomResult = string.Empty;

    // BMI Properties
    [ObservableProperty]
    private string bmiHeight = string.Empty;

    [ObservableProperty]
    private string bmiWeight = string.Empty;

    [ObservableProperty]
    private string bmiResult = string.Empty;

    // Data Size Properties
    [ObservableProperty]
    private List<string> dataSizeUnits = new();

    [ObservableProperty]
    private string dataValue = string.Empty;

    [ObservableProperty]
    private string selectedDataFromUnit = string.Empty;

    [ObservableProperty]
    private string selectedDataToUnit = string.Empty;

    [ObservableProperty]
    private string dataResult = string.Empty;

    partial void OnSelectedConversionTypeChanged(string value)
    {
        UpdateUnitLists();
    }

    private void UpdateUnitLists()
    {
        AvailableUnits = SelectedConversionType switch
        {
            "Temperature" => _unitConverter.TemperatureUnits.ToList(),
            "Length" => _unitConverter.LengthUnits.ToList(),
            "Mass" => _unitConverter.MassUnits.ToList(),
            _ => _unitConverter.TemperatureUnits.ToList()
        };

        if (AvailableUnits.Count >= 2)
        {
            SelectedFromUnit = AvailableUnits[0];
            SelectedToUnit = AvailableUnits[1];
        }
    }

    [RelayCommand]
    private void ConvertUnits()
    {
        UnitResult = string.Empty;
        try
        {
            if (!double.TryParse(UnitValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                throw new FormatException("Value must be a number (use dot for decimals).");

            var type = SelectedConversionType.ToLowerInvariant();
            var result = _unitConverter.Convert(type, value, SelectedFromUnit, SelectedToUnit);
            UnitResult = $"Result: {result} {SelectedToUnit}";
        }
        catch (Exception ex)
        {
            UnitResult = $"Error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void ConvertCurrency()
    {
        CurrencyResult = string.Empty;
        try
        {
            if (!double.TryParse(CurrencyAmount, NumberStyles.Float, CultureInfo.InvariantCulture, out var amount))
                throw new FormatException("Amount must be a number.");
            if (!double.TryParse(CurrencyRate, NumberStyles.Float, CultureInfo.InvariantCulture, out var rate))
                throw new FormatException("Rate must be a number.");

            var result = _currencyCalculator.Convert(amount, rate);
            CurrencyResult = $"Converted: {result}";
        }
        catch (Exception ex)
        {
            CurrencyResult = $"Error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void GeneratePassword()
    {
        PasswordResult = string.Empty;
        try
        {
            if (!int.TryParse(PasswordLength, out var length))
                throw new FormatException("Length must be an integer.");

            var password = _passwordGenerator.Generate(length, UseUppercase, UseDigits, UseSymbols);
            PasswordResult = $"Password: {password}";
        }
        catch (Exception ex)
        {
            PasswordResult = $"Error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void GenerateRandomInt()
    {
        RandomResult = string.Empty;
        try
        {
            if (!int.TryParse(RandomMin, out var min))
                throw new FormatException("Min must be an integer.");
            if (!int.TryParse(RandomMax, out var max))
                throw new FormatException("Max must be an integer.");

            var result = _passwordGenerator.RandomInt(min, max);
            RandomResult = $"Random int: {result}";
        }
        catch (Exception ex)
        {
            RandomResult = $"Error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void CalculateBmi()
    {
        BmiResult = string.Empty;
        try
        {
            if (!double.TryParse(BmiHeight, NumberStyles.Float, CultureInfo.InvariantCulture, out var height))
                throw new FormatException("Height must be a number.");
            if (!double.TryParse(BmiWeight, NumberStyles.Float, CultureInfo.InvariantCulture, out var weight))
                throw new FormatException("Weight must be a number.");

            var result = _bmiCalculator.Calculate(height, weight);
            BmiResult = $"BMI: {result.Value:F1} — {result.Category}\nAdvice: {result.Advice}";
        }
        catch (Exception ex)
        {
            BmiResult = $"Error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void ConvertDataSize()
    {
        DataResult = string.Empty;
        try
        {
            if (!double.TryParse(DataValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                throw new FormatException("Value must be a number.");

            var result = _dataSizeConverter.Convert(value, SelectedDataFromUnit, SelectedDataToUnit);
            DataResult = $"Result: {result} {SelectedDataToUnit}";
        }
        catch (Exception ex)
        {
            DataResult = $"Error: {ex.Message}";
        }
    }
}
