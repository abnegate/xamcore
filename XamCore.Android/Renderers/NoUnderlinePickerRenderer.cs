using Android.Content;
using XamCore.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(NoUnderlinePickerRenderer))]
namespace XamCore.Droid
{
    public class NoUnderlinePickerRenderer : PickerRenderer
    {
        public NoUnderlinePickerRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement is null) {
                Control.SetBackgroundResource(Android.Resource.Color.Transparent);
            }
        }
    }
}
