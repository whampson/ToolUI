using System;
using System.Windows;
using WpfEssentials;
using WpfEssentials.Win32;

namespace WHampson.ToolUI.ViewModels
{
    public class WindowVMBase : ObservableObject
    {
        private string m_title;

        public event EventHandler<MessageBoxEventArgs> MessageBoxRequest;
        public event EventHandler<FileDialogEventArgs> FileDialogRequest;
        public event EventHandler<FileDialogEventArgs> FolderDialogRequest;

        public string Title
        {
            get { return m_title; }
            set { m_title = value; OnPropertyChanged(); }
        }

        public virtual void Init()
        { }

        public virtual void Shutdown()
        { }

        #region Dialog Box Functions
        public void ShowInfo(string text, string title = "Information")
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text, title, icon: MessageBoxImage.Information));
        }

        public void ShowWarning(string text, string title = "Warning")
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text, title, icon: MessageBoxImage.Warning));
        }

        public void ShowError(string text, string title = "Error")
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text, title, icon: MessageBoxImage.Error));
        }

        public void ShowException(Exception e, string text = "An error has occurred.", string title = "Error")
        {
            ShowError(text + $"\n\n{e.GetType().Name}: {e.Message}", title);
        }

        public void PromptOkCancel(string text, string title = "Confirm?",
            MessageBoxImage icon = MessageBoxImage.Question,
            Action okAction = null, Action cancelAction = null)
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text)
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
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text)
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
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(text)
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
            MessageBoxRequest?.Invoke(this, e);
        }

        public void ShowFileDialog(FileDialogType type, Action<bool?, FileDialogEventArgs> callback = null)
        {
            ShowFileDialog(new FileDialogEventArgs(type, callback));
        }

        public void ShowFileDialog(FileDialogEventArgs e)
        {
            FileDialogRequest?.Invoke(this, e);
        }

        public void ShowFolderDialog(FileDialogType type, Action<bool?, FileDialogEventArgs> callback = null)
        {
            ShowFolderDialog(new FileDialogEventArgs(type, callback));
        }

        public void ShowFolderDialog(FileDialogEventArgs e)
        {
            FolderDialogRequest?.Invoke(this, e);
        }
        #endregion
    }
}
