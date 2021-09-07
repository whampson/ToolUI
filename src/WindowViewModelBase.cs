using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfEssentials.Win32;

namespace WHampson.ToolUI
{
    /// <summary>
    /// A view-model base class for use with <see cref="Window"/> views.
    /// </summary>
    /// 
    /// <remarks>
    /// View-models derived from this base class should be paired with
    /// views derived from <see cref="WindowBase"/>. With this pairing,
    /// certain functions will be called when the window's corresponding
    /// events are fired:
    /// <see cref="ViewModelBase.Init"/> is called when <see cref="FrameworkElement.Initialized"/> is fired,
    /// <see cref="ViewModelBase.Shutdown"/> is called when <see cref="Window.Closing"/> is fired (and not set to cancel),
    /// <see cref="ViewModelBase.Load"/> is called when <see cref="FrameworkElement.Loaded"/> is fired,
    /// <see cref="ViewModelBase.Unload"/> is called when <see cref="FrameworkElement.Unloaded"/> is fired,
    /// <see cref="Activated"/> is called when <see cref="Window.Activated"/> is fired,
    /// <see cref="Deactivated"/> is called when <see cref="Window.Deactivated"/> is fired,
    /// <see cref="ContentRendered"/> is called when <see cref="Window.ContentRendered"/> is fired.
    /// </remarks>
    public abstract class WindowViewModelBase : ViewModelBase
    {
        private readonly DispatcherTimer m_statusTimer;
        private readonly Stopwatch m_statusStopwatch;
        private long m_timerDuration;
        private string m_statusText;
        private string m_defaultStatusText;
        private string m_title;

        /// <summary>
        /// Fired when the view model requests the window to show a message box.
        /// </summary>
        /// <remarks>
        /// The window associated with this view model must handle this request
        /// in order for message box functionality to be supported.
        /// </remarks>
        public event EventHandler<MessageBoxEventArgs> ShowMessageBoxRequest;

        /// <summary>
        /// Fired when the view model requests the window to show a file selection dialog.
        /// </summary>
        /// <remarks>
        /// The window associated with this view model must handle this request
        /// in order for file selection dialog functionality to be supported.
        /// </remarks>
        public event EventHandler<FileDialogEventArgs> ShowFileDialogRequest;

        /// <summary>
        /// Fired when the view model requests the window to show a file selection dialog.
        /// </summary>
        /// <remarks>
        /// The window associated with this view model must handle this request
        /// in order for folder selection dialog functionality to be supported.
        /// </remarks>
        public event EventHandler<FileDialogEventArgs> ShowFolderDialogRequest;

        /// <summary>
        /// Fired when the view model requests the window to close.
        /// </summary>
        /// <remarks>
        /// The window associated with this view model must handle this request
        /// in order for the window to be closed progammatically by the view model.
        /// </remarks>
        public event EventHandler<bool?> WindowCloseRequest;

        /// <summary>
        /// Fired when the view model requests the window to hide.
        /// </summary>
        /// <remarks>
        /// The window associated with this view model must handle this request
        /// in order for the window to be hidden progammatically by the view model.
        /// </remarks>
        public event EventHandler WindowHideRequest;

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
        /// <remarks>
        /// Status text typically shows up in a toolbar at the bottom of the window.
        /// </remarks>
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

