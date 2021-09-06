using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace WHampson.ToolUI
{
    /// <summary>
    /// Base class for UserControls.
    /// </summary>
    public class UserControlBase : UserControl
    {
        private bool m_lazyInitialize;

        /// <summary>
        /// Gets or sets the UserControl's view model.
        /// </summary>
        public ViewModelBase ViewModel
        {
            get { return (ViewModelBase) DataContext; }
            set { DataContext = value; }
        }

        private void InitializeHandlers()
        {
            Debug.Assert(ViewModel != null, "ViewModel is null!");

            ViewModel.Initializing += ViewModel_Initializing;
            ViewModel.ShuttingDown += ViewModel_ShuttingDown;
            ViewModel.Loading += ViewModel_Loading;
            ViewModel.Unloading += ViewModel_Unloading;
            ViewModel.Updating += ViewModel_Updating;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (ViewModel != null)
            {
                InitializeHandlers();
                m_lazyInitialize = false;
            }
            else
            {
                m_lazyInitialize = true;
            }

            Loaded += (o, e) =>
            {
                if (m_lazyInitialize)
                {
                    InitializeHandlers();
                    OnInitialize();
                    OnLoad();
                    m_lazyInitialize = false;
                }
            };
        }

        private void ViewModel_Loading(object sender, EventArgs e) => OnLoad();
        private void ViewModel_Unloading(object sender, EventArgs e) => OnUnload();
        private void ViewModel_Updating(object sender, EventArgs e) => OnUpdate();
        private void ViewModel_Initializing(object sender, EventArgs e) => OnInitialize();
        private void ViewModel_ShuttingDown(object sender, EventArgs e)
        {
            OnShutdown();
            ViewModel.Initializing -= ViewModel_Initializing;
            ViewModel.ShuttingDown -= ViewModel_ShuttingDown;
            ViewModel.Loading -= ViewModel_Loading;
            ViewModel.Unloading -= ViewModel_Unloading;
            ViewModel.Updating -= ViewModel_Updating;
        }

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

        /// <summary>
        /// Called when the <see cref="ViewModelBase.Initializing"/> event is fired.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when the <see cref="ViewModelBase.ShuttingDown"/> event is fired.
        /// </summary>
        protected virtual void OnShutdown() { }
    }
}
