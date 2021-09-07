using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WHampson.ToolUI.Converters
{
    public class AndConverter : LogicalConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool r = DoConversion(Operation.And, values.Cast<bool>().ToArray());
            return System.Convert.ChangeType(r, targetType);
        }
    }

    public class OrConverter : LogicalConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool r = DoConversion(Operation.Or, values.Cast<bool>().ToArray());
            return System.Convert.ChangeType(r, targetType);
        }
    }

    public class XorConverter : LogicalConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool r = DoConversion(Operation.Xor, values.Cast<bool>().ToArray());
            return System.Convert.ChangeType(r, targetType);
        }
    }

    public abstract class LogicalConverter : IMultiValueConverter
    {
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();

        protected bool DoConversion(Operation op, bool[] x)
        {
            bool r = x[0];

            for (int i = 1; i < x.Length; i++)
            {
                switch (op)
                {
                    case Operation.And:
                        r &= x[i];
                        break;
                    case Operation.Or:
                        r |= x[i];
                        break;
                    case Operation.Xor:
                        r ^= x[i];
                        break;
                }
            }

            return r;
        }

        protected enum Operation
        {
            And,
            Or,
            Xor
        }
    }
}
