using HealthApp.Common.Model;
using MvvmHelpers;

namespace HealthApp.Models
{
    public class BookmarkModel : BaseViewModel
    {
        private ObservableRangeCollection<Record> _records;
        public ObservableRangeCollection<Record> Records
        {
            get { return _records; }
            set
            {
                _records = value;
                OnPropertyChanged();
            }
        }

        private bool _hasError;
        public bool HasError
        {
            get { return _hasError; }
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                _isEmpty = value;
                OnPropertyChanged();
            }
        }

        private bool _isAuthorized;
        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set
            {
                _isAuthorized = value;
                OnPropertyChanged();
            }
        }

        public BookmarkModel()
        {
            Records = new ObservableRangeCollection<Record>();

            IsBusy = true;
            IsAuthorized = true;
        }
    }
}
