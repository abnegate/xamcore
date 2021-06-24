using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamCore.Animations
{
    public class BounceAnimation : AnimationBase
    {
        public static readonly BindableProperty MaxScaleProperty = BindableProperty.Create(
            nameof(MaxScale),
            typeof(double),
            typeof(BounceAnimation),
            1.5);

        public static readonly BindableProperty MinScaleProperty = BindableProperty.Create(
            nameof(MinScale),
            typeof(double),
            typeof(BounceAnimation),
            1.5);

        public double MaxScale
        {
            get => (double)GetValue(MaxScaleProperty);
            set => SetValue(MaxScaleProperty, value);
        }

        public double MinScale
        {
            get => (double)GetValue(MinScaleProperty);
            set => SetValue(MinScaleProperty, value);
        }

        protected override Task BeginAnimation()
        {
            if (Target is null) {
                throw new NullReferenceException("Null Target property.");
            }

            return Task.Run(() => {
                Device.BeginInvokeOnMainThread(() => {
                    Target.Animate(
                        nameof(Bounce),
                        Bounce(),
                        16,
                        Convert.ToUInt32(Duration),
                        EasingHelper.GetEasing(Easing));
                });
            });
        }

        protected override Task ResetAnimation()
        {
            if (Target is null) {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.ScaleTo(1, 0, null);
        }

        internal Animation Bounce()
        {
            var animation = new Animation();

            animation.WithConcurrent(
                async (f) => await Target.ScaleTo(
                    f,
                    500,
                    null),
                1,
                MaxScale);

            animation.WithConcurrent(
                async (f) => await Target.ScaleTo(
                    f,
                    500,
                    EasingHelper.GetEasing(Easing)),
                MaxScale,
                1);

            return animation;
        }
    }
}
