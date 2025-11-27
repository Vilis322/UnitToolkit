using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UnitToolkit.Core.Services;

namespace UnitToolkit.Presentation.ViewModels;

/// <summary>
/// ViewModel for currency conversion tool.
/// Responsibilities:
/// - Load available currencies from API
/// - Handle currency conversion with real-time exchange rates
/// - Manage loading state and error handling
/// Dependencies: CurrencyExchangeService from Core
/// </summary>
public partial class CurrencyViewModel : ViewModelBase
{
    private readonly CurrencyExchangeService _currencyExchangeService;

    public CurrencyViewModel(CurrencyExchangeService currencyExchangeService)
    {
        _currencyExchangeService = currencyExchangeService;
        LoadCurrenciesAsync();
    }

    [ObservableProperty]
    private List<string> availableCurrencies = new();

    [ObservableProperty]
    private string selectedFromCurrency = "EUR";

    [ObservableProperty]
    private string selectedToCurrency = "USD";

    [ObservableProperty]
    private string amount = string.Empty;

    [ObservableProperty]
    private string result = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isLoading = false;

    private async void LoadCurrenciesAsync()
    {
        try
        {
            IsLoading = true;
            var currencies = await _currencyExchangeService.GetAvailableCurrenciesAsync();
            AvailableCurrencies = currencies;

            // Set default selections if not already set
            if (!string.IsNullOrEmpty(SelectedFromCurrency) && currencies.Contains(SelectedFromCurrency))
            {
                // Keep current selection
            }
            else
            {
                SelectedFromCurrency = currencies.Contains("EUR") ? "EUR" : currencies.FirstOrDefault() ?? "";
            }

            if (!string.IsNullOrEmpty(SelectedToCurrency) && currencies.Contains(SelectedToCurrency))
            {
                // Keep current selection
            }
            else
            {
                SelectedToCurrency = currencies.Contains("USD") ? "USD" : currencies.Skip(1).FirstOrDefault() ?? "";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load currencies: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Convert()
    {
        Result = string.Empty;
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Amount))
        {
            ErrorMessage = "Please enter an amount.";
            return;
        }

        if (!double.TryParse(Amount, NumberStyles.Float, CultureInfo.InvariantCulture, out var amountValue))
        {
            ErrorMessage = "Amount must be a valid number.";
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedFromCurrency) || string.IsNullOrWhiteSpace(SelectedToCurrency))
        {
            ErrorMessage = "Please select currencies.";
            return;
        }

        try
        {
            IsLoading = true;
            var resultValue = await _currencyExchangeService.ConvertAsync(amountValue, SelectedFromCurrency, SelectedToCurrency);
            Result = $"{amountValue:N2} {SelectedFromCurrency} = {resultValue:N2} {SelectedToCurrency}";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
