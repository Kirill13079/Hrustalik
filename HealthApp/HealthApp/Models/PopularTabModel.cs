using HealthApp.Common.Model;
using MvvmHelpers;

namespace HealthApp.Models
{
    public class PopularTabModel : BaseViewModel
    {
        private int _page;
        public int Page 
        { 
            get => _page; 
            set 
            { 
                _page = value; 
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

        public PopularTabModel()
        {
            Records = new ObservableRangeCollection<Record>();
            IsBusy = true;
        }
    }
}
