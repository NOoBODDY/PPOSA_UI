using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace SecretaryDesktopApp.Converters;

public class Base64ToBitmapConverter:IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType == typeof(IImage) && value is string base64)
        {
            byte[] binaryData = System.Convert.FromBase64String(base64);
            var stream = new MemoryStream(binaryData);
            return new Bitmap(stream);
        }

        throw new NotSupportedException(targetType.FullName);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType == typeof(string) && value is IImage)
        {
            switch (value)
            {
                case Bitmap bitmap:
                    var stream = new MemoryStream();
                    bitmap.Save(stream);
                    return System.Convert.ToBase64String(stream.ToArray());
                    break;
                default:
                    throw new NotSupportedException(value.ToString());
            }
        }
        throw new NotSupportedException(targetType.FullName);
    }
}