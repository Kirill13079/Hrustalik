using HealthApp.ViewModels.Data;
using MvvmHelpers;
using Xamarin.CommunityToolkit.UI.Views;

namespace HealthApp.Models
{
    public class ExtensibleModel : BaseViewModel
    {
        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }

        private LayoutState _currentStateTab;
        public LayoutState CurrentStateTab
        {
            get => _currentStateTab;
            set
            {
                if (_currentStateTab != value)
                {
                    _currentStateTab = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableRangeCollection<ExtensibleObject> _extensibleItems;
        public ObservableRangeCollection<ExtensibleObject> ExtensibleItems
        {
            get => _extensibleItems;
            set
            {
                _extensibleItems = value;
                OnPropertyChanged();
            }
        }

        public ExtensibleModel()
        {
            ExtensibleItems = new ObservableRangeCollection<ExtensibleObject>();

            IsBusy = true;
        }
    }
}
