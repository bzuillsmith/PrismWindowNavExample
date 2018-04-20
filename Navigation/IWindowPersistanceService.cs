using System.Windows;

namespace PrismWindowNavExample.Navigation
{
    public interface IWindowPersistanceService
    {
        /// <summary>
        /// Save data about the window. It should include the window width, height, and position.
        /// </summary>
        /// <param name="key">A unique key to identify the view hosted in the window.</param>
        /// <param name="window"></param>
        /// <returns>True if properties were applied, otherwise false.</returns>
        void SaveWindowProperties(string key, Window window);
        
        /// <summary>
        /// Load any persisted data about the window and apply it. For example, the window width, height, and position.
        /// </summary>
        /// <param name="key">A unique key to identify the view hosted in the window.</param>
        /// <param name="window"></param>
        /// <returns>True if properties were applied, otherwise false.</returns>
        bool LoadWindowProperties(string key, Window window);
    }
}