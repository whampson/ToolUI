using System;
using System.Numerics;
using System.Windows.Input;
using WHampson.ToolUI.ViewModels;
using WpfEssentials.Win32;

namespace ToolUI.Test
{
    public class MainWindowVM : WindowBaseVM
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
    }
}
