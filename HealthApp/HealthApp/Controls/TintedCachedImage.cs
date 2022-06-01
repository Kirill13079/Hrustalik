using FFImageLoading.Forms;
using FFImageLoading.Svg.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HealthApp.Controls
{
    public class TintedCachedImage : CachedImage
    {
        public static BindableProperty TintColorProperty = BindableProperty.Create(
            nameof(TintColor),
            typeof(Color),
            typeof(TintedCachedImage),
            Color.Transparent,
            propertyChanged: UpdateColor);

        public static BindableProperty SvgSourceProperty = BindableProperty.Create(
            nameof(TintColor), 
            typeof(string), 
            typeof(TintedCachedImage), 
            null, 
            propertyChanged: UpdateSvg);

        public Color TintColor
        {
            get => (Color)GetValue(TintColorProperty); 
            set => SetValue(TintColorProperty, value); 
        }

        public string SvgSource
        {
            get => (string)GetValue(SvgSourceProperty);
            set => SetValue(SvgSourceProperty, value);
        }

        private static void UpdateColor(BindableObject bindable, object oldColor, object newColor)
        {
            var oldcolor = (Color)oldColor;
            var newcolor = (Color)newColor;

            if (!oldcolor.Equals(newcolor))
            {
                var view = (TintedCachedImage)bindable;

                var transformations = new List<ITransformation>()
                {
                    new TintTransformation((int)(newcolor.R * 255), (int)(newcolor.G * 255), (int)(newcolor.B * 255), (int)(newcolor.A * 255))
                    {
                        EnableSolidColor = true
                    }
                };

                view.Transformations = transformations;
            }
        }

        private static void UpdateSvg(BindableObject bindable, object oldVal, object newVal)
        {
            var _instance = bindable as TintedCachedImage;

            _instance.Source = SvgImageSource.FromResource((string)newVal);
        }
    }
}
