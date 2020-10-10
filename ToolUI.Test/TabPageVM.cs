using WHampson.ToolUI;

namespace ToolUI.Test
{
    public class TabPageVM : UserControlVM
    {
        private string m_title;
        private MainVM m_mainWindow;
        private bool m_isVisible;

        public string Title
        {
            get { return m_title; }
            set { m_title = value; OnPropertyChanged(); }
        }

        public MainVM MainWindow
        {
            get { return m_mainWindow; }
            set { m_mainWindow = value; OnPropertyChanged(); }
        }

        public bool IsVisible
        {
            get { return m_isVisible; }
            set { SetVisibility(value); OnPropertyChanged(); }
        }

        private void SetVisibility(bool visible)
        {
            bool wasVisible = m_isVisible;
            m_isVisible = visible;

            if (wasVisible && !m_isVisible)
            {
                Unload();
            }
            if (m_isVisible && !wasVisible)
            {
                Load();
                //Update();
            }
        }
    }
}
