using Prism.Mvvm;
using PrismWindowNavExample.Navigation;

namespace PrismWindowNavExample.ViewModels
{
    public class MainWindowViewModel : BindableBase, IWindowManagerAware
    {
        public IWindowManager WindowManager { get; set; }
        
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _title = "WinBidPro";
    }
}
