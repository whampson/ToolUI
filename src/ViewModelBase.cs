using WpfEssentials;
using System;

namespace WHampson.ToolUI
{
    /// <summary>
    /// Base class for all view models.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject
    {
        public EventHandler Initializing;
        public EventHandler ShuttingDown;
        public EventHandler Loading;
        public EventHandler Unloading;
        public EventHandler Updating;

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

        public int InitCount
        {
            get { return m_initCount; }
            private set { m_initCount = value; OnPropertyChanged(); }
        }

        public int ShutdownCount
        {
            get { return m_shutdownCount; }
            private set { m_shutdownCount = value; OnPropertyChanged(); }
        }

        public int LoadCount
        {
            get { return m_loadCount; }
            private set { m_loadCount = value; OnPropertyChanged(); }
        }

        public int UnloadCount
        {
            get { return m_unloadCount; }
            private set { m_unloadCount = value; OnPropertyChanged(); }
        }

        public int UpdateCount
        {
            get { return m_updateCount; }
            private set { m_updateCount = value; OnPropertyChanged(); }
        }

        public bool IsInitializing
        { 
            get { return m_isInitializing; }
            private set { m_isInitializing = value; OnPropertyChanged(); }
        }

        public bool IsShuttingDown
        { 
            get { return m_isShuttingDown; }
            private set { m_isShuttingDown = value; OnPropertyChanged(); }
        }

        public bool IsLoading
        { 
            get { return m_isLoading; }
            private set { m_isLoading = value; OnPropertyChanged(); }
        }

        public bool IsUnloading
        { 
            get { return m_isUnloading; }
            private set { m_isUnloading = value; OnPropertyChanged(); }
        }

        public bool IsUpdating
        { 
            get { return m_isUpdating; }
            private set { m_isUpdating = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// Initializes the view model.
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
        /// Uninitializes the view model.
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
        /// Loads the view model.
        /// </summary>
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
