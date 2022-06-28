using HealthApp.ViewModels.Data;
using MvvmHelpers;

namespace HealthApp.Models
{
    public class MainTabModel : BaseViewModel
    {
        private RecordViewModel _hotRecord;
        public RecordViewModel HotRecord
        {
            get => _hotRecord;
            set
            {
                _hotRecord = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<TabModel> _subTabModel;
        public ObservableRangeCollection<TabModel> SubTabModel
        {
            get => _subTabModel;
            set
            {
                _subTabModel = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<RecordViewModel> _records;
        public ObservableRangeCollection<RecordViewModel> Records
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

        public MainTabModel()
        {
            SubTabModel = new ObservableRangeCollection<TabModel>();
            Records = new ObservableRangeCollection<RecordViewModel>();
            HotRecord = new RecordViewModel();

            IsBusy = true;
        }
    }
}
