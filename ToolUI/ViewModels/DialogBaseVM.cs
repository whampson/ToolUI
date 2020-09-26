using System;
using System.Windows.Input;
using WHampson.ToolUI.Events;
using WpfEssentials.Win32;

namespace WHampson.ToolUI.ViewModels
{
    public class DialogBaseVM : WindowBaseVM
    {
        public new event EventHandler<DialogCloseEventArgs> CloseRequest;

        public void Close(bool? result = null)
        {
            CloseRequest?.Invoke(this, new DialogCloseEventArgs(result));
        }

        #region Window Commands
        public new ICommand CloseCommand => new RelayCommand<bool?>
        (
            (r) => CloseRequest?.Invoke(this, new DialogCloseEventArgs(r))
        );

        public ICommand CancelCommand => new RelayCommand
        (
            () => Close(false)
        );

        public ICommand ConfirmCommand => new RelayCommand
        (
            () => Close(true)
        );
        #endregion
    }
}
