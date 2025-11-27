using Microsoft.Extensions.Logging;
using UnitToolkit.Presentation.ViewModels;

namespace UnitToolkit.Mobile;

public partial class BmiPage : ContentPage
{
	private readonly ILogger<BmiPage> _logger;

	public BmiPage(BmiViewModel viewModel, ILogger<BmiPage> logger)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_logger = logger;
		_logger.LogInformation("BmiPage loaded");
	}

	private async void OnBackClicked(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating back to dashboard");
		await Shell.Current.GoToAsync("..");
	}
}
