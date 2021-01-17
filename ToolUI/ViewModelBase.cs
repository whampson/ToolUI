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

        public int InitCount
        {
            get { return m_initCount; }
            set { m_initCount = value; OnPropertyChanged(); }
        }

        public int ShutdownCount
        {
            get { return m_shutdownCount; }
            set { m_shutdownCount = value; OnPropertyChanged(); }
        }

        public int LoadCount
        {
            get { return m_loadCount; }
            set { m_loadCount = value; OnPropertyChanged(); }
        }

        public int UnloadCount
        {
            get { return m_unloadCount; }
            set { m_unloadCount = value; OnPropertyChanged(); }
        }

        public int UpdateCount
        {
            get { return m_updateCount; }
            set { m_updateCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        /// <remarks>
        /// Perform lifetime initialization tasks here (e.g. allocating global memory).
        /// </remarks>
        public virtual void Init()
        {
            InitCount++;
            Initializing?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Uninitializes the view model.
        /// </summary>
        /// <remarks>
        /// Perform lifetime cleanup tasks here (e.g. freeing global memory).
        /// </remarks>
        public virtual void Shutdown()
        {
            ShutdownCount++;
            ShuttingDown?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Loads the view model.
        /// </summary>
        public virtual void Load()
        {
            LoadCount++;
            Loading?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Unloads the view model.
        /// </summary>
        public virtual void Unload()
        {
            UnloadCount++;
            Unloading?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the view model state.
        /// </summary>
        /// <remarks>
        /// Perform refresh tasks here.
        /// </remarks>
        public virtual void Update()
        {
            UpdateCount++;
            Updating?.Invoke(this, EventArgs.Empty);
        }
    }
}
