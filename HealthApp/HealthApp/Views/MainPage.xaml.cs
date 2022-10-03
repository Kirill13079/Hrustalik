using HealthApp.Animations;
using HealthApp.Models;
using HealthApp.Utils;
using HealthApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _bindingContext;
        private readonly List<MainTabModel> _bindingObject;

        private enum StatePage
        {
            Expanded,
            Completed
        }

        private AnimationStateMachine _animation;

        private const int _animationSpeed = 500;
        private const int _headerHeight = 80;
        private const int _padding = 5;

        public MainPage()
        {
            InitializeComponent();

            BindingContext = App.ViewModelLocator.MainVm;

            Shell.SetNavBarIsVisible(this, false);

            _bindingContext = BindingContext as MainViewModel;

            _bindingObject = new List<MainTabModel>
            {
                _bindingContext.MainTab
            };

            mainScrollView.ItemsSource = _bindingObject;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_animation?.CurrentState != null)
            {
                MainPageSizeChanged(this, new EventArgs());
            }

            SizeChanged += MainPageSizeChanged;
            scrollContainer.Scrolled += ScrollContainerScrolled;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            SizeChanged -= MainPageSizeChanged;
        }

        private async void ScrollContainerScrolled(object sender, ScrolledEventArgs e)
        {
            if (_animation.CurrentState != null)
            {
                StatePage currentState = (StatePage)_animation.CurrentState;

                if (e.ScrollY > 0 && currentState != StatePage.Completed)
                {
                    await ChangedStatePageAsync();
                }
                else if (e.ScrollY == 0 && currentState != StatePage.Expanded)
                {
                    await ChangedStatePageAsync();
                }
            }
        }

        private void MainPageSizeChanged(object sender, EventArgs e)
        {
            _animation = new AnimationStateMachine();

            Rectangle startContentPageRect = new Rectangle(x: 0, y: _headerHeight, width: Width, height: Height);
            Rectangle endContentPageRect = new Rectangle(x: 0, y: _headerHeight / 2, width: Width, height: Height);

            AbsoluteLayout.SetLayoutBounds(bindable: contentPage, bounds: startContentPageRect);

            Rectangle startHeaderPageRect = new Rectangle(x: 10, y: startContentPageRect.Y - headerLabel.Height - _padding, width: headerLabel.Width, height: headerLabel.Height);
            Rectangle endHeaderPageRect = new Rectangle(x: (Width / 2) - (headerLabel.Width / 2), y: -_padding, width: headerLabel.Width, height: headerLabel.Height);

            AbsoluteLayout.SetLayoutBounds(bindable: headerPage, bounds: startHeaderPageRect);

            _animation.Add(StatePage.Expanded, new ViewTransition[]
            {
                new ViewTransition(targetElement: headerPage, animationType: AnimationEnum.AnimationType.Layout, endLayout: startHeaderPageRect),
                new ViewTransition(targetElement: headerPage, animationType: AnimationEnum.AnimationType.Scale, endValue: 1),
                new ViewTransition(targetElement: contentPage, animationType: AnimationEnum.AnimationType.Layout, endLayout: startContentPageRect)
            });

            _animation.Add(StatePage.Completed, new ViewTransition[]
            {
                new ViewTransition(targetElement: headerPage, animationType: AnimationEnum.AnimationType.Layout, endLayout: endHeaderPageRect),
                new ViewTransition(targetElement: headerPage, animationType: AnimationEnum.AnimationType.Scale, endValue: 0.5),
                new ViewTransition(targetElement: contentPage, animationType: AnimationEnum.AnimationType.Layout, endLayout: endContentPageRect)
            });

            _animation.CurrentState = StatePage.Expanded;

            headerPage.Scale = 1;

            if (scrollContainer.ScrollY != 0)
            {
                _ = scrollContainer.ScrollToAsync(0, 0, true);
            }
        }

        private async Task ChangedStatePageAsync()
        {
            switch (_animation.CurrentState)
            {
                case StatePage.Expanded:
                    await StartAnimation(StatePage.Completed);
                    break;
                case StatePage.Completed:
                    await StartAnimation(StatePage.Expanded);
                    break;
                default:
                    break;
            }
        }

        private async Task StartAnimation(StatePage state)
        {
            _animation.Go(state);

            scrollContainer.IsEnabled = false;

            await Task.Delay(_animationSpeed);

            scrollContainer.IsEnabled = true;
        }
    }
}