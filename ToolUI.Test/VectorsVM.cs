using System.Numerics;

namespace ToolUI.Test
{
    public class VectorsVM : TabPageVM
    {
        private Vector2 m_testVector2;
        private Vector3 m_testVector3;

        public Vector2 TestVector2
        {
            get { return m_testVector2; }
            set { m_testVector2 = value; OnPropertyChanged(); }
        }

        public Vector3 TestVector3
        {
            get { return m_testVector3; }
            set { m_testVector3 = value; OnPropertyChanged(); }
        }
    }
}
