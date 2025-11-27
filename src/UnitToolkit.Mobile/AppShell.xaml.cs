namespace UnitToolkit.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register routes for navigation
		Routing.RegisterRoute(nameof(UnitsPage), typeof(UnitsPage));
		Routing.RegisterRoute(nameof(CurrencyPage), typeof(CurrencyPage));
		Routing.RegisterRoute(nameof(PasswordPage), typeof(PasswordPage));
		Routing.RegisterRoute(nameof(BmiPage), typeof(BmiPage));
		Routing.RegisterRoute(nameof(DataSizePage), typeof(DataSizePage));
	}
}
