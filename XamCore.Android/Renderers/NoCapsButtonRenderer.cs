using Android.Content;
using XamCore.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button), typeof(NoCapsButtonRenderer))]
namespace XamCore.Droid
{
    public class NoCapsButtonRenderer : ButtonRenderer
    {
        public NoCapsButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            Control.SetAllCaps(false);
        }
    }

}