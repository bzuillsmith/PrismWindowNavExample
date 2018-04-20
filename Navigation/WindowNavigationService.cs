using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Microsoft.Practices.ServiceLocation;
using PrismWindowNavExample.Views;

namespace PrismWindowNavExample.Navigation
{
    public class WindowNavigationService : IWindowNavigationService
    {
        public const string IS_HOSTED_IN_WINDOW_FLAG = "_isWindow";
        private const string WINDOW_CONTENT_REGION_NAME = "WindowContentRegion";

        private readonly IRegionManager _regionManager;
        private readonly IServiceLocator _container;
        private readonly IWindowPersistanceService _windowPersistanceService;

        Dictionary<WindowManager, WindowNavigationEntry> _windowsByWindowManager { get; set; }
            = new Dictionary<WindowManager, WindowNavigationEntry>();

        public WindowNavigationService(
            IServiceLocator container,
            IRegionManager regionManager,
            IWindowPersistanceService windowPersistanceService)
        {
            _container = container;
            _regionManager = regionManager;
            _windowPersistanceService = windowPersistanceService;
        }
        
        public object ShowDialog(WindowManager windowManager, string uri)
        {
            return ShowDialog(windowManager, uri, null);
        }

        public object ShowDialog(WindowManager parentWindowManager, string uri, NavigationParameters parameters)
        {
            // This might need to be a more flexible instantiation. It would need to come
            //   from some kind of registration list or from the view being added as content.
            var genericWindow = (Window)_container.GetInstance(typeof(GenericWindow));

            if (_windowsByWindowManager.ContainsKey(parentWindowManager))
                genericWindow.Owner = _windowsByWindowManager[parentWindowManager].Window;
            else
                genericWindow.Owner = Application.Current.MainWindow;

            //TODO: could this leak memory?
            genericWindow.SourceInitialized += (s, e) =>
            {
                InitializeWindowProperties(uri, (Window)s);
            };

            var windowRegionManager = _regionManager.CreateRegionManager();
            // Set region manager on the view's attached property
            RegionManager.SetRegionManager(genericWindow, windowRegionManager);
            // Set region manager on the view model
            RegionManagerAware.SetRegionManager(genericWindow, windowRegionManager);
            
            var windowManager = new WindowManager(this);
            SetWindowManager(genericWindow, windowManager);

            // This parameters is a flag for the benefit of Views/ViewModels that want to know if they are hosted in a window or simple region. It has no real function otherwise.
            //  This would allow a view model to know when to call RegionManager.GoBack() or WindowManager.GoBack() if it has both capabilitlies.
            if (parameters != null) parameters.Add(IS_HOSTED_IN_WINDOW_FLAG, true);

            // Create the navigationContext -- NOTE: This is not the exact instance that is passed to the view model
            //   for OnNavigatedTo() because that is built and passed by the RegionNavigationService, but it's pretty much built the same way.
            //   We use it later for OnNavigateFrom from because the RegionManager can't handle the GoBack navigation.
            var contentRegion = windowRegionManager.Regions[WINDOW_CONTENT_REGION_NAME];
            var context = new NavigationContext(contentRegion.NavigationService, new Uri(uri, UriKind.Relative), parameters);
            RecordWindowNavigation(genericWindow, windowManager, context);

            windowRegionManager.RequestNavigate(WINDOW_CONTENT_REGION_NAME, uri, parameters);

            // Look for a Title property on the view model and attempt to bind it to the window Title
            var view = contentRegion.Views.FirstOrDefault();
            if (view == null) throw new Exception("No views in the content region were found. Did you register the view for navigation in the bootstrapper?");
            BindViewModelTitlePropertyToWindowTitle(genericWindow, view);
            
            //TODO: could this leak memory?
            genericWindow.Closing += (s, e) =>
            {
                var window = (Window)s;
                var contentView = GetWindowContentView(windowRegionManager);
                var navigationEntry = FindNavigationEntryForWindow(window);
                TryOnNavigatedFrom(contentView, navigationEntry.Context);

                var keyValuePair = _windowsByWindowManager.FirstOrDefault(kv => kv.Value.Window == window);
                if (keyValuePair.Key != null) _windowsByWindowManager.Remove(keyValuePair.Key);

                SaveWindowProperties(uri, window);
            };

            genericWindow.ShowDialog();

            return windowManager.ReturnData;
        }

        private void RecordWindowNavigation(Window genericWindow, WindowManager windowManager, NavigationContext context)
        {
            _windowsByWindowManager.Add(windowManager, new WindowNavigationEntry(genericWindow, context));
        }

        private void BindViewModelTitlePropertyToWindowTitle(Window window, object item)
        {
            var element = item as FrameworkElement;
            if (element.DataContext != null)
            {
                // Bind Title of the window to Title property on the view model (which is the DataContext)
                BindingOperations.SetBinding(window,
                    Window.TitleProperty,
                    new Binding("Title") { Source = element.DataContext });
            }
        }

        private object GetWindowContentView(IRegionManager windowRegionManager)
        {
            return windowRegionManager.Regions[WINDOW_CONTENT_REGION_NAME].Views.First();
        }

        private WindowNavigationEntry FindNavigationEntryForWindow(Window window)
        {
            return _windowsByWindowManager.Values.ToList().FirstOrDefault(entry => entry.Window == window);
        }

        private void TryOnNavigatedFrom(object item, NavigationContext context)
        {
            var navigationAwareInnerView = item as INavigationAware;
            if (navigationAwareInnerView != null)
            {
                navigationAwareInnerView.OnNavigatedFrom(context);
            }
        }

        public void GoBack(WindowManager windowManager)
        {
            var navigationEntry = _windowsByWindowManager[windowManager];
            var window = navigationEntry.Window;

            window.Close();
        }

        private void SetWindowManager(FrameworkElement view, IWindowManager windowManager)
        {
            var windowAwareViewModel = view.DataContext as IWindowManagerAware;
            if (windowAwareViewModel != null)
            {
                windowAwareViewModel.WindowManager = windowManager;
            }
        }

        
        private void InitializeWindowProperties(string key, Window window)
        {
            var propertiesWereLoadedFromPersistedStore = _windowPersistanceService.LoadWindowProperties(key, window);
            if (!propertiesWereLoadedFromPersistedStore)
            {
                TryToSetDefaultsFromHostedView(window);
            }
        }

        private void TryToSetDefaultsFromHostedView(Window window)
        {
            var content = GetWindowContentView(RegionManager.GetRegionManager(window)) as UIElement;
            if (content != null)
                SetWindowDefaults(window, content);
        }

        private static void SetWindowDefaults(Window window, UIElement contentView)
        {
            var width = WindowProps.GetDefaultWidth(contentView);
            var height = WindowProps.GetDefaultHeight(contentView);

            if (width > 0d && height > 0d)
            {
                window.Width = width;
                window.Height = height;
            }

            window.SizeToContent = SizeToContent.Manual;
        }

        private void SaveWindowProperties(string key, Window window)
        {
            _windowPersistanceService.SaveWindowProperties(key, window);
        }
    }
}