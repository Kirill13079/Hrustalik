using System.Windows.Input;
using Acr.UserDialogs;
using HealthApp.Interfaces;
using HealthApp.Service;
using MvvmHelpers;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Base
{
    public class ViewBaseModel : BaseViewModel
    {
        protected readonly IApiManager ApiManagerService = new ApiManagerService();
        protected readonly IAlertDialog AlertDialogService = new AlertDialogService();

        protected static IUserDialogs UserDialogs = Acr.UserDialogs.UserDialogs.Instance;

        private LayoutState _currentState = LayoutState.Loading;
        public LayoutState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SearchCommand { get; set; }

        public ICommand LikeRecordCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand ReloadCommand { get; set; }

        public ICommand SignOutCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public ICommand AuthorizationCommand { get; set; }

        public ICommand GoBackPageCommand { get; }

        public ViewBaseModel()
        {
            GoBackPageCommand = new Command(GoBackPageCommandHandler);
        }

        private void GoBackPageCommandHandler()
        {
            NavigationService.GoBackAsync();
        }
    }
}
