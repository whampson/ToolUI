using WpfEssentials;
using System;

namespace WHampson.ToolUI
{
    /// <summary>
    /// Base class for all view models.
    /// </summary>
    public class BaseVM : ObservableObject
    {
        public EventHandler Initializing;
        public EventHandler ShuttingDown;
        public EventHandler Loading;
        public EventHandler Unloading;
        public EventHandler Updating;

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        /// <remarks>
        /// Perform lifetime initialization tasks here.
        /// </remarks>
        public virtual void Init()
        {
            Initializing?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Uninitializes the view model.
        /// </summary>
        /// <remarks>
        /// Perform lifetime cleanup tasks here.
        /// </remarks>
        public virtual void Shutdown()
        {
            ShuttingDown?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Loads the view model.
        /// </summary>
        public virtual void Load()
        {
            Loading?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Unloads the view model.
        /// </summary>
        public virtual void Unload()
        {
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
            Updating?.Invoke(this, EventArgs.Empty);
        }
    }
}
