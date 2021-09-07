namespace ToolUI.Test
{
    public class ExtrasVM : TabPageVM
    {
        private double m_value1;
        private double m_value2;
        private double m_value3;
        private bool m_boolValue1;
        private bool m_boolValue2;
        private bool m_boolValue3;

        public double Value1
        {
            get { return m_value1; }
            set { m_value1 = value; OnPropertyChanged(); }
        }

        public double Value2
        {
            get { return m_value2; }
            set { m_value2 = value; OnPropertyChanged(); }
        }

        public double Value3
        {
            get { return m_value3; }
            set { m_value3 = value; OnPropertyChanged(); }
        }

        public bool BoolValue1
        {
            get { return m_boolValue1; }
            set { m_boolValue1 = value; OnPropertyChanged(); }
        }

        public bool BoolValue2
        {
            get { return m_boolValue2; }
            set { m_boolValue2 = value; OnPropertyChanged(); }
        }

        public bool BoolValue3
        {
            get { return m_boolValue3; }
            set { m_boolValue3 = value; OnPropertyChanged(); }
        }
    }
}
