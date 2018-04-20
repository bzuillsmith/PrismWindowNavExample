using System.Windows;

namespace PrismWindowNavExample.Navigation
{
    public class WindowPersistenceStore : IWindowPersistanceService
    {
        public bool LoadWindowProperties(string key, Window window)
        {
            // TODO: Load window width, height, and position
            return false;
        }

        public void SaveWindowProperties(string key, Window window)
        {
            // TODO: Save window width height and position.
        }
    }
}
