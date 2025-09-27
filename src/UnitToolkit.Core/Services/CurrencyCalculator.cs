namespace UnitToolkit.Core.Services;

public class CurrencyCalculator
{
    /// <summary>
    /// Convert amount by a fixed rate.
    /// </summary>
    /// <param name="amount">Amount in source currency.</param>
    /// <param name="rate">Rate FROM→TO (e.g., 1 EUR → 1.1 USD =&gt; rate=1.1).</param>
    /// <returns>Amount in target currency.</returns>
    public double Convert(double amount, double rate)
    {
        return amount * rate;
    }
}