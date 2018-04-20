using Prism.Regions;
using System;
using System.Collections.Specialized;
using System.Windows;

namespace PrismWindowNavExample.Navigation
{
    public class RegionManagerAwareBehavior : RegionBehavior
    {
        public const string BehaviorKey = "RegionManagerAwareBehavior";

        protected override void OnAttach()
        {
            Region.Views.CollectionChanged += Views_CollectionChanged;
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    IRegionManager regionManager = Region.RegionManager;

                    FrameworkElement element = item as FrameworkElement;
                    if (element != null)
                    {
                        IRegionManager scopedRegionManager = element.GetValue(RegionManager.RegionManagerProperty) as IRegionManager;
                        if (scopedRegionManager != null)
                            regionManager = scopedRegionManager;
                    }

                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = regionManager);
                }

            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = null);
                }
            }
        }

        static void InvokeOnRegionManagerAwareElement(object item, Action<IRegionManagerAware> invocation)
        {
            var rmAwareItem = item as IRegionManagerAware;
            if (rmAwareItem != null)
                invocation(rmAwareItem);

            var frameworkElement = item as FrameworkElement;
            if (frameworkElement != null)
            {
                var rmAwareDataContext = frameworkElement.DataContext as IRegionManagerAware;
                if (rmAwareDataContext != null)
                {
                    var frameworkElementParent = frameworkElement.Parent as FrameworkElement;
                    if (frameworkElementParent != null)
                    {
                        var rmAwareDataContextParent = frameworkElementParent.DataContext as IRegionManagerAware;
                        if (rmAwareDataContextParent != null && rmAwareDataContext == rmAwareDataContextParent && rmAwareDataContext.RegionManager != null)
                        {
                            return;
                        }
                    }

                    invocation(rmAwareDataContext);
                }
            }
        }
    }
}
