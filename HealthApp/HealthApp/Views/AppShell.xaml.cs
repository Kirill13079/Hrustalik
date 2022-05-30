using HealthApp.Views.Authors;
using HealthApp.Views.Categories;
using HealthApp.Views.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        private bool _startUp = true;
        private Dictionary<string, Type> _routes = new Dictionary<string, Type>();

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            _routes.Add("record", typeof(RecordPage));
            _routes.Add("categoryInfo", typeof(CategoryInfoPage));
            _routes.Add("authorInfo", typeof(AuthorInfoPage));

            foreach (var route in _routes)
            {
                Routing.RegisterRoute(route.Key, route.Value);
            }
        }

        private DateTime LastFlyoutHiddenUtcDateTime { get; set; }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(FlyoutIsPresented))
            {
                if (!FlyoutIsPresented)
                {
                    LastFlyoutHiddenUtcDateTime = DateTime.UtcNow;
                }
            }
        }

        private bool WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug = false;

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            if (!WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug)
            {
                // if the above value is true, then this is the re-run navigation from the GoToAsync(args.Target) call below - skip this block this second pass through, as the flyout is now closed
                if ((DateTime.UtcNow - LastFlyoutHiddenUtcDateTime).TotalMilliseconds < 1000)
                {
                    args.Cancel();

                    FlyoutIsPresented = false;

                    OnPropertyChanged(nameof(FlyoutIsPresented));

                    await Task.Delay(300);

                    WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug = true;

                    // re-run the originally requested navigation
                    await GoToAsync(args.Target);

                    return;
                }
            }

            WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug = false;

            base.OnNavigating(args);
        }
	}

    //protected override void OnNavigated(ShellNavigatedEventArgs args)
    //{
    //    base.OnNavigated(args);
    //    _startUp = false;
    //}

    //protected async override void OnPropertyChanged(
    //    [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    //{
    //    base.OnPropertyChanged(propertyName);
    //    if (!_startUp && propertyName.Equals("CurrentState") 
    //        && Device.RuntimePlatform == Device.Android)
    //    {
    //        FlyoutIsPresented = true;

    //        await Task.Delay(300);

    //        FlyoutIsPresented = false;
    //    }
    //}
}