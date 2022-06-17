using HealthApp.Common.Model;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthApp.Models
{
    public class AuthorsAndCategoriesModel : BaseViewModel
    {
        private Category _category;
        public Category Category
        {
            get { return _category; }
            set { _category = value; }
        }

        private Author _author;
        public Author Author
        {
            get { return _author; }
            set { _author = value; }
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

        private ObservableRangeCollection<AuthorsAndCategoriesModel> _authorsAndСategories;
        public ObservableRangeCollection<AuthorsAndCategoriesModel> AuthorsAndСategories
        {
            get { return _authorsAndСategories; }
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
