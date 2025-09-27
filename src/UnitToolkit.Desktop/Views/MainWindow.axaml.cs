using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using UnitToolkit.Core.Services;

namespace UnitToolkit.Desktop.Views;

public partial class MainWindow : Window
{
    // Services from Core
    private readonly UnitConverter _unit = new();
    private readonly CurrencyCalculator _currency = new();
    private readonly PasswordGenerator _pwd = new();
    private readonly BmiCalculator _bmi = new();
    private readonly DataSizeConverter _data = new();

    public MainWindow()
    {
        InitializeComponent();
        PopulateUnitBoxes("Temperature");
        UnitTypeBox.SelectionChanged += (_, _) =>
        {
            var type = (UnitTypeBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Temperature";
            PopulateUnitBoxes(type);
        };

        PopulateDataSizeBoxes();
    }

    // ---------- Units ----------
    private void PopulateUnitBoxes(string type)
    {
        FromUnitBox.Items.Clear();
        ToUnitBox.Items.Clear();

        switch (type)
        {
            case "Temperature":
                FromUnitBox.Items.Add("C"); FromUnitBox.Items.Add("F"); FromUnitBox.Items.Add("K");
                ToUnitBox.Items.Add("C");   ToUnitBox.Items.Add("F");   ToUnitBox.Items.Add("K");
                break;
            case "Length":
                FromUnitBox.Items.Add("m"); FromUnitBox.Items.Add("cm"); FromUnitBox.Items.Add("km");
                ToUnitBox.Items.Add("m");   ToUnitBox.Items.Add("cm");   ToUnitBox.Items.Add("km");
                break;
            default: // Mass
                FromUnitBox.Items.Add("kg"); FromUnitBox.Items.Add("g"); FromUnitBox.Items.Add("lb");
                ToUnitBox.Items.Add("kg");   ToUnitBox.Items.Add("g");   ToUnitBox.Items.Add("lb");
                break;
        }
        FromUnitBox.SelectedIndex = 0;
        ToUnitBox.SelectedIndex = 1;
    }

    private void OnUnitsConvertClick(object? sender, RoutedEventArgs e)
    {
        UnitResultText.Text = string.Empty;
        try
        {
            var type = (UnitTypeBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Temperature";
            if (!double.TryParse(UnitValueBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var v))
                throw new FormatException("Value must be a number (use dot for decimals).");

            var from = FromUnitBox.SelectedItem?.ToString() ?? "";
            var to   = ToUnitBox.SelectedItem?.ToString() ?? "";

            var result = _unit.Convert(type.ToLowerInvariant(), v, from, to);
            UnitResultText.Text = $"Result: {result} {to}";
        }
        catch (Exception ex)
        {
            UnitResultText.Text = $"Error: {ex.Message}";
        }
    }

    // ---------- Currency ----------
    private void OnCurrencyConvertClick(object? sender, RoutedEventArgs e)
    {
        CurResultText.Text = string.Empty;
        try
        {
            if (!double.TryParse(CurAmountBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var amount))
                throw new FormatException("Amount must be a number.");
            if (!double.TryParse(CurRateBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var rate))
                throw new FormatException("Rate must be a number.");
            var result = _currency.Convert(amount, rate);
            CurResultText.Text = $"Converted: {result}";
        }
        catch (Exception ex)
        {
            CurResultText.Text = $"Error: {ex.Message}";
        }
    }

    // ---------- Password / Random ----------
    private void OnGeneratePasswordClick(object? sender, RoutedEventArgs e)
    {
        PwdResultText.Text = string.Empty;
        try
        {
            if (!int.TryParse(PwdLengthBox.Text, out var len))
                throw new FormatException("Length must be an integer.");
            var password = _pwd.Generate(len,
                useUpper:   PwdUpperBox.IsChecked == true,
                useDigits:  PwdDigitsBox.IsChecked == true,
                useSymbols: PwdSymbolsBox.IsChecked == true);
            PwdResultText.Text = $"Password: {password}";
        }
        catch (Exception ex)
        {
            PwdResultText.Text = $"Error: {ex.Message}";
        }
    }

    private void OnRandomIntClick(object? sender, RoutedEventArgs e)
    {
        RandResultText.Text = string.Empty;
        try
        {
            if (!int.TryParse(RandMinBox.Text, out var min))
                throw new FormatException("Min must be an integer.");
            if (!int.TryParse(RandMaxBox.Text, out var max))
                throw new FormatException("Max must be an integer.");
            var n = _pwd.RandomInt(min, max);
            RandResultText.Text = $"Random int: {n}";
        }
        catch (Exception ex)
        {
            RandResultText.Text = $"Error: {ex.Message}";
        }
    }

    // ---------- BMI ----------
    private void OnBmiCalculateClick(object? sender, RoutedEventArgs e)
    {
        BmiResultText.Text = string.Empty;
        try
        {
            if (!double.TryParse(BmiHeightBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var height))
                throw new FormatException("Height must be a number.");
            if (!double.TryParse(BmiWeightBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var weight))
                throw new FormatException("Weight must be a number.");

            var (bmi, cat, advice) = _bmi.Calculate(height, weight);
            BmiResultText.Text = $"BMI: {bmi:F1} — {cat}\nAdvice: {advice}";
        }
        catch (Exception ex)
        {
            BmiResultText.Text = $"Error: {ex.Message}";
        }
    }

    // ---------- Data Size ----------
    private void PopulateDataSizeBoxes()
    {
        // SI
        DataFromBox.Items.Add("B");  DataToBox.Items.Add("B");
        DataFromBox.Items.Add("KB"); DataToBox.Items.Add("KB");
        DataFromBox.Items.Add("MB"); DataToBox.Items.Add("MB");
        DataFromBox.Items.Add("GB"); DataToBox.Items.Add("GB");
        DataFromBox.Items.Add("TB"); DataToBox.Items.Add("TB");
        // IEC
        DataFromBox.Items.Add("KiB"); DataToBox.Items.Add("KiB");
        DataFromBox.Items.Add("MiB"); DataToBox.Items.Add("MiB");
        DataFromBox.Items.Add("GiB"); DataToBox.Items.Add("GiB");
        DataFromBox.Items.Add("TiB"); DataToBox.Items.Add("TiB");

        DataFromBox.SelectedIndex = 1; // KB
        DataToBox.SelectedIndex   = 2; // MB
    }

    private void OnDataSizeConvertClick(object? sender, RoutedEventArgs e)
    {
        DataResultText.Text = string.Empty;
        try
        {
            if (!double.TryParse(DataValueBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                throw new FormatException("Value must be a number.");
            var from = DataFromBox.SelectedItem?.ToString() ?? "KB";
            var to   = DataToBox.SelectedItem?.ToString() ?? "MB";

            var result = _data.Convert(value, from, to);
            DataResultText.Text = $"Result: {result} {to}";
        }
        catch (Exception ex)
        {
            DataResultText.Text = $"Error: {ex.Message}";
        }
    }
}
