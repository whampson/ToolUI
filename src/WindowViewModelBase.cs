using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfEssentials.Win32;

namespace WHampson.ToolUI
{
    /// <summary>
    /// Base class for window view models.
    /// </summary>
    public abstract class WindowViewModelBase : ViewModelBase
    {
        public event EventHandler<MessageBoxEventArgs> ShowMessageBoxRequest;
        public event EventHandler<FileDialogEventArgs> ShowFileDialogRequest;
        public event EventHandler<FileDialogEventArgs> ShowFolderDialogRequest;
        public event EventHandler<bool?> WindowCloseRequest;
        public event EventHandler WindowHideRequest;

        private readonly DispatcherTimer m_statusTimer;
        private readonly Stopwatch m_statusStopwatch;
        private long m_timerDuration;
        private string m_statusText;
        private string m_defaultStatusText;
        private string m_title;

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public string Title
        {
            get { return m_title; }
            set { m_title = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the status text.
        /// </summary>
        public string StatusText
        {
            get { return m_statusText; }
            private set { m_statusText = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Creates a new <see cref="WindowViewModelBase"/> instance.
        /// </summary>
        public WindowViewModelBase()
        {
            m_statusTimer = new DispatcherTimer();
            m_statusStopwatch = new Stopwatch();
        }

        /// <summary>
        /// This method is invoked when the window's <see cref="Window.Activated"/>
        /// event is fired.
        /// </summary>
        public virtual void Activated()
        { }

        /// <summary>
        /// This method is invoked when the window's <see cref="Window.Deactivated"/>
        /// event is fired.
        /// </summary>
        public virtual void Deactivated()
        { }

        /// <summary>
        /// This method is invoked when the window's <see cref="Window.ContentRendered"/>
        /// event is fired.
        /// </summary>
        public virtual void ContentRendered()
        { }

        public override void Init()
        {
            base.Init();
            m_statusTimer.Tick += StatusTimer_Tick;
        }

        public override void Shutdown()
        {
            base.Shutdown();
            m_statusTimer.Tick -= StatusTimer_Tick;
        }

        /// <summary>
        /// Sets the status text.
        /// </summary>
        /// <param name="status">
        /// The new status text.
        /// </param>
        public void SetStatusText(string status)
        {
            if (m_statusTimer.IsEnabled)
            {
                m_statusTimer.Stop();
            }

            StatusText = status;
            m_defaultStatusText = status;
        }

        /// <summary>
        /// Sets the status text for a period of time.
        /// </summary>
        /// <param name="status">
        /// The new temporary status text.
        /// </param>
        /// <param name="duration">
        /// The time in seconds to display the temporary status text.
        /// </param>
        /// <param name="expiredStatus">
        /// The text to show after the temporary status text expires.
        /// If <c>null</c>, the status text will be restored to the
        /// previous value.
        /// </param>
        public void SetTimedStatusText(string status,
            double duration = 2.5,
            string expiredStatus = null)
        {
            if (duration < 0) throw new ArgumentOutOfRangeException(nameof(duration));
            if (expiredStatus == null) expiredStatus = m_defaultStatusText;

            if (m_statusTimer.IsEnabled)
            {
                m_statusTimer.Stop();
                m_statusText = expiredStatus;
            }

            m_defaultStatusText = expiredStatus;
            StatusText = status;
            m_timerDuration = (int) (duration * 1000);
            m_statusTimer.Interval = TimeSpan.FromMilliseconds(1);

            m_statusStopwatch.Reset();
            m_statusTimer.Start();
            m_statusStopwatch.Start();
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (m_statusStopwatch.ElapsedMilliseconds >= m_timerDuration)
            {
                m_statusTimer.Stop();
                StatusText = m_defaultStatusText;
            }
        }

        public void ShowInfo(string text, string title = "Information")
        {
            ShowMessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text, title, icon: MessageBoxImage.Information));
        }

        public void ShowWarning(string text, string title = "Warning")
        {
            ShowMessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text, title, icon: MessageBoxImage.Warning));
        }

