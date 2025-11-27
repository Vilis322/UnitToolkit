using CommunityToolkit.Mvvm.ComponentModel;

namespace UnitToolkit.Presentation.ViewModels;

/// <summary>
/// Main ViewModel for the dashboard/navigation.
/// Responsibilities:
/// - Manage navigation state between different tool pages
/// - Coordinate tool-specific ViewModels
/// Dependencies: All tool ViewModels
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    public UnitsViewModel UnitsViewModel { get; }
    public CurrencyViewModel CurrencyViewModel { get; }
    public PasswordViewModel PasswordViewModel { get; }
    public BmiViewModel BmiViewModel { get; }
    public DataSizeViewModel DataSizeViewModel { get; }

    public MainViewModel(
        UnitsViewModel unitsViewModel,
        CurrencyViewModel currencyViewModel,
        PasswordViewModel passwordViewModel,
        BmiViewModel bmiViewModel,
        DataSizeViewModel dataSizeViewModel)
    {
        UnitsViewModel = unitsViewModel;
        CurrencyViewModel = currencyViewModel;
        PasswordViewModel = passwordViewModel;
        BmiViewModel = bmiViewModel;
        DataSizeViewModel = dataSizeViewModel;
    }
}
