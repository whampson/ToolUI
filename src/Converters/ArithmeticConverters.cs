using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WHampson.ToolUI.Converters
{
    public class AddConverter : ArithmeticConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double r = DoConversion(Operation.Add, values.Cast<double>().ToArray());
            return System.Convert.ChangeType(r, targetType);
        }
    }

    public class SubtractConverter : ArithmeticConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double r = DoConversion(Operation.Subtract, values.Cast<double>().ToArray());
            return System.Convert.ChangeType(r, targetType);
        }
    }

    public class MultiplyConverter : ArithmeticConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double r = DoConversion(Operation.Multiply, values.Cast<double>().ToArray());
            return System.Convert.ChangeType(r, targetType);
        }
    }

    public class DivideConverter : ArithmeticConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var r = DoConversion(Operation.Divide, values.Cast<double>().ToArray());
            return System.Convert.ChangeType(r, targetType);
        }
    }

    public abstract class ArithmeticConverter : IMultiValueConverter
    {
        public double? ClampMin { get; set; }
        public double? ClampMax { get; set; }

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();

        protected double DoConversion(Operation op, double[] x)
        {
            double r = x[0];

            for (int i = 1; i < x.Length; i++)
            {
                switch (op)
                {
                    case Operation.Add:
                        r += x[i];
                        break;
                    case Operation.Subtract:
                        r -= x[i];
                        break;
                    case Operation.Multiply:
                        r *= x[i];
                        break;
                    case Operation.Divide:
                        r /= x[i];
                        break;
                }
            }

            double min = ClampMin ?? double.MinValue;
            double max = ClampMax ?? double.MaxValue;
            return Math.Clamp(r, min, max);
        }

        protected enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }
    }
}
