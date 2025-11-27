using Microsoft.Extensions.Logging;
using UnitToolkit.Presentation.ViewModels;

namespace UnitToolkit.Mobile;

public partial class PasswordPage : ContentPage
{
	private readonly ILogger<PasswordPage> _logger;

	public PasswordPage(PasswordViewModel viewModel, ILogger<PasswordPage> logger)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_logger = logger;
		_logger.LogInformation("PasswordPage loaded");
	}

	private async void OnBackClicked(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating back to dashboard");
		await Shell.Current.GoToAsync("..");
	}
}