        /// <summary>
        /// Requests the window to show an 'Information' message box.
        /// </summary>
        /// <param name="text">Message box text.</param>
        /// <param name="title">Message box title.</param>
        public void ShowInfo(string text, string title = "Information")
        {
            ShowMessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text, title, icon: MessageBoxImage.Information));
        }

        /// <summary>
        /// Requests the window to show a 'Warning' message box.
        /// </summary>
        /// <param name="text">Message box text.</param>
        /// <param name="title">Message box title.</param>
        public void ShowWarning(string text, string title = "Warning")
        {
            ShowMessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text, title, icon: MessageBoxImage.Warning));
        }

        /// <summary>
        /// Requests the window to show an 'Error' message box.
        /// </summary>
        /// <param name="text">Message box text.</param>
        /// <param name="title">Message box title.</param>
        public void ShowError(string text, string title = "Error")
        {
            ShowMessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text, title, icon: MessageBoxImage.Error));
        }

        /// <summary>
        /// Requests the window to show an 'Error' message box with an exception name and message.
        /// </summary>
        /// <param name="text">Message box text to supplement the exception message.</param>
        /// <param name="title">Message box title.</param>
        public void ShowException(Exception e, string text = "An error has occurred.", string title = "Error")
        {
            ShowError(text + $"\n\n{e.GetType().Name}: {e.Message}", title);
        }

        /// <summary>
        /// Requests the window to show a message box promting the user to choose OK or Cancel.
        /// </summary>
        /// <param name="text">Message box text.</param>
        /// <param name="title">Message box title.</param>
        /// <param name="icon">Message box icon.</param>
        /// <param name="okAction">Callback for when 'OK' is selected.</param>
        /// <param name="cancelAction">Callback for when 'Cancel' is selected.</param>
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

        /// <summary>
        /// Requests the window to show a message box promting the user to choose Yes or No.
        /// </summary>
        /// <param name="text">Message box text.</param>
        /// <param name="title">Message box title.</param>
        /// <param name="icon">Message box icon.</param>
        /// <param name="yesAction">Callback for when 'Yes' is selected.</param>
        /// <param name="noAction">Callback for when 'No' is selected.</param>
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

        /// <summary>
        /// Requests the window to show a message box promting the user to choose Yes, No, or Cancel.
        /// </summary>
        /// <param name="text">Message box text.</param>
        /// <param name="title">Message box title.</param>
        /// <param name="icon">Message box icon.</param>
        /// <param name="yesAction">Callback for when 'Yes' is selected.</param>
        /// <param name="noAction">Callback for when 'No' is selected.</param>
        /// <param name="cancelAction">Callback for when 'Cancel' is selected.</param>
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

        /// <summary>
        /// Requests the window to show a generic message box.
        /// </summary>
        /// <param name="e"></param>
        public void ShowMessageBox(MessageBoxEventArgs e)
        {
            ShowMessageBoxRequest?.Invoke(this, e);
        }

        /// <summary>
        /// Requests the window to show a file selection dialog.
        /// </summary>
        /// <param name="type">Dialog type (open or save).</param>
        /// <param name="callback">Action to perform after dialog is closed.</param>
        public void ShowFileDialog(FileDialogType type, Action<bool?, FileDialogEventArgs> callback = null)
        {
            ShowFileDialog(new FileDialogEventArgs(type, callback)
            {
                Filter = "All Files|*.*"
            });
        }

        /// <summary>
        /// Requests the window to show a generic file selection dialog.
        /// </summary>
        /// <param name="e">Dialog arguments.</param>
        public void ShowFileDialog(FileDialogEventArgs e)
        {
            ShowFileDialogRequest?.Invoke(this, e);
        }

        /// <summary>
        /// Requests the window to show a folder selection dialog.
        /// </summary>
        /// <param name="callback">Action to perform after dialog is closed.</param>
        public void ShowFolderDialog(Action<bool?, FileDialogEventArgs> callback = null)
        {
            var type = FileDialogType.OpenFileDialog;   // irrelevant
            ShowFolderDialog(new FileDialogEventArgs(type, callback));
        }

        /// <summary>
        /// Requests the window to show a generic folder selection dialog.
        /// </summary>
        /// <param name="e">Dialog arguments.</param>
        public void ShowFolderDialog(FileDialogEventArgs e)
        {
            ShowFolderDialogRequest?.Invoke(this, e);
        }

        /// <summary>
        /// Requests the window to hide.
        /// </summary>
        public void Hide()
        {
            WindowHideRequest?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Requests the window to close.
        /// </summary>
        /// <param name="result">Window/dialog result.</param>
        public void Close(bool? result = null)
        {
            WindowCloseRequest?.Invoke(this, result);
        }

        /// <summary>
        /// Requests the window to hide.
        /// </summary>
        public ICommand HideCommand => new RelayCommand(() => Hide());

        /// <summary>
        /// Requests the window to close.
        /// </summary>
        public ICommand CloseCommand => new RelayCommand(() => Close(null));

        /// <summary>
        /// Requests the window to close with a result.
        /// </summary>
        public ICommand CloseWithResultCommand => new RelayCommand<bool?>((r) => Close(r));

        /// <summary>
        /// Requests the window to close with a 'true' result.
        /// </summary>
        public ICommand ConfirmCommand => new RelayCommand(() => Close(true));

        /// <summary>
        /// Requests the window to close with a 'false' result.
        /// </summary>
        public ICommand CancelCommand => new RelayCommand(() => Close(false));
    }
}