        public void ShowError(string text, string title = "Error")
        {
            ShowMessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text, title, icon: MessageBoxImage.Error));
        }

        public void ShowException(Exception e, string text = "An error has occurred.", string title = "Error")
        {
            ShowError(text + $"\n\n{e.GetType().Name}: {e.Message}", title);
        }

        public void PromptOkCancel(string text, string title = "Confirm?",
            MessageBoxImage icon = MessageBoxImage.Question,
            Action okAction = null, Action cancelAction = null)
        {
            ShowMessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text)
            {
                Title = title,
                Icon = icon,
                Buttons = MessageBoxButton.OKCancel,
                Callback = (r) =>
                {
                    switch (r)
                    {
                        case MessageBoxResult.OK:
                            okAction?.Invoke();
                            break;
                        case MessageBoxResult.Cancel:
                            cancelAction?.Invoke();
                            break;
                    }
                }
            });
        }

        public void PromptYesNo(string text, string title = "Yes or No?",
            MessageBoxImage icon = MessageBoxImage.Question,
            Action yesAction = null, Action noAction = null)
        {
            ShowMessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text)
            {
                Title = title,
                Icon = icon,
                Buttons = MessageBoxButton.YesNo,
                Callback = (r) =>
                {
                    switch (r)
                    {
                        case MessageBoxResult.Yes:
                            yesAction?.Invoke();
                            break;
                        case MessageBoxResult.No:
                            noAction?.Invoke();
                            break;
                    }
                }
            });
        }

        public void PromptYesNoCancel(string text, string title = "Yes or No?",
            MessageBoxImage icon = MessageBoxImage.Question,
            Action yesAction = null, Action noAction = null, Action cancelAction = null)
        {
            ShowMessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text)
            {
                Title = title,
                Icon = icon,
                Buttons = MessageBoxButton.YesNoCancel,
                Callback = (r) =>
                {
                    switch (r)
                    {
                        case MessageBoxResult.Yes:
                            yesAction?.Invoke();
                            break;
                        case MessageBoxResult.No:
                            noAction?.Invoke();
                            break;
                        case MessageBoxResult.Cancel:
                            cancelAction?.Invoke();
                            break;
                    }
                }
            });
        }

        public void ShowMessageBox(MessageBoxEventArgs e)
        {
            ShowMessageBoxRequest?.Invoke(this, e);
        }

        public void ShowFileDialog(FileDialogType type, Action<bool?, FileDialogEventArgs> callback = null)
        {
            ShowFileDialog(new FileDialogEventArgs(type, callback)
            {
                Filter = "All Files|*.*"
            });
        }

        public void ShowFileDialog(FileDialogEventArgs e)
        {
            ShowFileDialogRequest?.Invoke(this, e);
        }

        public void ShowFolderDialog(Action<bool?, FileDialogEventArgs> callback = null)
        {
            var type = FileDialogType.OpenFileDialog;   // irrelevant
            ShowFolderDialog(new FileDialogEventArgs(type, callback));
        }

        public void ShowFolderDialog(FileDialogEventArgs e)
        {
            ShowFolderDialogRequest?.Invoke(this, e);
        }

        public void Close(bool? result = null)
        {
            WindowCloseRequest?.Invoke(this, result);
        }

        public ICommand HideCommand => new RelayCommand
        (
            () => WindowHideRequest?.Invoke(this, EventArgs.Empty)
        );

        public ICommand CloseWithResultCommand => new RelayCommand<bool?>
        (
            (r) => Close(r)
        );

        public ICommand CloseCommand => new RelayCommand
        (
            () => Close(null)
        );

        public ICommand ConfirmCommand => new RelayCommand
        (
            () => Close(true)
        );

        public ICommand CancelCommand => new RelayCommand
        (
            () => Close(false)
        );
    }
}
