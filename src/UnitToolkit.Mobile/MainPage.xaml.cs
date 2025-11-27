using Microsoft.Extensions.Logging;

namespace UnitToolkit.Mobile;

public partial class MainPage : ContentPage
{
	private readonly ILogger<MainPage> _logger;

	public MainPage(ILogger<MainPage> logger)
	{
		InitializeComponent();
		_logger = logger;
		_logger.LogInformation("MainPage dashboard loaded");
	}

	private async void OnUnitsCardTapped(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating to Units page");
		await Shell.Current.GoToAsync(nameof(UnitsPage));
	}

	private async void OnCurrencyCardTapped(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating to Currency page");
		await Shell.Current.GoToAsync(nameof(CurrencyPage));
	}

	private async void OnPasswordCardTapped(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating to Password page");
		await Shell.Current.GoToAsync(nameof(PasswordPage));
	}

	private async void OnBmiCardTapped(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating to BMI page");
		await Shell.Current.GoToAsync(nameof(BmiPage));
	}

	private async void OnDataSizeCardTapped(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating to Data Size page");
		await Shell.Current.GoToAsync(nameof(DataSizePage));
	}
}
