using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitToolkit.Core.Services;

public record ExchangeRate(string Currency, double Rate);

public record CurrencyExchangeData(string BaseCurrency, Dictionary<string, double> Rates);

public class CurrencyExchangeService
{
    private readonly HttpClient _httpClient;
    private const string ApiBaseUrl = "https://api.frankfurter.app";

    // Common currencies to display
    public static readonly string[] CommonCurrencies = new[]
    {
        "USD", "EUR", "GBP", "JPY", "CHF", "CAD", "AUD", "CNY", "INR", "RUB"
    };

    public CurrencyExchangeService(HttpClient? httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient();
    }

    /// <summary>
    /// Get all available currencies
    /// </summary>
    public async Task<List<string>> GetAvailableCurrenciesAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"{ApiBaseUrl}/currencies");
            var currencies = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
            return currencies?.Keys.OrderBy(c => c).ToList() ?? new List<string>();
        }
        catch
        {
            // Return fallback list if API fails
            return new List<string> { "USD", "EUR", "GBP", "JPY", "CHF", "CAD", "AUD", "CNY", "INR", "RUB", "BRL", "MXN", "ZAR", "SEK", "NOK", "DKK", "PLN", "TRY", "NZD", "SGD" };
        }
    }

    /// <summary>
    /// Convert amount between currencies
    /// </summary>
    public async Task<double> ConvertAsync(double amount, string fromCurrency, string toCurrency)
    {
        if (string.IsNullOrWhiteSpace(fromCurrency) || string.IsNullOrWhiteSpace(toCurrency))
            throw new ArgumentException("Currency codes cannot be empty");

        if (fromCurrency.Equals(toCurrency, StringComparison.OrdinalIgnoreCase))
            return amount;

        try
        {
            var url = $"{ApiBaseUrl}/latest?amount={amount}&from={fromCurrency.ToUpper()}&to={toCurrency.ToUpper()}";
            var response = await _httpClient.GetStringAsync(url);
            var json = JsonDocument.Parse(response);

            var rates = json.RootElement.GetProperty("rates");
            if (rates.TryGetProperty(toCurrency.ToUpper(), out var rateElement))
            {
                return rateElement.GetDouble();
            }

            throw new InvalidOperationException($"Could not get exchange rate for {toCurrency}");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch exchange rates: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Get exchange rates for common currencies relative to a base currency
    /// </summary>
    public async Task<List<ExchangeRate>> GetCommonRatesAsync(string baseCurrency = "USD")
    {
        try
        {
            var currenciesToFetch = CommonCurrencies.Where(c => !c.Equals(baseCurrency, StringComparison.OrdinalIgnoreCase)).ToArray();
            var currenciesParam = string.Join(",", currenciesToFetch);

            var url = $"{ApiBaseUrl}/latest?from={baseCurrency.ToUpper()}&to={currenciesParam}";
            var response = await _httpClient.GetStringAsync(url);
            var json = JsonDocument.Parse(response);

            var rates = json.RootElement.GetProperty("rates");
            var result = new List<ExchangeRate>();

            foreach (var property in rates.EnumerateObject())
            {
                result.Add(new ExchangeRate(property.Name, property.Value.GetDouble()));
            }

            return result.OrderBy(r => r.Currency).ToList();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch exchange rates: {ex.Message}", ex);
        }
    }
}
