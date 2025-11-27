using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UnitToolkit.Core.Services;

namespace UnitToolkit.Presentation.ViewModels;

/// <summary>
/// ViewModel for unit conversion tool.
/// Responsibilities:
/// - Manage unit conversion types (Temperature, Length, Mass)
/// - Handle unit selection and conversion logic
/// - Validate user input and display errors
/// Dependencies: UnitConverter from Core
/// </summary>
public partial class UnitsViewModel : ViewModelBase
{
    private readonly UnitConverter _unitConverter;

    public UnitsViewModel(UnitConverter unitConverter)
    {
        _unitConverter = unitConverter;

        // Initialize unit type collections
        ConversionTypes = new List<string> { "Temperature", "Length", "Mass" };
        SelectedConversionType = ConversionTypes[0];
        UpdateUnitLists();
    }

    [ObservableProperty]
    private List<string> conversionTypes = new();

    [ObservableProperty]
    private string selectedConversionType = string.Empty;

    [ObservableProperty]
    private List<string> availableUnits = new();

    [ObservableProperty]
    private string selectedFromUnit = string.Empty;

    [ObservableProperty]
    private string selectedToUnit = string.Empty;

    [ObservableProperty]
    private string unitValue = string.Empty;

    [ObservableProperty]
    private string unitResult = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    partial void OnSelectedConversionTypeChanged(string value)
    {
        UpdateUnitLists();
    }

    private void UpdateUnitLists()
    {
        AvailableUnits = SelectedConversionType switch
        {
            "Temperature" => _unitConverter.TemperatureUnits.ToList(),
            "Length" => _unitConverter.LengthUnits.ToList(),
            "Mass" => _unitConverter.MassUnits.ToList(),
            _ => _unitConverter.TemperatureUnits.ToList()
        };

        if (AvailableUnits.Count >= 2)
        {
            SelectedFromUnit = AvailableUnits[0];
            SelectedToUnit = AvailableUnits[1];
        }
    }

    [RelayCommand]
    private void Convert()
    {
        UnitResult = string.Empty;
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(UnitValue))
        {
            ErrorMessage = "Please enter a value to convert.";
            return;
        }

        if (!double.TryParse(UnitValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
        {
            ErrorMessage = "Value must be a valid number. Use dot (.) for decimals.";
            return;
        }

        try
        {
            var type = SelectedConversionType.ToLowerInvariant();
            var result = _unitConverter.Convert(type, value, SelectedFromUnit, SelectedToUnit);

            // Format result based on conversion type
            string formattedResult;
            if (type == "mass" && SelectedFromUnit.ToLowerInvariant() == "kg" && SelectedToUnit.ToLowerInvariant() == "lb")
            {
                formattedResult = result.ToString("F2", CultureInfo.InvariantCulture);
            }
            else
            {
                formattedResult = result.ToString(CultureInfo.InvariantCulture);
            }

            UnitResult = $"Result: {formattedResult} {SelectedToUnit}";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
