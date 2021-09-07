using WpfEssentials;
using System;

namespace WHampson.ToolUI
{
    /// <summary>
    /// A view-model base class.
    /// </summary>
    /// 
    /// <remarks>
    /// A "view-model" is the layer that sits between the "model" and the "view" in
    /// the Model-View-ViewModel (MVVM) pattern. A view-model contains properties and
    /// commands that the view can bind to, and can provide "glue logic" that transforms
    /// data from the model into a format that can be represented by the view.
    /// 
    /// The framework tends to be "bottom-up" in that actions and events are triggered by
    /// the model or view-model and propagate upwards to be consumed by the view. This means
    /// that it is typically the model's or view-model's responsibility to inform the view of
    /// when data has changed. As such, this view-model base class derives from <see cref="ObservableObject"/>
    /// which provides a means of informing the view of changes made to the underlying data
    /// in the form of the <see cref="ObservableObject.PropertyChanged"/> event and the
    /// <see cref="ObservableObject.OnPropertyChanged(string)"/> event trigger.
    /// 
    /// For more information on the MVVM pattern, see
    /// <see href="https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm"/>.
    /// </remarks>
    public abstract class ViewModelBase : ObservableObject
    {
        private int m_initCount;
        private int m_shutdownCount;
        private int m_loadCount;
        private int m_unloadCount;
        private int m_updateCount;

        private bool m_isInitializing;
        private bool m_isShuttingDown;
        private bool m_isLoading;
        private bool m_isUnloading;
        private bool m_isUpdating;

        /// <summary>
        /// Fired when <see cref="Init"/> is called.
        /// </summary>
        public EventHandler Initializing;

        /// <summary>
        /// Fired when <see cref="Shutdown"/> is called.
        /// </summary>
        public EventHandler ShuttingDown;

        /// <summary>
        /// Fired when <see cref="Load"/> is called.
        /// </summary>
        public EventHandler Loading;

        /// <summary>
        /// Fired when <see cref="Unload"/> is called.
        /// </summary>
        public EventHandler Unloading;

        /// <summary>
        /// Fired when <see cref="Update"/> is called.
        /// </summary>
        public EventHandler Updating;

        /// <summary>
        /// Gets the total number of times the <see cref="Initializing"/> event was fired.
        /// </summary>
        public int InitCount
        {
            get { return m_initCount; }
            private set { m_initCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the total number of times the <see cref="ShuttingDown"/> event was fired.
        /// </summary>
        public int ShutdownCount
        {
            get { return m_shutdownCount; }
            private set { m_shutdownCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the total number of times the <see cref="Loading"/> event was fired.
        /// </summary>
        public int LoadCount
        {
            get { return m_loadCount; }
            private set { m_loadCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the total number of times the <see cref="Unloading"/> event was fired.
        /// </summary>
        public int UnloadCount
        {
            get { return m_unloadCount; }
            private set { m_unloadCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the total number of times the <see cref="Updating"/> event was fired.
        /// </summary>
        public int UpdateCount
        {
            get { return m_updateCount; }
            private set { m_updateCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets a value indicating whether view model is initializing.
        /// </summary>
        public bool IsInitializing
        { 
            get { return m_isInitializing; }
            private set { m_isInitializing = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets a value indicating whether view model is shutting down.
        /// </summary>
        public bool IsShuttingDown
        { 
            get { return m_isShuttingDown; }
            private set { m_isShuttingDown = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets a value indicating whether view model is loading.
        /// </summary>
        public bool IsLoading
        { 
            get { return m_isLoading; }
            private set { m_isLoading = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets a value indicating whether view model is unloading.
        /// </summary>
        public bool IsUnloading
        { 
            get { return m_isUnloading; }
            private set { m_isUnloading = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets a value indicating whether view model is updating.
        /// </summary>
        public bool IsUpdating
        { 
            get { return m_isUpdating; }
            private set { m_isUpdating = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Initializes the view model and fires the <see cref="Initializing"/> event.
        /// </summary>
        /// <remarks>
        /// Perform lifetime initialization tasks here (e.g. allocating global memory).
        /// </remarks>
        public virtual void Init()
        {
            IsInitializing = true;
            
            InitCount++;
            Initializing?.Invoke(this, EventArgs.Empty);

            IsInitializing = false;
        }

        /// <summary>
        /// Uninitializes the view model and fires the <see cref="ShuttingDown"/> event.
        /// </summary>
        /// <remarks>
        /// Perform lifetime cleanup tasks here (e.g. freeing global memory).
        /// </remarks>
        public virtual void Shutdown()
        {
            IsShuttingDown = true;

            ShutdownCount++;
            ShuttingDown?.Invoke(this, EventArgs.Empty);

            IsShuttingDown = false;
        }

        /// <summary>
        /// Loads the view model and fires the <see cref="Loading"/> event.
        /// </summary>
        /// <remarks>
        /// Perform transient initialization tasks here (e.g. re-loading member variables when a new file is opened).
        /// </remarks>
        public virtual void Load()
        {
            IsLoading = true;

            LoadCount++;
            Loading?.Invoke(this, EventArgs.Empty);

            IsLoading = false;
        }

        /// <summary>
        /// Unloads the view model.
        /// </summary>
        /// /// <remarks>
        /// Perform transient cleanup tasks here (e.g. cleaning up when a file is closed).
        /// </remarks>
        public virtual void Unload()
        {
            IsUnloading = true;

            UnloadCount++;
            Unloading?.Invoke(this, EventArgs.Empty);

            IsUnloading = false;
        }

        /// <summary>
        /// Updates the view model state.
        /// </summary>
        /// <remarks>
        /// Perform refresh tasks here.
        /// </remarks>
        public virtual void Update()
        {
            IsUpdating = true;

            UpdateCount++;
            Updating?.Invoke(this, EventArgs.Empty);

            IsUpdating = false;
        }
    }
}
