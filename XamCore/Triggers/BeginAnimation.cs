using XamCore.Animations;
using Xamarin.Forms;

namespace XamCore.Triggers
{
    public class BeginAnimation : TriggerAction<VisualElement>
    {
        public enum AnimationAction
        {
            Start,
            Stop,
            Reset,
            Restart
        }

        public AnimationAction Action { get; set; } = AnimationAction.Restart;
        public AnimationBase? Animation { get; set; }

        protected override async void Invoke(VisualElement sender)
        {
            if (Animation is not null) {
                switch (Action) {
                    case AnimationAction.Start:
                        await Animation.Begin();
                        break;
                    case AnimationAction.Stop:
                        ViewExtensions.CancelAnimations(sender);
                        break;
                    case AnimationAction.Reset:
                        await Animation.Reset();
                        break;
                    case AnimationAction.Restart:
                        await Animation.Reset();
                        await Animation.Begin();
                        break;
                }
            }
        }
    }
}