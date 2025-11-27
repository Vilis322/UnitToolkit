using Microsoft.Extensions.Logging;

namespace UnitToolkit.Mobile;

public partial class App : Application
{
	private readonly ILogger<App>? _logger;

	public App()
	{
		InitializeComponent();

		// Get logger if available
		_logger = Handler?.MauiContext?.Services?.GetService<ILogger<App>>();

		// Set up global exception handling
		AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
		TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

		_logger?.LogInformation("UnitToolkit Mobile app starting...");
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		_logger?.LogInformation("Creating main window");
		return new Window(new AppShell());
	}

	private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		if (e.ExceptionObject is Exception exception)
		{
			_logger?.LogCritical(exception, "Unhandled exception occurred");
			System.Diagnostics.Debug.WriteLine($"[CRASH] Unhandled exception: {exception}");
		}
	}

	private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
	{
		_logger?.LogError(e.Exception, "Unobserved task exception occurred");
		System.Diagnostics.Debug.WriteLine($"[ERROR] Unobserved task exception: {e.Exception}");
		e.SetObserved(); // Mark as observed to prevent app crash
	}
}