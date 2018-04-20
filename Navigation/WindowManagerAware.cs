using System.Windows;

namespace PrismWindowNavExample.Navigation
{
    public static class WindowManagerAware
    {
        public static void SetWindowManager(object item, IWindowManager windowManager)
        {
            var rmAwareViewModel = item as IWindowManagerAware;
            if (rmAwareViewModel != null)
                rmAwareViewModel.WindowManager = windowManager;

            var rmAwareFrameworkElement = item as FrameworkElement;
            if (rmAwareFrameworkElement != null)
            {
                var rmAwareDataContext = rmAwareFrameworkElement.DataContext as IWindowManagerAware;
                if (rmAwareDataContext != null)
                    rmAwareDataContext.WindowManager = windowManager;
            }
        }
    }
}
