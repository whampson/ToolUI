using System;
using System.Globalization;
using System.Windows.Data;

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
            bool isNumeric;
            bool result = false;

            if (value == null)
            {
                result = false;
                goto Done;
            }

            Type t = value.GetType();
            t = Nullable.GetUnderlyingType(t) ?? t;

            if (t.IsPrimitive)
            {
                isNumeric =
                    t != typeof(bool) &&
                    t != typeof(char) &&
                    t != typeof(IntPtr) &&
                    t != typeof(UIntPtr);
            }
            else
            {
                isNumeric = (t == typeof(decimal));
            }

            if (!isNumeric)
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
