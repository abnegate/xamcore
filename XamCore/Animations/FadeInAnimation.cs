using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamCore.Animations
{
    public class FadeInAnimation : AnimationBase
    {
        public enum FadeDirection
        {
            Up,
            Down,
            None
        }

        public static readonly BindableProperty DirectionProperty = BindableProperty.Create(
            "Direction",
            typeof(FadeDirection),
            typeof(FadeInAnimation),
            FadeDirection.None,
            propertyChanged: (bindable, oldValue, newValue) => ((FadeInAnimation)bindable).Direction = (FadeDirection)newValue);

        public FadeDirection Direction
        {
            get { return (FadeDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target is null) {
                throw new NullReferenceException("Null Target property.");
            }

            return Task.Run(() => {
                Device.BeginInvokeOnMainThread(() => {
                    Target.Animate("FadeIn", FadeIn(), 16, Convert.ToUInt32(Duration));
                });
            });
        }

        protected override Task ResetAnimation()
        {
            if (Target is null) {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.FadeTo(0, 0, null);
        }

        internal Animation FadeIn()
        {
            var animation = new Animation();

            animation.WithConcurrent((f) => Target.Opacity = f, 0, 1, Xamarin.Forms.Easing.CubicOut);

            animation.WithConcurrent(
              (f) => Target.TranslationY = f,
              Direction != FadeDirection.None ? Target.TranslationY + ((Direction == FadeDirection.Up) ? 75 : -75) : Target.TranslationY,
              Target.TranslationY,
              Xamarin.Forms.Easing.CubicOut,
              0,
              1);

            return animation;
        }
    }
}
