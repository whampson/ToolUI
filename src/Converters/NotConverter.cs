using System;
using System.Globalization;
using System.Windows.Data;

namespace WHampson.ToolUI.Converters
{
    /// <summary>
    /// Negates a boolean value.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class NotConverter : IValueConverter
    {
        /// <summary>
        /// The default value to return if the specified value is not a <see cref="bool"/> or is null.
        /// </summary>
        public bool DefaultValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is bool))
            {
                return DefaultValue;
            }

            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && targetType == typeof(bool))
            {
                return !b;
            }

            throw new InvalidOperationException($"ConvertBack is not supported for type '{targetType}'.");
        }
    }
}
