using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SecretaryDesktopApp.Converters;

public class StringToGeometryConverter:IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType == typeof(Geometry) && value is string stringPath)
            return Geometry.Parse(stringPath);
        throw new NotSupportedException();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType == typeof(string) && value is Geometry path)
            return path.ToString();
        throw new NotSupportedException();
    }
}