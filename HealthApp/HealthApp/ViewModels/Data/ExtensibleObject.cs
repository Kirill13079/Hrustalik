using HealthApp.Common.Model;
using MvvmHelpers;

namespace HealthApp.ViewModels.Data
{
    public class ExtensibleObject : ObservableObject
    {
        private Category _category;
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        private Author _author;
        public Author Author
        {
            get => _author;
            set
            {
                _author = value;
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
