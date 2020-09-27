using System;
using System.ComponentModel;
using System.Windows;
using Ookii.Dialogs.Wpf;
using WHampson.ToolUI.ViewModels;
using WpfEssentials.Win32;

namespace WHampson.ToolUI.Views
{
    public class WindowBase : Window
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        public WindowBaseVM ViewModel
        {
            get { return (WindowBaseVM) DataContext; }
            set { DataContext = value; }
        }

        /// <summary>
        /// Hide the window instead of closing it when <see cref="Window.Close"/> is called.
        /// </summary>
        public bool HideOnClose { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ViewModel.Init();
            ViewModel.MessageBoxRequest += ViewModel_MessageBoxRequest;
            ViewModel.FileDialogRequest += ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest += ViewModel_FolderDialogRequest;
            ViewModel.HideRequest += ViewModel_HideRequest;
            ViewModel.CloseRequest += ViewModel_CloseRequest;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (HideOnClose)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            base.OnClosing(e);
            ViewModel.Shutdown();
            ViewModel.MessageBoxRequest -= ViewModel_MessageBoxRequest;
            ViewModel.FileDialogRequest -= ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest -= ViewModel_FolderDialogRequest;
            ViewModel.HideRequest -= ViewModel_HideRequest;
            ViewModel.CloseRequest -= ViewModel_CloseRequest;
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
            ViewModel.Dectivated();
        }

        private void ViewModel_MessageBoxRequest(object sender, MessageBoxEventArgs e)
        {
            MessageBoxEx msgBox = new MessageBoxEx() { Owner = this };
            MessageBoxExVM vm = msgBox.ViewModel as MessageBoxExVM;
            vm.Title = e.Title;
            vm.Text = e.Text;
            vm.Buttons = e.Buttons;
            vm.Icon = e.Icon;
            msgBox.ShowDialog();
            e.Callback?.Invoke(vm.Result);
        }
        private void ViewModel_FileDialogRequest(object sender, FileDialogEventArgs e)
        {
            e.ShowDialog(this);
        }

        private void ViewModel_FolderDialogRequest(object sender, FileDialogEventArgs e)
        {
            VistaFolderBrowserDialog d = new VistaFolderBrowserDialog();
            bool? r = d.ShowDialog(this);
            e.FileName = d.SelectedPath;
            e.Callback?.Invoke(r, e);
        }

        private void ViewModel_HideRequest(object sender, EventArgs e)
        {
            Hide();
        }

        private void ViewModel_CloseRequest(object sender, EventArgs e)
        {
            Close();
        }
    }
}
