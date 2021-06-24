using Android.Content;
using XamCore.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(NoUnderlineEntryRenderer))]
namespace XamCore.Droid
{
    public class NoUnderlineEntryRenderer : EntryRenderer
    {
        public NoUnderlineEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement is null) {
                Control.SetBackgroundResource(Android.Resource.Color.Transparent);
            }
        }
    }
}
