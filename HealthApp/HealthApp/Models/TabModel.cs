using HealthApp.Common.Model;
using MvvmHelpers;

namespace HealthApp.Models
{
    public class TabModel : BaseViewModel
    {
        public int Page { get; set; }

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

        public TabModel()
        {
            Records = new ObservableRangeCollection<Record>();
            IsBusy = true;
        }
    }
}
