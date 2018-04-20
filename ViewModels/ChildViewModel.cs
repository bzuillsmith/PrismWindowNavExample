using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismWindowNavExample.Navigation;
using PrismWindowNavExample.Views;

namespace PrismWindowNavExample.ViewModels
{
    public class ChildViewModel : BindableBase, IWindowManagerAware, INavigationAware
    {
        public ChildViewModel()
        {

        }

        private const string DIALOG_NUMBER_PARAMETER_NAME = "dialog_number";
        
        public IWindowManager WindowManager { get; set; }

        private string _title = "Window";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private int _dialogNumber;
        public int DialogNumber
        {
            get { return _dialogNumber; }
            set { SetProperty(ref _dialogNumber, value); }
        }

        private DelegateCommand _openChildWindowCommand;
        public DelegateCommand OpenChildWindowCommand =>
            _openChildWindowCommand ?? (_openChildWindowCommand = new DelegateCommand(ExecuteOpenChildWindowCommand));
        void ExecuteOpenChildWindowCommand()
        {
            WindowManager.ShowDialog(nameof(ChildView), new NavigationParameters() { { DIALOG_NUMBER_PARAMETER_NAME, DialogNumber + 1 } });
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var dialogNumberParameter = navigationContext.Parameters[DIALOG_NUMBER_PARAMETER_NAME] as int?;
            if (dialogNumberParameter.HasValue)
                DialogNumber = dialogNumberParameter.Value;

            Title = $"Window {DialogNumber}";
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
