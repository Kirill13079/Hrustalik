using HealthApp.Common.Model;
using MvvmHelpers;

namespace HealthApp.Models
{
    public class AuthorsAndCategoriesModel : BaseViewModel
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

    public class AuthorAndCategoryTabModel : BaseViewModel
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

        private ObservableRangeCollection<AuthorsAndCategoriesModel> _authorsAndСategories;
        public ObservableRangeCollection<AuthorsAndCategoriesModel> AuthorsAndСategories
        {
            get => _authorsAndСategories;
            set
            {
                _authorsAndСategories = value;
                OnPropertyChanged();
            }
        }

        public AuthorAndCategoryTabModel()
        {
            AuthorsAndСategories = new ObservableRangeCollection<AuthorsAndCategoriesModel>();

            IsBusy = true;
        }
    }
}
