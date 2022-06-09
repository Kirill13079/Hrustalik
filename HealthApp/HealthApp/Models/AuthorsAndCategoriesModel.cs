using HealthApp.Common.Model;
using MvvmHelpers;

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
    }
}
