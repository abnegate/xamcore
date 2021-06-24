using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamCore.View
{
    public class AnimatedGradientPage : ContentPage
    {
        public AnimatedGradientPage()
        {
        }

        protected async void AnimateBackground(RadialGradientBrush brush, VisualElement container)
        {
            double iterationSeed = new Random().NextDouble();
            double iterationEnd = new Random().NextDouble();

            void Forward(double input)
            {
                brush.Center = new Point(input, input * iterationSeed);
                brush.Radius = 1 + input * iterationSeed;
            }

            void Backward(double input)
            {
                brush.Center = new Point(input, input * iterationSeed);
                brush.Radius = 1 + input * iterationSeed;
            }

            while (true) {
                var start = iterationEnd;
                var midpoint = new Random().NextDouble();
                var length = new Random().Next(4000, 6000);

                iterationEnd = new Random().NextDouble();
                iterationSeed = new Random().NextDouble();

                container.Animate(
                    name: "forward",
                    callback: Forward,
                    start: start,
                    end: midpoint,
                    length: (uint)length,
                    easing: Easing.CubicInOut);

                await Task.Delay(length);

                container.Animate(
                    name: "backward",
                    callback: Backward,
                    start: midpoint,
                    end: iterationEnd,
                    length: (uint)length,
                    easing: Easing.CubicInOut);

                await Task.Delay(length);
            }
        }
    }
}