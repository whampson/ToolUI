using System.Numerics;
using System.Windows.Input;
using WHampson.ToolUI.ViewModels;
using WpfEssentials.Win32;

namespace ToolUI.Test
{
    public class MainWindowVM : WindowVMBase
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

        public override void Init()
        {
            base.Init();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public ICommand GreetingCommand => new RelayCommand
        (
            () => ShowInfo("Hello, world!", "Hello!")
        );
    }
}
