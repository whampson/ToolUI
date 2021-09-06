using System.Numerics;
using System.Windows;
using System.Windows.Data;

namespace WHampson.ToolUI.Controls
{
    /// <summary>
    /// Interaction logic for CoordinatePicker3D.xaml
    /// </summary>
    public partial class CoordinatePicker3D : CoordinatePickerBase
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(Vector3?), typeof(CoordinatePicker3D),
            new FrameworkPropertyMetadata(default(Vector3?),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged));

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ValueChanged), RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<Vector3?>), typeof(CoordinatePicker3D));

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            nameof(X), typeof(float), typeof(CoordinatePicker3D));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            nameof(Y), typeof(float), typeof(CoordinatePicker3D));

        public static readonly DependencyProperty ZProperty = DependencyProperty.Register(
            nameof(Z), typeof(float), typeof(CoordinatePicker3D));

        public static readonly DependencyProperty XLabelProperty = DependencyProperty.Register(
            nameof(XLabel), typeof(string), typeof(CoordinatePicker3D),
            new FrameworkPropertyMetadata("X:"));

        public static readonly DependencyProperty YLabelProperty = DependencyProperty.Register(
            nameof(YLabel), typeof(string), typeof(CoordinatePicker3D),
            new FrameworkPropertyMetadata("Y:"));

        public static readonly DependencyProperty ZLabelProperty = DependencyProperty.Register(
            nameof(ZLabel), typeof(string), typeof(CoordinatePicker3D),
            new FrameworkPropertyMetadata("Z:"));

        public event RoutedPropertyChangedEventHandler<Vector3?> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public Vector3? Value
        {
            get { return (Vector3?) GetValue(ValueProperty); }
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

        public float Z
        {
            get { return (float) GetValue(ZProperty); }
            set { SetValue(ZProperty, value); }
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

        public string ZLabel
        {
            get { return (string) GetValue(ZLabelProperty); }
            set { SetValue(ZLabelProperty, value); }
        }

        private bool m_suppressValueChanged;
        private bool m_suppressComponentsChanged;

        public CoordinatePicker3D()
        {
            InitializeComponent();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CoordinatePicker3D obj)
            {
                obj.OnValueChanged((Vector3?) e.OldValue, (Vector3?) e.NewValue);
            }
        }

        private void OnValueChanged(Vector3? oldValue, Vector3? newValue)
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
                    Z = newValue.Value.Z;
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
                    Value = new Vector3(newValue, Y, Z);
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
                    Value = new Vector3(X, newValue, Z);
                }
                finally
                {
                    m_suppressValueChanged = false;
                }
            }
        }

        private void ZComponent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!m_suppressComponentsChanged
                && e.OldValue is float oldValue
                && e.NewValue is float newValue
                && oldValue != newValue)
            {
                m_suppressValueChanged = true;
                try
                {
                    Value = new Vector3(X, Y, newValue);
                }
                finally
                {
                    m_suppressValueChanged = false;
                }
            }
        }
    }
}
