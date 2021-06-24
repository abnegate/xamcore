using Android.Content;
using AndroidX.AppCompat.Widget;
using XamCore.Droid;
using XamCore.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TransparentNavigationPage), typeof(TransprentNavPageRenderer))]
namespace XamCore.Droid
{
    public class TransprentNavPageRenderer : NavigationPageRenderer
    {
        public TransprentNavPageRenderer(Context context) : base(context)
        {
        }

        IPageController PageController => Element;
        TransparentNavigationPage? CustomNavigationPage => Element as TransparentNavigationPage;

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            CustomNavigationPage!.IgnoreLayoutChange = true;
            base.OnLayout(changed, l, t, r, b);
            CustomNavigationPage.IgnoreLayoutChange = false;

            int containerHeight = b - t;

            PageController.ContainerArea = new Rectangle(0, 0, Context.FromPixels(r - l), Context.FromPixels(containerHeight));

            for (var i = 0; i < ChildCount; i++) {
                var child = GetChildAt(i);
                if (child is Toolbar) {
                    continue;
                }
                child?.Layout(0, 0, r, b);
            }
        }
    }
}