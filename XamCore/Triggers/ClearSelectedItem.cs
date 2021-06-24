using System;
using Xamarin.Forms;

namespace XamCore.Triggers
{
    public class ClearSelectedItem : TriggerAction<VisualElement>
    {
        public CollectionView? Target { get; set; }

        protected override void Invoke(VisualElement sender)
        {
            if (Target is null) {
                return;
            }

            Target.SelectedItem = null;
        }
    }
}
