using Microsoft.Extensions.Logging;
using UnitToolkit.Presentation.ViewModels;

namespace UnitToolkit.Mobile;

public partial class DataSizePage : ContentPage
{
	private readonly ILogger<DataSizePage> _logger;

	public DataSizePage(DataSizeViewModel viewModel, ILogger<DataSizePage> logger)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_logger = logger;
		_logger.LogInformation("DataSizePage loaded");
	}

	private async void OnBackClicked(object? sender, EventArgs e)
	{
		_logger.LogInformation("Navigating back to dashboard");
		await Shell.Current.GoToAsync("..");
	}
}
