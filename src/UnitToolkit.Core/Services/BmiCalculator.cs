using System;

namespace UnitToolkit.Core.Services;

public record BmiResult(double Value, string Category, string Advice);

public class BmiCalculator
{
    /// <summary>
    /// Calculate Body Mass Index and return structured result.
    /// Height can be given in meters (e.g., 1.70) or centimeters (e.g., 170).
    /// </summary>
    /// <param name="heightInput">Height in meters or centimeters.</param>
    /// <param name="weightKg">Weight in kilograms.</param>
    /// <returns>BmiResult containing the BMI value, category, and health advice.</returns>
    /// <exception cref="ArgumentException">If height or weight is not positive.</exception>
    public BmiResult Calculate(double heightInput, double weightKg)
    {
        double meters = heightInput > 3 ? heightInput / 100.0 : heightInput;

        if (meters <= 0 || weightKg <= 0)
            throw new ArgumentException("Height and weight must be positive.");

        double bmi = weightKg / (meters * meters);

        var (category, advice) = bmi switch
        {
            < 18.5                 => ("Underweight", "Consider a calorie-adequate diet and monitoring."),
            >= 18.5 and < 25.0     => ("Normal", "Maintain balanced nutrition and regular activity."),
            >= 25.0 and < 30.0     => ("Overweight", "Increase physical activity and watch portions."),
            _                      => ("Obese", "Consult a healthcare professional for guidance.")
        };

        return new BmiResult(bmi, category, advice);
    }
}