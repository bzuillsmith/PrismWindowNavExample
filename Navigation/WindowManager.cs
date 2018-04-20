using Prism.Regions;

namespace PrismWindowNavExample.Navigation
{
    public class WindowManager : IWindowManager
    {
        public WindowManager(IWindowNavigationService windowNavigationService)
        {
            WindowNavigationService = windowNavigationService;
        }


        public IWindowNavigationService WindowNavigationService { get; set; }

        public object ReturnData { get; private set; }


        public void GoBack(object returnData)
        {
            ReturnData = returnData;
            WindowNavigationService.GoBack(this);
        }

        public void GoBack()
        {
            WindowNavigationService.GoBack(this);
        }
        
        public object ShowDialog(string uri)
        {
            return ShowDialog(uri, null);
        }

        public object ShowDialog(string uri, NavigationParameters parameters)
        {
            return WindowNavigationService.ShowDialog(this, uri, parameters);
        }
    }
}