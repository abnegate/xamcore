using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamCore.Animations
{
    public class FadeOutAnimation : AnimationBase
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
            typeof(FadeOutAnimation),
            FadeDirection.None,
            propertyChanged: (bindable, oldValue, newValue) => ((FadeOutAnimation)bindable).Direction = (FadeDirection)newValue);

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
                    Target.Animate("FadeOut", FadeOut(), 16, Convert.ToUInt32(Duration));
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

        internal Animation FadeOut()
        {
            var animation = new Animation();

            animation.WithConcurrent(
                 (f) => Target.Opacity = f,
                 1, 0);

            animation.WithConcurrent(
                  (f) => Target.TranslationY = f,
                  Target.TranslationY,
                  Direction != FadeDirection.None ? Target.TranslationY + ((Direction == FadeDirection.Up) ? 50 : -50) : Target.TranslationY);

            return animation;
        }
    }
}
