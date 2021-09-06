using System.Numerics;
using System.Windows;
using System.Windows.Data;

namespace WHampson.ToolUI.Controls
{
    /// <summary>
    /// Interaction logic for CoordinatePicker2D.xaml
    /// </summary>
    public partial class CoordinatePicker2D : CoordinatePickerBase
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(Vector2?), typeof(CoordinatePicker2D),
            new FrameworkPropertyMetadata(default(Vector2?),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged));

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ValueChanged), RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<Vector2?>),
            typeof(CoordinatePicker2D));

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            nameof(X), typeof(float), typeof(CoordinatePicker2D));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            nameof(Y), typeof(float), typeof(CoordinatePicker2D));

        public static readonly DependencyProperty XLabelProperty = DependencyProperty.Register(
            nameof(XLabel), typeof(string), typeof(CoordinatePicker2D),
            new FrameworkPropertyMetadata("X:"));

        public static readonly DependencyProperty YLabelProperty = DependencyProperty.Register(
            nameof(YLabel), typeof(string), typeof(CoordinatePicker2D),
            new FrameworkPropertyMetadata("Y:"));

        public event RoutedPropertyChangedEventHandler<Vector2> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public Vector2? Value
        {
            get { return (Vector2?) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public float X
        {
            get { return (float) GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public float Y
        {
            get { return (float) GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public string XLabel
        {
            get { return (string) GetValue(XLabelProperty); }
            set { SetValue(XLabelProperty, value); }
        }

        public string YLabel
        {
            get { return (string) GetValue(YLabelProperty); }
            set { SetValue(YLabelProperty, value); }
        }

        private bool m_suppressValueChanged;
        private bool m_suppressComponentsChanged;

        public CoordinatePicker2D()
        {
            InitializeComponent();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CoordinatePicker2D obj)
            {
                obj.OnValueChanged((Vector2?) e.OldValue, (Vector2?) e.NewValue);
            }
        }

        private void OnValueChanged(Vector2? oldValue, Vector2? newValue)
        {
            if (!m_suppressValueChanged
                && oldValue != newValue
                && newValue.HasValue)
            {
                m_suppressComponentsChanged = true;
                try
                {
                    X = newValue.Value.X;
                    Y = newValue.Value.Y;
                }
                finally
                {
                    m_suppressComponentsChanged = false;
                }
            }
        }

        private static object OnCoerceValue(DependencyObject d, object value)
        {
            return value;
        }

        private void XComponent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!m_suppressComponentsChanged
                && e.OldValue is float oldValue
                && e.NewValue is float newValue
                && oldValue != newValue)
            {
                m_suppressValueChanged = true;
                try
                {
                    Value = new Vector2(newValue, Y);
                }
                finally
                {
                    m_suppressValueChanged = false;
                }
            }
        }

        private void YComponent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!m_suppressComponentsChanged
                && e.OldValue is float oldValue
                && e.NewValue is float newValue
                && oldValue != newValue)
            {
                m_suppressValueChanged = true;
                try
                {
                    Value = new Vector2(X, newValue);
                }
                finally
                {
                    m_suppressValueChanged = false;
                }
            }
        }
    }
}
