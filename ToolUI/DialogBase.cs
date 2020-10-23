namespace WHampson.ToolUI
{
    /// <summary>
    /// Base class for dialog boxes.
    /// </summary>
    public class DialogBase : WindowBase
    {
        public new DialogViewModelBase ViewModel
        {
            get { return (DialogViewModelBase) DataContext; }
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

        private void ViewModel_CloseRequest(object sender, bool? e)
        {
            DialogResult = e;
        }
    }
}
