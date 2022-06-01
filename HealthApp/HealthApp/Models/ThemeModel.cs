using MvvmHelpers;

namespace HealthApp.Models
{
    public class ThemeModel : BaseViewModel
    {
        /// <summary>
        /// Используется для проверки активности текущей темы
        /// </summary>
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
