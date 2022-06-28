using HealthApp.ViewModels.Data;
using MvvmHelpers;

namespace HealthApp.Models
{
    public class AuthorAndCategoryModel : BaseViewModel
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

        private ObservableRangeCollection<AuthorAndCategoryViewModel> _authorsAndСategories;
        public ObservableRangeCollection<AuthorAndCategoryViewModel> AuthorsAndСategories
        {
            get => _authorsAndСategories;
            set
            {
                _authorsAndСategories = value;
                OnPropertyChanged();
            }
        }

        public AuthorAndCategoryModel()
        {
            AuthorsAndСategories = new ObservableRangeCollection<AuthorAndCategoryViewModel>();

            IsBusy = true;
        }
    }
}
