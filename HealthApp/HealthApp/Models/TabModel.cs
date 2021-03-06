using HealthApp.ViewModels.Data;
using MvvmHelpers;

namespace HealthApp.Models
{
    public class TabModel : BaseViewModel
    {
        public int _page;
        public int Page 
        {
            get => _page;
            set
            {
                _page = value;
                OnPropertyChanged();
            }
        }

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

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing; 
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<RecordViewModel> _records;
        public ObservableRangeCollection<RecordViewModel> Records
        {
            get => _records; 
            set
            {
                _records = value;
                OnPropertyChanged();
            }
        }

        public TabModel()
        {
            Records = new ObservableRangeCollection<RecordViewModel>();

            IsBusy = true;
        }
    }
}
