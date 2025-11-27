using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UnitToolkit.Core.Services;

namespace UnitToolkit.Presentation.ViewModels;

/// <summary>
/// ViewModel for data size converter.
/// Responsibilities:
/// - Convert between different data size units (B, KB, MB, GB, etc.)
/// - Support both decimal (KB, MB) and binary (KiB, MiB) units
/// - Validate numeric input
/// Dependencies: DataSizeConverter from Core
/// </summary>
public partial class DataSizeViewModel : ViewModelBase
{
    private readonly DataSizeConverter _dataSizeConverter;

    public DataSizeViewModel(DataSizeConverter dataSizeConverter)
    {
        _dataSizeConverter = dataSizeConverter;

        // Initialize data size units
        Units = new List<string> { "B", "KB", "MB", "GB", "TB", "KiB", "MiB", "GiB", "TiB" };
        SelectedFromUnit = Units[1]; // KB
        SelectedToUnit = Units[2];   // MB
    }

    [ObservableProperty]
    private List<string> units = new();

    [ObservableProperty]
    private string value = string.Empty;

    [ObservableProperty]
    private string selectedFromUnit = string.Empty;

    [ObservableProperty]
    private string selectedToUnit = string.Empty;

    [ObservableProperty]
    private string result = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [RelayCommand]
    private void Convert()
    {
        Result = string.Empty;
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Value))
        {
            ErrorMessage = "Please enter a value to convert.";
            return;
        }

        if (!double.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
        {
            ErrorMessage = "Value must be a valid number.";
            return;
        }

        try
        {
            var resultValue = _dataSizeConverter.Convert(value, SelectedFromUnit, SelectedToUnit);
            Result = $"Result: {resultValue} {SelectedToUnit}";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
