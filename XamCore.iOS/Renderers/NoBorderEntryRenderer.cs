using System;
using XamCore.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(NoBorderEntryRenderer))]
namespace XamCore.iOS.Renderers
{
    public class NoBorderEntryRenderer : EntryRenderer
    {
        public NoBorderEntryRenderer() : base() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement is null) {
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}
