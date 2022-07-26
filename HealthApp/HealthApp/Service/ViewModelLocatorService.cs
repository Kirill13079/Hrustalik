using HealthApp.ViewModels;

namespace HealthApp.Service
{
    public class ViewModelLocatorService
    {
        private MainViewModel _mainVm;
        public MainViewModel MainVm => _mainVm ?? (_mainVm = new MainViewModel());

        private CategoryViewModel _categoryVm;
        public CategoryViewModel CategoryVm => _categoryVm ?? (_categoryVm = new CategoryViewModel());

        private SettingsViewModel _settingsVm;
        public SettingsViewModel SettingsVM => _settingsVm ?? (_settingsVm = new SettingsViewModel());

        private BookmarkViewModel _bookmarkVm;
        public BookmarkViewModel BookmarkVm => _bookmarkVm ?? (_bookmarkVm = new BookmarkViewModel());

        public LoginViewModel LoginVm => new LoginViewModel();

        public AuthorsAndCategoriesViewModel AuthorsAndCategoriesVm => new AuthorsAndCategoriesViewModel();

        public ViewModelLocatorService()
        {
            _mainVm = new MainViewModel();
            _categoryVm = new CategoryViewModel();
            _bookmarkVm = new BookmarkViewModel();
            _settingsVm = new SettingsViewModel();
        }
    }
}
