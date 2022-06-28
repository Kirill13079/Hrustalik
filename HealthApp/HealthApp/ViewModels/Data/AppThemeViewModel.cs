using MvvmHelpers;

namespace HealthApp.ViewModels.Data
{
    public class AppThemeViewModel : ObservableObject
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _subtitle;
        public string Subtitle
        {
            get => _subtitle;
            set
            {
                _subtitle = value;
                OnPropertyChanged();
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }
    }
}
