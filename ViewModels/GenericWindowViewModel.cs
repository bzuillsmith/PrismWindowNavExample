using Prism.Regions;
using PrismWindowNavExample.Navigation;

namespace PrismWindowNavExample.ViewModels
{
    public class GenericWindowViewModel : IRegionManagerAware, IWindowManagerAware
    {
        public IRegionManager RegionManager { get; set; }
        public IWindowManager WindowManager { get; set; }
        public string Title { get; set; }
    }
}
