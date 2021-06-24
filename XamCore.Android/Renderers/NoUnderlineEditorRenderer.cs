using Android.Content;
using XamCore.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(NoUnderlineEditorRenderer))]
namespace XamCore.Droid
{
    public class NoUnderlineEditorRenderer : EditorRenderer
    {
        public NoUnderlineEditorRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement is null) {
                Control.SetBackgroundResource(Android.Resource.Color.Transparent);
            }
        }
    }
}
