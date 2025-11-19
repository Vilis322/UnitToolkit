using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using UnitToolkit.Core.Services;
using UnitToolkit.Desktop.ViewModels;
using UnitToolkit.Desktop.Views;

namespace UnitToolkit.Desktop;

public partial class App : Application
{
    public static ServiceProvider? ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        // Register Core services
        services.AddSingleton<UnitConverter>();
        services.AddSingleton<CurrencyCalculator>();
        services.AddSingleton<PasswordGenerator>();
        services.AddSingleton<BmiCalculator>();
        services.AddSingleton<DataSizeConverter>();

        // Register ViewModels
        services.AddTransient<MainViewModel>();

        ServiceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
