using System;
using System.Globalization;
using System.Windows.Data;

namespace WHampson.ToolUI.Converters
{
    /// <summary>
    /// Tests whether an object is null.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class NullConverter : IValueConverter
    {
        /// <summary>
        /// Setting this to true will invert the null test result,
        /// effectively making this a "NotNullConverter".
        /// </summary>
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Invert)
                ? value != null
                : value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Cannot convert '{value}' to type {targetType}.");
        }
    }
}
