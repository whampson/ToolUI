using System.Windows;
using System.Windows.Controls;

namespace WHampson.ToolUI.Controls
{
    public abstract class CoordinatePickerBase : UserControl
    {
        public static readonly DependencyProperty LabelMarginProperty = DependencyProperty.Register(
            nameof(LabelMargin), typeof(Thickness), typeof(CoordinatePickerBase),
            new FrameworkPropertyMetadata(new Thickness(5, 5, 5, 5)));

        public static readonly DependencyProperty UpDownMarginProperty = DependencyProperty.Register(
            nameof(UpDownMargin), typeof(Thickness), typeof(CoordinatePickerBase),
            new FrameworkPropertyMetadata(new Thickness(0, 2.5, 0, 2.5)));

        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register(
            nameof(FormatString), typeof(string), typeof(CoordinatePickerBase),
            new FrameworkPropertyMetadata(""));

        public static readonly DependencyProperty IsLabelVisibleProperty = DependencyProperty.Register(
            nameof(IsLabelVisible), typeof(bool), typeof(CoordinatePickerBase),
            new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register(
            nameof(LabelWidth), typeof(double), typeof(CoordinatePickerBase),
            new FrameworkPropertyMetadata(double.NaN));

        public static readonly DependencyProperty LabelHeightProperty = DependencyProperty.Register(
            nameof(LabelHeight), typeof(double), typeof(CoordinatePickerBase),
            new FrameworkPropertyMetadata(double.NaN));

        public static readonly DependencyProperty UpDownWidthProperty = DependencyProperty.Register(
            nameof(UpDownWidth), typeof(double), typeof(CoordinatePickerBase),
            new FrameworkPropertyMetadata(double.NaN));

        public static readonly DependencyProperty UpDownHeightProperty = DependencyProperty.Register(
            nameof(UpDownHeight), typeof(double), typeof(CoordinatePickerBase),
            new FrameworkPropertyMetadata(double.NaN));

        public Thickness LabelMargin
        {
            get { return (Thickness) GetValue(LabelMarginProperty); }
            set { SetValue(LabelMarginProperty, value); }
        }

        public Thickness UpDownMargin
        {
            get { return (Thickness) GetValue(UpDownMarginProperty); }
            set { SetValue(UpDownMarginProperty, value); }
        }

        public string FormatString
        {
            get { return (string) GetValue(FormatStringProperty); }
            set { SetValue(FormatStringProperty, value); }
        }

        public bool IsLabelVisible
        {
            get { return (bool) GetValue(IsLabelVisibleProperty); }
            set { SetValue(IsLabelVisibleProperty, value); }
        }

        public double LabelWidth
        {
            get { return (double) GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        public double LabelHeight
        {
            get { return (double) GetValue(LabelHeightProperty); }
            set { SetValue(LabelHeightProperty, value); }
        }

        public double UpDownWidth
        {
            get { return (double) GetValue(UpDownWidthProperty); }
            set { SetValue(UpDownWidthProperty, value); }
        }

        public double UpDownHeight
        {
            get { return (double) GetValue(UpDownHeightProperty); }
            set { SetValue(LabelHeightProperty, value); }
        }
    }
}
