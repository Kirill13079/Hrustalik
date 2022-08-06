using HealthApp.Controls.Base;
using System;
using Xamarin.Forms;

namespace HealthApp.Controls.Behaviors
{
    public class SwipeDownToClosePopupPage : BaseBehavior<View>
    {
        private readonly PanGestureRecognizer _panGestureRecognizer;

        private DateTimeOffset? _startPanDownTime;
        private DateTimeOffset? _endPanDownTime;
        private bool _reachedEdge;
        private double _totalY;

        private static BindableProperty ClosingEdgeProperty = BindableProperty.Create(
            propertyName: nameof(ClosingEdge),
            returnType: typeof(double),
            declaringType: typeof(SwipeDownToClosePopupPage),
            defaultValue: Convert.ToDouble(100),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnClosingEdgePropertyChanged);

        private static BindableProperty ClosingTimeInMsProperty = BindableProperty.Create(
            propertyName: nameof(ClosingTimeInMs),
            returnType: typeof(long),
            declaringType: typeof(SwipeDownToClosePopupPage),
            defaultValue: Convert.ToInt64(500),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnClosingTimeInMsPropertyChanged);

        public event Action CloseAction;

        public SwipeDownToClosePopupPage()
        {
            _panGestureRecognizer = new PanGestureRecognizer();
        }

        public double ClosingEdge
        {
            get => (double)GetValue(ClosingEdgeProperty);
            set => SetValue(ClosingEdgeProperty, Convert.ToDouble(value));
        }

        public long ClosingTimeInMs
        {
            get => (long)GetValue(ClosingTimeInMsProperty);
            set => SetValue(ClosingTimeInMsProperty, Convert.ToInt64(value));
        }

        private static void OnClosingEdgePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SwipeDownToClosePopupPage control = (SwipeDownToClosePopupPage)bindable;

            if (newValue != null)
            {
                control.ClosingEdge = Convert.ToDouble(newValue);
            }
        }

        private static void OnClosingTimeInMsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SwipeDownToClosePopupPage control = (SwipeDownToClosePopupPage)bindable;

            if (newValue != null)
            {
                control.ClosingTimeInMs = Convert.ToInt64(newValue);
            }
        }

        protected override void OnAttachedTo(View v)
        {
            base.OnAttachedTo(v);

            _panGestureRecognizer.PanUpdated += OnPanUpdated;

            v.GestureRecognizers.Add(_panGestureRecognizer);
        }

        protected override void OnDetachingFrom(View v)
        {
            base.OnDetachingFrom(v);

            _panGestureRecognizer.PanUpdated -= OnPanUpdated;

            _ = v.GestureRecognizers.Remove(_panGestureRecognizer);
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            View v = sender as View;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    {
                        _startPanDownTime = DateTime.Now;
                        break;
                    }
                case GestureStatus.Running:
                    {
                        _totalY = e.TotalY;

                        if (_totalY > 0)
                        {
                            if (Device.RuntimePlatform == Device.Android)
                            {
                                _ = v.TranslateTo(0, _totalY + v.TranslationY, 20, Easing.Linear);

                                _reachedEdge = _totalY + v.TranslationY > v.Height - ClosingEdge;
                            }

                            else
                            {
                                _ = v.TranslateTo(0, _totalY, 20, Easing.Linear);

                                _reachedEdge = _totalY > v.Height - ClosingEdge;
                            }
                        }
                        break;
                    }
                case GestureStatus.Completed:
                    {
                        _endPanDownTime = DateTimeOffset.Now;

                        if ((_endPanDownTime.Value.ToUnixTimeMilliseconds() - _startPanDownTime.Value.ToUnixTimeMilliseconds() < ClosingTimeInMs && _totalY > 0)
                            || _reachedEdge)
                        {
                            CloseAction?.Invoke();
                        }
                        else
                        {
                            _ = v.TranslateTo(0, 0, 20, Easing.Linear);
                        }
                        break;
                    }
                case GestureStatus.Canceled:
                    break;
                default:
                    break;
            }

            if (e.StatusType == GestureStatus.Completed || e.StatusType == GestureStatus.Canceled)
            {
                _startPanDownTime = null;
                _endPanDownTime = null;
            }
        }
    }
}
