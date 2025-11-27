using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UnitToolkit.Core.Services;

namespace UnitToolkit.Presentation.ViewModels;

/// <summary>
/// ViewModel for BMI (Body Mass Index) calculator.
/// Responsibilities:
/// - Calculate BMI from height and weight
/// - Display BMI category and health advice
/// - Validate numeric input
/// Dependencies: BmiCalculator from Core
/// </summary>
public partial class BmiViewModel : ViewModelBase
{
    private readonly BmiCalculator _bmiCalculator;

    public BmiViewModel(BmiCalculator bmiCalculator)
    {
        _bmiCalculator = bmiCalculator;
    }

    [ObservableProperty]
    private string height = string.Empty;

    [ObservableProperty]
    private string weight = string.Empty;

    [ObservableProperty]
    private string result = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [RelayCommand]
    private void Calculate()
    {
        Result = string.Empty;
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Height))
        {
            ErrorMessage = "Please enter your height.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Weight))
        {
            ErrorMessage = "Please enter your weight.";
            return;
        }

        if (!double.TryParse(Height, NumberStyles.Float, CultureInfo.InvariantCulture, out var height))
        {
            ErrorMessage = "Height must be a valid number.";
            return;
        }

        if (!double.TryParse(Weight, NumberStyles.Float, CultureInfo.InvariantCulture, out var weight))
        {
            ErrorMessage = "Weight must be a valid number.";
            return;
        }

        try
        {
            var bmiResult = _bmiCalculator.Calculate(height, weight);
            Result = $"BMI: {bmiResult.Value:F1} — {bmiResult.Category}\nAdvice: {bmiResult.Advice}";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
