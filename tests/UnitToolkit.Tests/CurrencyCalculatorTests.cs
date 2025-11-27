using System.Globalization;
using System.Net;
using System.Text;
using UnitToolkit.Core.Services;
using Xunit;

namespace UnitToolkit.Tests;

public class CurrencyExchangeServiceTests
{
    private static HttpClient CreateMockHttpClient(string jsonResponse)
    {
        var handler = new MockHttpMessageHandler(jsonResponse);
        return new HttpClient(handler);
    }

    [Theory]
    [InlineData(100, "USD", "EUR", 1.1, 110)]
    [InlineData(0, "USD", "EUR", 1.5, 0)]
    [InlineData(50, "EUR", "USD", 2.0, 100)]
    public async Task ConvertAsync_WithValidRate_ReturnsConvertedAmount(
        double amount, string fromCurrency, string toCurrency, double rate, double expected)
    {
        // Arrange
        var rateStr = rate.ToString(CultureInfo.InvariantCulture);
        var jsonResponse = $@"{{
            ""base_code"": ""{fromCurrency}"",
            ""rates"": {{
                ""{toCurrency}"": {rateStr}
            }}
        }}";

        var httpClient = CreateMockHttpClient(jsonResponse);
        var service = new CurrencyExchangeService(httpClient);

        // Act
        var actual = await service.ConvertAsync(amount, fromCurrency, toCurrency);

        // Assert
        Assert.Equal(expected, actual, 10);
    }

    [Fact]
    public async Task ConvertAsync_SameCurrency_ReturnsOriginalAmount()
    {
        // Arrange
        var service = new CurrencyExchangeService();

        // Act
        var result = await service.ConvertAsync(100, "USD", "USD");

        // Assert
        Assert.Equal(100, result);
    }

    [Fact]
    public async Task ConvertAsync_EmptyCurrency_ThrowsArgumentException()
    {
        // Arrange
        var service = new CurrencyExchangeService();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await service.ConvertAsync(100, "", "USD"));
    }

    [Fact]
    public async Task GetCommonRatesAsync_ReturnsOnlyCommonCurrencies()
    {
        // Arrange
        var jsonResponse = @"{
            ""base_code"": ""USD"",
            ""rates"": {
                ""EUR"": 0.85,
                ""GBP"": 0.73,
                ""JPY"": 110.5,
                ""XYZ"": 1.5
            }
        }";

        var httpClient = CreateMockHttpClient(jsonResponse);
        var service = new CurrencyExchangeService(httpClient);

        // Act
        var rates = await service.GetCommonRatesAsync("USD");

        // Assert
        Assert.NotEmpty(rates);
        Assert.All(rates, rate =>
            Assert.Contains(rate.Currency, CurrencyExchangeService.CommonCurrencies));
        Assert.DoesNotContain(rates, rate => rate.Currency == "XYZ");
    }
}

// Mock HTTP Message Handler for testing
public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly string _response;

    public MockHttpMessageHandler(string response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(_response, Encoding.UTF8, "application/json")
        });
    }
}
