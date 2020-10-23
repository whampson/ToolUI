using System;
using System.Windows.Input;
using WpfEssentials.Win32;

namespace WHampson.ToolUI
{
    /// <summary>
    /// Base class for dialog box view models.
    /// </summary>
    public abstract class DialogViewModelBase : WindowViewModelBase
    {
        public new event EventHandler<bool?> CloseRequest;

        public void Close(bool? result = null)
        {
            CloseRequest?.Invoke(this, result);
        }

        public new ICommand CloseCommand => new RelayCommand<bool?>
        (
            (r) => Close(r)
        );

        public ICommand CancelCommand => new RelayCommand
        (
            () => Close(false)
        );

        public ICommand ConfirmCommand => new RelayCommand
        (
            () => Close(true)
        );
    }
}
