using WHampson.ToolUI.Events;

namespace WHampson.ToolUI
{
    /// <summary>
    /// Base class for dialog boxes.
    /// </summary>
    public class DialogBase : WindowBase
    {
        public new DialogVM ViewModel
        {
            get { return (DialogVM) DataContext; }
            set { DataContext = value; }
        }

        public DialogBase()
        { }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ViewModel.CloseRequest += ViewModel_CloseRequest;
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
            ViewModel.CloseRequest -= ViewModel_CloseRequest;
        }

        private void ViewModel_CloseRequest(object sender, DialogCloseEventArgs e)
        {
            DialogResult = e.DialogResult;
        }
    }
}
