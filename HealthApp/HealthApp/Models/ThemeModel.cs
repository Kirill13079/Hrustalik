using MvvmHelpers;

namespace HealthApp.Models
{
    public class ThemeModel : BaseViewModel
    {
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
