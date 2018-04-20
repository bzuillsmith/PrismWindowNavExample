using Prism.Regions;

namespace PrismWindowNavExample.Navigation
{
    public interface IWindowNavigationService
    {
        object ShowDialog(WindowManager windowManager, string uri, NavigationParameters parameters);
        object ShowDialog(WindowManager windowManager, string uri);
        void GoBack(WindowManager windowManager);
    }
}
