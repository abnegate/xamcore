using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamCore.Animations
{
    public class TranslateAnimation : AnimationBase
    {
        public static readonly BindableProperty TranslateXProperty = BindableProperty.Create(
            nameof(TranslateX),
            typeof(double),
            typeof(TranslateAnimation),
            0.0d,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((TranslateAnimation)bindable).TranslateX = (double)newValue);

        public static readonly BindableProperty TranslateYProperty = BindableProperty.Create(
            nameof(TranslateY),
            typeof(double),
            typeof(TranslateAnimation),
            0.0d,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((TranslateAnimation)bindable).TranslateY = (double)newValue);

        public double TranslateX
        {
            get => (double)GetValue(TranslateXProperty);
            set => SetValue(TranslateXProperty, value);
        }

        public double TranslateY
        {
            get => (double)GetValue(TranslateYProperty);
            set => SetValue(TranslateYProperty, value);
        }

        protected override Task BeginAnimation()
        {
            if (Target is null) {
                throw new NullReferenceException("Null Target property.");
            }
            return Target.TranslateTo(
                Target.TranslationX + TranslateX,
                Target.TranslationY + TranslateY,
                Convert.ToUInt32(Duration),
                EasingHelper.GetEasing(Easing)
            );
        }

        protected override Task ResetAnimation()
        {
            if (Target is null) {
                throw new NullReferenceException("Null Target property.");
            }

            ViewExtensions.CancelAnimations(Target);

            return Task.CompletedTask;
        }
    }
}
