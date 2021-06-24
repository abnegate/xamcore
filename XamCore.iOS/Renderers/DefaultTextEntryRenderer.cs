using System.ComponentModel;
using XamCore.Controls;
using XamCore.iOS.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(DefaultTextEntry), typeof(DefaultTextEntryRenderer))]
namespace XamCore.iOS.Renderers
{
    public class DefaultTextEntryRenderer : NoBorderEntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (DefaultTextEntry)Element;

            if (view?.DefaultTextOnSelected is not null
                && view?.IsFocused == true
                && view?.Text is null) {
                view!.Text = view.DefaultTextOnSelected;
            }
        }
    }
}
