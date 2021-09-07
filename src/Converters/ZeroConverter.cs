using System;
using System.Globalization;
using System.Windows.Data;
using WHampson.ToolUI.Extensions;

namespace WHampson.ToolUI.Converters
{
    /// <summary>
    /// Tests whether an object is zero.
    /// </summary>
    public class ZeroConverter : IValueConverter
    {
        /// <summary>
        /// Setting this to true will invert the null test result,
        /// effectively making this a "NotZeroConverter".
        /// </summary>
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (value == null)
            {
                result = false;
                goto Done;
            }

            Type t = value.GetType().GetUnderlyingType();
            if (!t.IsNumeric())
            {
                result = false;
                goto Done;
            }

            if (decimal.TryParse(value.ToString(), out decimal num))
            {
                result = (num == 0);
            }

        Done:
            if (Invert) result = !result;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Cannot convert '{value}' to type {targetType}.");
        }
    }
}
