using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using UnitToolkit.Desktop.ViewModels;

namespace UnitToolkit.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void UnitsCard_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.NavigateToMeasurementsCommand.Execute(null);
        }
    }

    private void CurrencyCard_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.NavigateToCurrencyCommand.Execute(null);
        }
    }

    private void PasswordCard_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.NavigateToPasswordCommand.Execute(null);
        }
    }

    private void BmiCard_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.NavigateToBmiCommand.Execute(null);
        }
    }

    private void DataSizeCard_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.NavigateToDataSizeCommand.Execute(null);
        }
    }
}
