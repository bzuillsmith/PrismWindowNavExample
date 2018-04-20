using Prism.Regions;

namespace PrismWindowNavExample.Navigation
{
    /// <summary>
    /// Interface that view models must implement to use the current window's RegionManager
    /// </summary>
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; set; }
    }
}
