using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UnitToolkit.Core.Services;

namespace UnitToolkit.Desktop.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly UnitConverter _unitConverter;
    private readonly CurrencyExchangeService _currencyExchangeService;
    private readonly PasswordGenerator _passwordGenerator;
    private readonly BmiCalculator _bmiCalculator;
    private readonly DataSizeConverter _dataSizeConverter;

    public MainViewModel(
        UnitConverter unitConverter,
        CurrencyExchangeService currencyExchangeService,
        PasswordGenerator passwordGenerator,
        BmiCalculator bmiCalculator,
        DataSizeConverter dataSizeConverter)
    {
        _unitConverter = unitConverter;
        _currencyExchangeService = currencyExchangeService;
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

        // Initialize currency data
        LoadCurrenciesAsync();
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

    [ObservableProperty]
    private string unitErrorMessage = string.Empty;

    // Currency Properties
    [ObservableProperty]
    private List<string> availableCurrencies = new();

    [ObservableProperty]
    private string selectedFromCurrency = "USD";

    [ObservableProperty]
    private string selectedToCurrency = "EUR";

    [ObservableProperty]
    private string currencyAmount = string.Empty;

    [ObservableProperty]
    private string currencyResult = string.Empty;

    [ObservableProperty]
    private string currencyErrorMessage = string.Empty;

    [ObservableProperty]
    private bool isLoadingCurrencies = false;

    [ObservableProperty]
    private ObservableCollection<ExchangeRate> commonExchangeRates = new();

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

    [ObservableProperty]
    private string passwordErrorMessage = string.Empty;

    // Random Integer Properties
    [ObservableProperty]
    private string randomMin = string.Empty;

    [ObservableProperty]
    private string randomMax = string.Empty;

    [ObservableProperty]
    private string randomResult = string.Empty;

    [ObservableProperty]
    private string randomErrorMessage = string.Empty;

    // BMI Properties
    [ObservableProperty]
    private string bmiHeight = string.Empty;

    [ObservableProperty]
    private string bmiWeight = string.Empty;

    [ObservableProperty]
    private string bmiResult = string.Empty;

    [ObservableProperty]
    private string bmiErrorMessage = string.Empty;

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

    [ObservableProperty]
    private string dataErrorMessage = string.Empty;

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
        UnitErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(UnitValue))
        {
            UnitErrorMessage = "Please enter a value to convert.";
            return;
        }

        if (!double.TryParse(UnitValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
        {
            UnitErrorMessage = "Value must be a valid number. Use dot (.) for decimals.";
            return;
        }

        try
        {
            var type = SelectedConversionType.ToLowerInvariant();
            var result = _unitConverter.Convert(type, value, SelectedFromUnit, SelectedToUnit);
            UnitResult = $"Result: {result} {SelectedToUnit}";
        }
        catch (Exception ex)
        {
            UnitErrorMessage = ex.Message;
        }
    }

    private async void LoadCurrenciesAsync()
    {
        try
        {
            IsLoadingCurrencies = true;
            var currencies = await _currencyExchangeService.GetAvailableCurrenciesAsync();
            AvailableCurrencies = currencies;

            // Set default selections if not already set
            if (!string.IsNullOrEmpty(SelectedFromCurrency) && currencies.Contains(SelectedFromCurrency))
            {
                // Keep current selection
            }
            else
            {
                SelectedFromCurrency = currencies.Contains("USD") ? "USD" : currencies.FirstOrDefault() ?? "";
            }

            if (!string.IsNullOrEmpty(SelectedToCurrency) && currencies.Contains(SelectedToCurrency))
            {
                // Keep current selection
            }
            else
            {
                SelectedToCurrency = currencies.Contains("EUR") ? "EUR" : currencies.Skip(1).FirstOrDefault() ?? "";
            }

            // Load common rates
            await LoadCommonRatesAsync();
        }
        catch (Exception ex)
        {
            CurrencyErrorMessage = $"Failed to load currencies: {ex.Message}";
        }
        finally
        {
            IsLoadingCurrencies = false;
        }
    }

    private async Task LoadCommonRatesAsync()
    {
        try
        {
            var rates = await _currencyExchangeService.GetCommonRatesAsync(SelectedFromCurrency);
            CommonExchangeRates.Clear();
            foreach (var rate in rates)
            {
                CommonExchangeRates.Add(rate);
            }
        }
        catch
        {
            // Silently fail - rates table is optional
        }
    }

    [RelayCommand]
    private async Task ConvertCurrency()
    {
        CurrencyResult = string.Empty;
        CurrencyErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(CurrencyAmount))
        {
            CurrencyErrorMessage = "Please enter an amount.";
            return;
        }

        if (!double.TryParse(CurrencyAmount, NumberStyles.Float, CultureInfo.InvariantCulture, out var amount))
        {
            CurrencyErrorMessage = "Amount must be a valid number.";
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedFromCurrency) || string.IsNullOrWhiteSpace(SelectedToCurrency))
        {
            CurrencyErrorMessage = "Please select currencies.";
            return;
        }

        try
        {
            var result = await _currencyExchangeService.ConvertAsync(amount, SelectedFromCurrency, SelectedToCurrency);
            CurrencyResult = $"{amount:N2} {SelectedFromCurrency} = {result:N2} {SelectedToCurrency}";

            // Refresh common rates table
            await LoadCommonRatesAsync();
        }
        catch (Exception ex)
        {
            CurrencyErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void GeneratePassword()
    {
        PasswordResult = string.Empty;
        PasswordErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(PasswordLength))
        {
            PasswordErrorMessage = "Please enter a password length.";
            return;
        }

        if (!int.TryParse(PasswordLength, out var length))
        {
            PasswordErrorMessage = "Length must be a valid integer.";
            return;
        }

        try
        {
            var password = _passwordGenerator.Generate(length, UseUppercase, UseDigits, UseSymbols);
            PasswordResult = $"Password: {password}";
        }
        catch (Exception ex)
        {
            PasswordErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void GenerateRandomInt()
    {
        RandomResult = string.Empty;
        RandomErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(RandomMin))
        {
            RandomErrorMessage = "Please enter a minimum value.";
            return;
        }

        if (string.IsNullOrWhiteSpace(RandomMax))
        {
            RandomErrorMessage = "Please enter a maximum value.";
            return;
        }

        if (!int.TryParse(RandomMin, out var min))
        {
            RandomErrorMessage = "Minimum must be a valid integer.";
            return;
        }

        if (!int.TryParse(RandomMax, out var max))
        {
            RandomErrorMessage = "Maximum must be a valid integer.";
            return;
        }

        try
        {
            var result = _passwordGenerator.RandomInt(min, max);
            RandomResult = $"Random int: {result}";
        }
        catch (Exception ex)
        {
            RandomErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void CalculateBmi()
    {
        BmiResult = string.Empty;
        BmiErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(BmiHeight))
        {
            BmiErrorMessage = "Please enter your height.";
            return;
        }

        if (string.IsNullOrWhiteSpace(BmiWeight))
        {
            BmiErrorMessage = "Please enter your weight.";
            return;
        }

        if (!double.TryParse(BmiHeight, NumberStyles.Float, CultureInfo.InvariantCulture, out var height))
        {
            BmiErrorMessage = "Height must be a valid number.";
            return;
        }

        if (!double.TryParse(BmiWeight, NumberStyles.Float, CultureInfo.InvariantCulture, out var weight))
        {
            BmiErrorMessage = "Weight must be a valid number.";
            return;
        }

        try
        {
            var result = _bmiCalculator.Calculate(height, weight);
            BmiResult = $"BMI: {result.Value:F1} — {result.Category}\nAdvice: {result.Advice}";
        }
        catch (Exception ex)
        {
            BmiErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void ConvertDataSize()
    {
        DataResult = string.Empty;
        DataErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(DataValue))
        {
            DataErrorMessage = "Please enter a value to convert.";
            return;
        }

        if (!double.TryParse(DataValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
        {
            DataErrorMessage = "Value must be a valid number.";
            return;
        }

        try
        {
            var result = _dataSizeConverter.Convert(value, SelectedDataFromUnit, SelectedDataToUnit);
            DataResult = $"Result: {result} {SelectedDataToUnit}";
        }
        catch (Exception ex)
        {
            DataErrorMessage = ex.Message;
        }
    }
}
