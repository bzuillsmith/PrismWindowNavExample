using Microsoft.Practices.Unity;
using Prism.Regions;
using Prism.Unity;
using PrismWindowNavExample.Navigation;
using PrismWindowNavExample.Views;
using System.Threading;
using System.Windows;

namespace PrismWindowNavExample
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IWindowNavigationService, WindowNavigationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IWindowPersistanceService, WindowPersistenceStore>();
            Container.RegisterType<IWindowManager, WindowManager>();
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var behaviors = base.ConfigureDefaultRegionBehaviors();
            behaviors.AddIfMissing(RegionManagerAwareBehavior.BehaviorKey, typeof(RegionManagerAwareBehavior));
            behaviors.AddIfMissing(WindowManagerAwareBehavior.BehaviorKey, typeof(WindowManagerAwareBehavior));
            return behaviors;
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            Container.RegisterTypeForNavigation<ChildView>();
        }

        protected override void InitializeShell()
        {
            WindowManagerAware.SetWindowManager(Shell, Container.Resolve<IWindowManager>());

            var regionManager = Container.Resolve<IRegionManager>();
            Application.Current.MainWindow.Show();

            SynchronizationContext.Current.Post((state) =>
            {
                regionManager.RequestNavigate("WindowContentRegion", nameof(ChildView), new NavigationParameters());
            }, null);
        }
    }
}
