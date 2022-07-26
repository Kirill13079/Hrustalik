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
            propertyName: nameof(TintColor),
            returnType: typeof(Color),
            declaringType: typeof(TintedCachedImage),
            defaultValue: Color.Transparent,
            propertyChanged: UpdateColor);

        public static BindableProperty SvgSourceProperty = BindableProperty.Create(
            propertyName: nameof(TintColor),
            returnType: typeof(string),
            declaringType: typeof(TintedCachedImage),
            defaultValue: null,
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
            Color oldcolor = (Color)oldColor;
            Color newcolor = (Color)newColor;

            if (!oldcolor.Equals(newcolor))
            {
                TintedCachedImage view = (TintedCachedImage)bindable;

                List<ITransformation> transformations = new List<ITransformation>()
                {
                    new TintTransformation(
                        r: (int)(newcolor.R * 255),
                        g: (int)(newcolor.G * 255),
                        b: (int)(newcolor.B * 255),
                        a: (int)(newcolor.A * 255))
                    {
                        EnableSolidColor = true
                    }
                };

                view.Transformations = transformations;
            }
        }

        private static void UpdateSvg(BindableObject bindable, object oldVal, object newVal)
        {
            TintedCachedImage _instance = bindable as TintedCachedImage;

            _instance.Source = SvgImageSource.FromResource(resource: (string)newVal);
        }
    }
}
