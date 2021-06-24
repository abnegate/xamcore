using Android.Content;
using XamCore.Controls;
using XamCore.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DefaultTextEntry), typeof(DefaultTextEntryRenderer))]
namespace XamCore.Droid
{
    public class DefaultTextEntryRenderer : NoUnderlineEntryRenderer
    {
        public DefaultTextEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement is null) {
                return;
            }

            e.NewElement.Focused += (sender, evt) => {
                var view = (DefaultTextEntry)Element;
                if (view?.DefaultTextOnSelected is not null
                    && view?.Text is null) {
                    view.Text = view.DefaultTextOnSelected;
                }
            };
        }
    }
}
