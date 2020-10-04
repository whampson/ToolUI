using System;
using System.Globalization;
using System.Windows.Data;

namespace WHampson.ToolUI.Converters
{
    /// <summary>
    /// Tests whether an object is null.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets whether to invert the null test result.
        /// </summary>
        /// <remarks>
        /// Setting this to <c>true</c> will effectively turn the converter into
        /// a "IsNotNull" converter.
        /// </remarks>
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Invert)
            {
                return value != null;
            }
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"ConvertBack is not supported for this converter.");
        }
    }
}
