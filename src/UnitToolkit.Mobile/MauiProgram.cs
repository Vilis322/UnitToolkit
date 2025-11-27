using Microsoft.Extensions.Logging;
using UnitToolkit.Core.Services;
using UnitToolkit.Presentation.ViewModels;

namespace UnitToolkit.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Configure logging
		builder.Logging.AddDebug();
		builder.Logging.SetMinimumLevel(LogLevel.Debug);
		builder.Logging.AddFilter("UnitToolkit", LogLevel.Debug);

		// Register Core services
		builder.Services.AddSingleton<UnitConverter>();
		builder.Services.AddSingleton<CurrencyExchangeService>();
		builder.Services.AddSingleton<PasswordGenerator>();
		builder.Services.AddSingleton<BmiCalculator>();
		builder.Services.AddSingleton<DataSizeConverter>();

		// Register ViewModels
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<UnitsViewModel>();
		builder.Services.AddTransient<CurrencyViewModel>();
		builder.Services.AddTransient<PasswordViewModel>();
		builder.Services.AddTransient<BmiViewModel>();
		builder.Services.AddTransient<DataSizeViewModel>();

		// Register Pages
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<UnitsPage>();
		builder.Services.AddTransient<CurrencyPage>();
		builder.Services.AddTransient<PasswordPage>();
		builder.Services.AddTransient<BmiPage>();
		builder.Services.AddTransient<DataSizePage>();

		return builder.Build();
	}
}
