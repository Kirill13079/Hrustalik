using System.Windows.Input;
using HealthApp.Interfaces;
using HealthApp.Service;
using MvvmHelpers;
using Xamarin.CommunityToolkit.UI.Views;

namespace HealthApp.ViewModels.Base
{
    public class ViewBaseModel : BaseViewModel
    {
        protected readonly IApiManager ApiManager = new ApiManager();

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

        public ICommand LikeRecordCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand ReloadCommand { get; set; }

        public ICommand SignOutCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public ICommand AuthorizationCommand { get; set; }

        public ViewBaseModel()
        {

        }
    }
}
