using Prism.Regions;
using System.Windows;

namespace PrismWindowNavExample.Navigation
{
    public static class RegionManagerAware
    {
        public static void SetRegionManager(object item, IRegionManager regionManager)
        {
            var rmAwareViewModel = item as IRegionManagerAware;
            if (rmAwareViewModel != null)
                rmAwareViewModel.RegionManager = regionManager;

            var rmAwareFrameworkElement = item as FrameworkElement;
            if (rmAwareFrameworkElement != null)
            {
                var rmAwareDataContext = rmAwareFrameworkElement.DataContext as IRegionManagerAware;
                if (rmAwareDataContext != null)
                    rmAwareDataContext.RegionManager = regionManager;
            }
        }
    }
}
