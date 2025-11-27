using System.Globalization;

namespace UnitToolkit.Mobile.Converters;

public class StringNotEmptyConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value is string str && !string.IsNullOrWhiteSpace(str);
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
