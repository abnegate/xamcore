using System;
using Xamarin.Forms;

namespace XamCore.Triggers
{
    public class SetOpacity : TriggerAction<VisualElement>
    {
        public VisualElement? Target { get; set; }
        public double Value { get; set; }

        protected override void Invoke(VisualElement sender)
        {
            if (Target is null) {
                return;
            }

            Target.Opacity = 0;
        }
    }
}
