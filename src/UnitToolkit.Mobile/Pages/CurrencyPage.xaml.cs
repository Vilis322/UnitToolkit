using Microsoft.Extensions.Logging;
using UnitToolkit.Presentation.ViewModels;

namespace UnitToolkit.Mobile;

public partial class CurrencyPage : ContentPage
{
	private readonly ILogger<CurrencyPage> _logger;

	public CurrencyPage(CurrencyViewModel viewModel, ILogger<CurrencyPage> logger)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_logger = logger;
		_logger.LogInformation("CurrencyPage loaded");
	}

	private async void OnBackClicked(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating back to dashboard");
		await Shell.Current.GoToAsync("..");
	}
}
