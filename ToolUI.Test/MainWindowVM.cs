using System;
using System.IO;
using System.Numerics;
using System.Windows.Input;
using Bogus;
using WHampson.ToolUI.ViewModels;
using WpfEssentials.Win32;

namespace ToolUI.Test
{
    public class MainWindowVM : WindowBaseVM
    {
        private Vector2 m_testVector2;
        private Vector3 m_testVector3;
        private string m_status;
        private string m_timedStatus;
        private double m_statusDuration;

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

        public string Status
        {
            get { return m_status; }
            set { m_status = value; OnPropertyChanged(); }
        }

        public string TimedStatus
        {
            get { return m_timedStatus; }
            set { m_timedStatus = value; OnPropertyChanged(); }
        }

        public double StatusDuration
        {
            get { return m_statusDuration; }
            set { m_statusDuration = value; OnPropertyChanged(); }
        }

        public MainWindowVM()
        {
            Status = "Ready.";
            TimedStatus = "She sells sea shells by the sea shore.";
        }

        public override void Init()
        {
            base.Init();
            StatusDuration = 5;
            SetStatusText(Status);
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

        public ICommand MessageBoxBigTextCommand => new RelayCommand
        (
            () => ShowInfo(new Faker().Lorem.Paragraphs(5), "Big Text")
        );

        public ICommand SetStatusCommand => new RelayCommand
        (
            () => SetStatusText(Status)
        );

        public ICommand SetTimedStatusCommand => new RelayCommand
        (
            () => SetTimedStatusText(TimedStatus, StatusDuration)
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
