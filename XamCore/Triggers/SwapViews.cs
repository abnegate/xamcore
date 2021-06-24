using Xamarin.Forms;

namespace XamCore.Triggers
{
    public class SwapViews : TriggerAction<VisualElement>
    {
        public VisualElement? From { get; set; }
        public VisualElement? To { get; set; }

        protected override void Invoke(VisualElement sender)
        {
            if (From is null || To is null) {
                return;
            }

            From.IsVisible = false;
            To.IsVisible = true;
        }
    }
}
