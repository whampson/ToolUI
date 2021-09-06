using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Input;
using Bogus;
using WHampson.ToolUI;
using WpfEssentials.Win32;

namespace ToolUI.Test
{
    public class MainVM : WindowVM
    {
        private ObservableCollection<TabPageVM> m_tabs;
        private int m_selectedTabIndex;
        private int m_selectedTabSet;
        private bool m_initialized;

        private VectorsVM m_vectorsTab;
        private StatusTextVM m_statusTextTab;
        private ExtrasVM m_extrasTab;

        public ObservableCollection<TabPageVM> Tabs
        {
            get { return m_tabs; }
            set { m_tabs = value; OnPropertyChanged(); }
        }

        public int SelectedTabIndex
        {
            get { return m_selectedTabIndex; }
            set { m_selectedTabIndex = value; OnPropertyChanged(); }
        }

        public int SelectedTabSet
        {
            get { return m_selectedTabSet; }
            set { m_selectedTabSet = value; OnPropertyChanged(); }
        }

        public bool Initialized
        {
            get { return m_initialized; }
            private set { m_initialized = value; OnPropertyChanged(); }
        }

        public MainVM()
        {
            m_vectorsTab = new VectorsVM() { MainWindow = this, Title = "Vectors" };
            m_statusTextTab = new StatusTextVM() { MainWindow = this, Title = "Status Text" };
            m_extrasTab = new ExtrasVM() { MainWindow = this, Title = "Extras" };
            
            Tabs = new ObservableCollection<TabPageVM> { m_vectorsTab, m_statusTextTab, m_extrasTab };
        }

        public override void Init()
        {
            base.Init();

            InitAllTabs();
            Initialized = true;
        }

        public override void Shutdown()
        {
            base.Shutdown();

            ShutdownAllTabs();
        }

        public override void Load()
        {
            base.Load();

            SwitchTabSets();
            UpdateActiveTab();
        }

        public void InitAllTabs()
        {
            foreach (var t in Tabs)
            {
                t.Init();
            }
        }

        public void ShutdownAllTabs()
        {
            foreach (var t in Tabs)
            {
                t.Shutdown();
            }
        }

        public void UpdateActiveTab()
        {
            var activeTab = Tabs.Where(x => x.IsVisible).FirstOrDefault();
            if (activeTab != null)
            {
                activeTab.Update();
            }
        }

        public void SwitchTabSets()
        {
            m_vectorsTab.IsVisible = (SelectedTabSet == 0);
            m_statusTextTab.IsVisible = (SelectedTabSet == 0);
            m_extrasTab.IsVisible = (SelectedTabSet == 1);

            SelectedTabIndex = Tabs.IndexOf(Tabs.Where(x => x.IsVisible).FirstOrDefault());
        }

        public ICommand MessageBoxInfoCommand => new RelayCommand
        (
            () => ShowInfo("This is some info.")
        );

        public ICommand MessageBoxWarningCommand => new RelayCommand
        (
            () => ShowWarning("This a warning.")
        );

        public ICommand MessageBoxErrorCommand => new RelayCommand
        (
            () => ShowError("This an error.")
        );

        public ICommand MessageBoxExceptionCommand => new RelayCommand
        (
            () =>
            {
                try
                {
                    int a = 5;
                    int b = 0;
                    int c = a / b;
                }
                catch (DivideByZeroException ex)
                {
                    ShowException(ex, "This is an exception");
                }
            }
        );

        public ICommand MessageBoxOKCommand => new RelayCommand
        (
            () => ShowInfo("This is a message box with an OK button.")
        );

        public ICommand MessageBoxOKCancelCommand => new RelayCommand
        (
            () => PromptOkCancel("This is a message box with OK and Cancel buttons.",
                okAction: () => ShowInfo("OK selected."),
                cancelAction: () => ShowInfo("Cancel selected."))
        );

        public ICommand MessageBoxYesNoCommand => new RelayCommand
        (
            () => PromptYesNo("This is a message box with Yes and No buttons.",
                yesAction: () => ShowInfo("Yes selected."),
                noAction: () => ShowInfo("No selected."))
        );

        public ICommand MessageBoxYesNoCancelCommand => new RelayCommand
        (
            () => PromptYesNoCancel("This is a message box with Yes, No, and Cancel buttons.",
                yesAction: () => ShowInfo("Yes selected."),
                noAction: () => ShowInfo("No selected."),
                cancelAction: () => ShowInfo("Cancel selected."))
        );

        public ICommand MessageBoxBigTextCommand => new RelayCommand
        (
            () => ShowInfo(new Faker().Lorem.Paragraphs(5), "Big Text")
        );

        public ICommand FileDialogOpenCommand => new RelayCommand
        (
            () =>
            {
                Action<bool?, FileDialogEventArgs> callback = (r, e) =>
                {
                    if (r == true)
                    {
                        ShowInfo($"Selected file: {Path.GetFileName(e.FileName)}");
                    }
                };
                ShowFileDialog(FileDialogType.OpenFileDialog, callback);
            }
        );

        public ICommand FileDialogSaveCommand => new RelayCommand
        (
            () =>
            {
                Action<bool?, FileDialogEventArgs> callback = (r, e) =>
                {
                    if (r == true)
                    {
                        ShowInfo($"Selected file: {Path.GetFileName(e.FileName)}");
                    }
                };
                ShowFileDialog(FileDialogType.SaveFileDialog, callback);
            }
        );

        public ICommand FolderDialogCommand => new RelayCommand
        (
            () =>
            {
                Action<bool?, FileDialogEventArgs> callback = (r, e) =>
                {
                    if (r == true)
                    {
                        ShowInfo($"Selected path: {e.FileName}");
                    }
                };
                ShowFolderDialog(callback);
            }
        );
    }
}
