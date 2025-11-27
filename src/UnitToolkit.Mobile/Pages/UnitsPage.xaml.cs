using Microsoft.Extensions.Logging;
using UnitToolkit.Presentation.ViewModels;

namespace UnitToolkit.Mobile;

public partial class UnitsPage : ContentPage
{
	private readonly ILogger<UnitsPage> _logger;

	public UnitsPage(UnitsViewModel viewModel, ILogger<UnitsPage> logger)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_logger = logger;
		_logger.LogInformation("UnitsPage loaded");
	}

	private async void OnBackClicked(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating back to dashboard");
		await Shell.Current.GoToAsync("..");
	}
}
