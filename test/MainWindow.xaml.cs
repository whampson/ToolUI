using System.ComponentModel;
using System.Windows.Controls;
using WHampson.ToolUI;

namespace ToolUI.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        public new MainVM ViewModel
        {
            get { return (MainVM) DataContext; }
            set { DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.Initialized)
            {
                foreach (var item in e.AddedItems)
                {
                    if (item is TabPageVM page)
                    {
                        page.Update();
                    }
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.Initialized)
            {
                ViewModel.SwitchTabSets();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ViewModel.PromptYesNo(
                "Are you sure you want to quit?",
                "Exit Application",
                noAction: () => e.Cancel = true);
        }
    }
}
