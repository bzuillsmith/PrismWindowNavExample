using Prism.Regions;
using System;
using System.Collections.Specialized;
using System.Windows;

namespace PrismWindowNavExample.Navigation
{
    public class WindowManagerAwareBehavior : RegionBehavior
    {
        public const string BehaviorKey = "WindowManagerAwareBehavior";

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
        }

        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    IWindowManager windowManager = null;

                    FrameworkElement element = item as FrameworkElement;
                    if (element != null)
                    {
                        var window = FindAncestorOfType(element, typeof(Window));
                        if (window != null)
                        {
                            var dc = window.DataContext as IWindowManagerAware;
                            if (dc != null)
                                windowManager = dc.WindowManager;
                        }
                    }

                    InvokeOnWindowManagerAwareElement(item, x => x.WindowManager = windowManager);
                }

            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnWindowManagerAwareElement(item, x => x.WindowManager = null);
                }
            }
        }

        static void InvokeOnWindowManagerAwareElement(object item, Action<IWindowManagerAware> invocation)
        {
            var winAwareItem = item as IWindowManagerAware;
            if (winAwareItem != null)
                invocation(winAwareItem);

            var frameworkElement = item as FrameworkElement;
            if (frameworkElement != null)
            {
                var winAwareDataContext = frameworkElement.DataContext as IWindowManagerAware;
                if (winAwareDataContext != null)
                {
                    // could be a view with no view model so we need to check
                    var frameworkElementParent = frameworkElement.Parent as FrameworkElement;
                    if (frameworkElementParent != null)
                    {
                        var winAwareDataContextParent = frameworkElementParent.DataContext as IWindowManagerAware;
                        if (winAwareDataContextParent != null && winAwareDataContext == winAwareDataContextParent)
                        {
                            return;
                        }
                    }

                    invocation(winAwareDataContext);
                }
            }
        }

        public FrameworkElement FindAncestorOfType(FrameworkElement item, Type type)
        {
            if (!item.GetType().IsSubclassOf(type))
            {
                var parentElement = item.Parent as FrameworkElement;
                if (parentElement != null)
                    return FindAncestorOfType(parentElement, type);
                else
                    return null;
            }
            else
            {
                return item;
            }

        }
    }

}