using System.Windows.Controls;

namespace ToolUI.Test
{
    public class TabPageBase<T> : UserControl
        where T : TabPageVM
    {
        public T ViewModel
        {
            get { return (T) DataContext; }
            set { DataContext = value; }
        }
    }
}
