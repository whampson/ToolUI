using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Ookii.Dialogs.Wpf;
using WpfEssentials.Win32;

namespace WHampson.ToolUI
{
    /// <summary>
    /// A base <see cref="Window"/> class.
    /// </summary>
    /// <remarks>
    /// Windows derived from this class are meant to be paired with view-models derived from
    /// <see cref="WindowViewModelBase"/>.
    /// </remarks>
    public class WindowBase : Window
    {
        /// <summary>
        /// Gets or sets the window's view model.
        /// </summary>
        public WindowViewModelBase ViewModel
        {
            get { return (WindowViewModelBase) DataContext; }
            set { DataContext = value; }
        }

        /// <summary>
        /// Hide the window instead of closing it when <see cref="Window.Close"/> is called.
        /// </summary>
        public bool HideOnClose { get; set; }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.Initializing"/> event is fired.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.ShuttingDown"/> event is fired.
        /// </summary>
        protected virtual void OnShutdown() { }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.Loading"/> event is fired.
        /// </summary>
        protected virtual void OnLoad() { }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.Unloading"/> event is fired.
        /// </summary>
        protected virtual void OnUnload() { }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.Updating"/> event is fired.
        /// </summary>
        protected virtual void OnUpdate() { }
        
        protected override void OnInitialized(EventArgs e)
        {
            Debug.Assert(ViewModel != null, "ViewModel cannot be null!");

            base.OnInitialized(e);

            Loaded += Window_Loaded;
            Unloaded += Window_Unloaded;
            ViewModel.Initializing += ViewModel_Initializing;
            ViewModel.ShuttingDown += ViewModel_ShuttingDown;
            ViewModel.Loading += ViewModel_Loading;
            ViewModel.Unloading += ViewModel_Unloading;
            ViewModel.Updating += ViewModel_Updating;
            ViewModel.ShowMessageBoxRequest += ViewModel_MessageBoxRequest;
            ViewModel.ShowFileDialogRequest += ViewModel_FileDialogRequest;
            ViewModel.ShowFolderDialogRequest += ViewModel_FolderDialogRequest;
            ViewModel.WindowHideRequest += ViewModel_HideRequest;
            ViewModel.WindowCloseRequest += ViewModel_CloseRequest;

            ViewModel.Init();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Debug.Assert(ViewModel != null, "ViewModel cannot be null!");

            if (HideOnClose)
            {
                e.Cancel = true;
                Hide();
            }

            base.OnClosing(e);

            if (!e.Cancel)
            {
                ViewModel.Shutdown();

                Loaded -= Window_Loaded;
                Unloaded -= Window_Unloaded;
                ViewModel.Initializing -= ViewModel_Initializing;
                ViewModel.ShuttingDown -= ViewModel_ShuttingDown;
                ViewModel.Loading -= ViewModel_Loading;
                ViewModel.Unloading -= ViewModel_Unloading;
                ViewModel.Updating -= ViewModel_Updating;
                ViewModel.ShowMessageBoxRequest -= ViewModel_MessageBoxRequest;
                ViewModel.ShowFileDialogRequest -= ViewModel_FileDialogRequest;
                ViewModel.ShowFolderDialogRequest -= ViewModel_FolderDialogRequest;
                ViewModel.WindowHideRequest -= ViewModel_HideRequest;
                ViewModel.WindowCloseRequest -= ViewModel_CloseRequest;
            }
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            ViewModel.ContentRendered();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ViewModel.Activated();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            ViewModel.Deactivated();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => ViewModel.Load();
        private void Window_Unloaded(object sender, RoutedEventArgs e) => ViewModel.Unload();
        
        private void ViewModel_Initializing(object sender, EventArgs e) => OnInitialize();
        private void ViewModel_ShuttingDown(object sender, EventArgs e) => OnShutdown();
        private void ViewModel_Loading(object sender, EventArgs e) => OnLoad();
        private void ViewModel_Unloading(object sender, EventArgs e) => OnUnload();
        private void ViewModel_Updating(object sender, EventArgs e) => OnUpdate();
        private void ViewModel_HideRequest(object sender, EventArgs e) => Hide();
        private void ViewModel_CloseRequest(object sender, bool? e) => Close();

        private void ViewModel_MessageBoxRequest(object sender, MessageBoxEventArgs e)
        {
            MessageBoxEx msgBox = new MessageBoxEx() { Owner = this };
            MessageBoxExViewModel vm = msgBox.ViewModel as MessageBoxExViewModel;
            vm.Title = e.Title;
            vm.Text = e.Text;
            vm.Buttons = e.Buttons;
            vm.Icon = e.Icon;
            _ = msgBox.ShowDialog();    // result set automatically in view model's 'Result' property
            e.Callback?.Invoke(vm.Result);
        }

        private void ViewModel_FileDialogRequest(object sender, FileDialogEventArgs e)
        {
            e.ShowDialog(this);         // result params are set and callback is invoked automatically when dialog closed
        }

        private void ViewModel_FolderDialogRequest(object sender, FileDialogEventArgs e)
        {
            VistaFolderBrowserDialog d = new VistaFolderBrowserDialog();
            bool? r = d.ShowDialog(this);
            e.FileName = d.SelectedPath;
            e.Callback?.Invoke(r, e);
        }
    }
}
