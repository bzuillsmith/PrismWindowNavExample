using Prism.Regions;

namespace PrismWindowNavExample.Navigation
{
    public interface IWindowManager
    {
        object ShowDialog(string uri);
        object ShowDialog(string uri, NavigationParameters parameters);
        void GoBack(object returnData);
        void GoBack();
    }
}
