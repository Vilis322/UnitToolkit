using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UnitToolkit.Core.Services;

namespace UnitToolkit.Presentation.ViewModels;

/// <summary>
/// ViewModel for password generation and random integer tools.
/// Responsibilities:
/// - Generate secure passwords with configurable options
/// - Generate random integers within a specified range
/// - Validate user input for length and range
/// Dependencies: PasswordGenerator from Core
/// </summary>
public partial class PasswordViewModel : ViewModelBase
{
    private readonly PasswordGenerator _passwordGenerator;

    public PasswordViewModel(PasswordGenerator passwordGenerator)
    {
        _passwordGenerator = passwordGenerator;
    }

    [ObservableProperty]
    private string passwordLength = "12";

    [ObservableProperty]
    private bool useUppercase = true;

    [ObservableProperty]
    private bool useDigits = true;

    [ObservableProperty]
    private bool useSymbols = false;

    [ObservableProperty]
    private string passwordResult = string.Empty;

    [ObservableProperty]
    private string passwordErrorMessage = string.Empty;

    [ObservableProperty]
    private string randomMin = string.Empty;

    [ObservableProperty]
    private string randomMax = string.Empty;

    [ObservableProperty]
    private string randomResult = string.Empty;

    [ObservableProperty]
    private string randomErrorMessage = string.Empty;

    [RelayCommand]
    private void GeneratePassword()
    {
        PasswordResult = string.Empty;
        PasswordErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(PasswordLength))
        {
            PasswordErrorMessage = "Please enter a password length.";
            return;
        }

        if (!int.TryParse(PasswordLength, out var length))
        {
            PasswordErrorMessage = "Length must be a valid integer.";
            return;
        }

        try
        {
            var password = _passwordGenerator.Generate(length, UseUppercase, UseDigits, UseSymbols);
            PasswordResult = $"Password: {password}";
        }
        catch (Exception ex)
        {
            PasswordErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void GenerateRandomInt()
    {
        RandomResult = string.Empty;
        RandomErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(RandomMin))
        {
            RandomErrorMessage = "Please enter a minimum value.";
            return;
        }

        if (string.IsNullOrWhiteSpace(RandomMax))
        {
            RandomErrorMessage = "Please enter a maximum value.";
            return;
        }

        if (!int.TryParse(RandomMin, out var min))
        {
            RandomErrorMessage = "Minimum must be a valid integer.";
            return;
        }

        if (!int.TryParse(RandomMax, out var max))
        {
            RandomErrorMessage = "Maximum must be a valid integer.";
            return;
        }

        try
        {
            var result = _passwordGenerator.RandomInt(min, max);
            RandomResult = $"Random int: {result}";
        }
        catch (Exception ex)
        {
            RandomErrorMessage = ex.Message;
        }
    }
}
