using HealthApp.ViewModels.Data;
using MvvmHelpers;

namespace HealthApp.Models
{
    public class BookmarkModel : BaseViewModel
    {
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

        private bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                OnPropertyChanged();
            }
        }

        private bool _isAuthorized;
        public bool IsAuthorized
        {
            get => _isAuthorized; 
            set
            {
                _isAuthorized = value;
                OnPropertyChanged();
            }
        }

        public BookmarkModel()
        {
            Records = new ObservableRangeCollection<RecordViewModel>();

            IsBusy = true;
            IsAuthorized = true;
        }
    }
}
