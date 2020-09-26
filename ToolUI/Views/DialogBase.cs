using System;
using System.ComponentModel;
using WHampson.ToolUI.Events;
using WHampson.ToolUI.ViewModels;

namespace WHampson.ToolUI.Views
{
    public class DialogBase : WindowBase
    {
        public new DialogBaseVM ViewModel
        {
            get { return (DialogBaseVM) DataContext; }
            set { DataContext = value; }
        }

        public DialogBase()
        { }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ViewModel.CloseRequest += ViewModel_CloseRequest;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            ViewModel.CloseRequest -= ViewModel_CloseRequest;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

        private void ViewModel_CloseRequest(object sender, DialogCloseEventArgs e)
        {
            DialogResult = e.DialogResult;
        }
    }
}
