using HealthApp.Animations;
using HealthApp.Utils;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private enum State
        {
            HeaderExpanded,
            HeaderHidden
        }

        private AnimationStateMachine _animation;

        private const int _animationSpeed = 250;
        private const int _headerHeight = 80;
        private const int _padding = 5;

        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = App.ViewModelLocator.SettingsVM;

            Shell.SetNavBarIsVisible(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SizeChanged += SettingsPageSizeChanged;
            scrollContainer.Scrolled += ScrollContainerScrolled;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            SizeChanged -= SettingsPageSizeChanged;
        }

        private async void ScrollContainerScrolled(object sender, ScrolledEventArgs e)
        {
            if (_animation.CurrentState != null)
            {
                if (e.ScrollY > 0 && (State)_animation.CurrentState != State.HeaderHidden)
                {
                    await ChangedStatePageAsync();
                }
                else if (e.ScrollY == 0 && (State)_animation.CurrentState != State.HeaderExpanded)
                {
                    await ChangedStatePageAsync();
                }
            }
        }

        private void SettingsPageSizeChanged(object sender, System.EventArgs e)
        {
            _animation = new AnimationStateMachine();

            Rectangle startFrameRect = new Rectangle(
                x: 0,
                y: _headerHeight,
                width: Width,
                height: Height);
            Rectangle endFrameRect = new Rectangle(
                x: 0,
                y: _headerHeight / 2,
                width: Width,
                height: Height);

            AbsoluteLayout.SetLayoutBounds(bindable: contentFrame, bounds: startFrameRect);

            Rectangle startStackLayoutHeader = new Rectangle(
                x: 10,
                y: startFrameRect.Y - headerLabel.Height - _padding,
                width: headerLabel.Width,
                height: headerLabel.Height);
            Rectangle endStackLayoutHeader = new Rectangle(
                x: (Width / 2) - (headerLabel.Width / 2),
                y: -_padding,
                width: headerLabel.Width,
                height: headerLabel.Height);

            AbsoluteLayout.SetLayoutBounds(bindable: headerPage, bounds: startStackLayoutHeader);

            _animation.Add(State.HeaderExpanded, new ViewTransition[]
            {
                new ViewTransition(targetElement: headerPage, animationType: AnimationEnum.AnimationType.Layout, endLayout: startStackLayoutHeader),
                new ViewTransition(targetElement: headerPage, animationType: AnimationEnum.AnimationType.Scale, endValue: 1),
                new ViewTransition(targetElement: contentFrame, animationType: AnimationEnum.AnimationType.Layout, endLayout: startFrameRect)
            });

            _animation.Add(State.HeaderHidden, new ViewTransition[]
            {
                new ViewTransition(headerPage, AnimationEnum.AnimationType.Layout, endStackLayoutHeader),
                new ViewTransition(headerPage, AnimationEnum.AnimationType.Scale, 0.5),
                new ViewTransition(contentFrame, AnimationEnum.AnimationType.Layout, endFrameRect)
            });

            _animation.CurrentState = State.HeaderExpanded;

            headerPage.Scale = 1;
        }

        private async Task ChangedStatePageAsync()
        {
            switch (_animation.CurrentState)
            {
                case State.HeaderExpanded:
                    await StartAnimation(State.HeaderHidden);
                    break;
                case State.HeaderHidden:
                    await StartAnimation(State.HeaderExpanded);
                    break;
                default:
                    break;
            }
        }

        private async Task StartAnimation(State state)
        {
            _animation.Go(state);

            scrollContainer.IsEnabled = false;

            await Task.Delay(_animationSpeed);

            scrollContainer.IsEnabled = true;
        }

        private void AppThemeTapped(object sender, System.EventArgs e)
        {

        }
    }
}