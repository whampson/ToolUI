using System;
using System.Windows.Controls;

namespace WHampson.ToolUI
{
    /// <summary>
    /// Base class for UserControls.
    /// </summary>
    public class UserControlBase : UserControl
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the UserControl's view model.
        /// </summary>
        public UserControlViewModelBase ViewModel
        {
            get { return (UserControlViewModelBase) DataContext; }
            set { DataContext = value; }
        }
        #endregion

        #region Virtual View Model Event Handlers
        /// <summary>
        /// Called when the <see cref="ViewModelBase.Initializing"/> event is fired.
        /// </summary>
        protected virtual void OnInitialize()
        { }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.ShuttingDown"/> event is fired.
        /// </summary>
        protected virtual void OnShutdown()
        { }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.Loading"/> event is fired.
        /// </summary>
        protected virtual void OnLoad()
        { }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.Unloading"/> event is fired.
        /// </summary>
        protected virtual void OnUnload()
        { }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.Updating"/> event is fired.
        /// </summary>
        protected virtual void OnUpdate()
        { }
        #endregion

        #region UserControl Event Handlers
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ViewModel.Initializing += ViewModel_Initializing;
            ViewModel.ShuttingDown += ViewModel_ShuttingDown;
            ViewModel.Loading += ViewModel_Loading;
            ViewModel.Unloading += ViewModel_Unloading;
            ViewModel.Updating += ViewModel_Updating;
        }
        #endregion

        #region View Model Event Handlers
        private void ViewModel_Initializing(object sender, EventArgs e)
        {
            OnInitialize();
        }

        private void ViewModel_ShuttingDown(object sender, EventArgs e)
        {
            OnShutdown();
            ViewModel.Initializing -= ViewModel_Initializing;
            ViewModel.ShuttingDown -= ViewModel_ShuttingDown;
            ViewModel.Loading -= ViewModel_Loading;
            ViewModel.Unloading -= ViewModel_Unloading;
            ViewModel.Updating -= ViewModel_Updating;
        }

        private void ViewModel_Loading(object sender, EventArgs e)
        {
            OnLoad();
        }

        private void ViewModel_Unloading(object sender, EventArgs e)
        {
            OnUnload();
        }
        
        private void ViewModel_Updating(object sender, EventArgs e)
        {
            OnUpdate();
        }
        #endregion
    }
}
