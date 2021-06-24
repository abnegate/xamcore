using System;
using Foundation;
using UIKit;

namespace XamCore.iOS.Dialogs
{
    public partial class FormattedDialogViewController : UIViewController
    {
        private readonly string _title;
        private readonly NSMutableAttributedString _message;
        private readonly string _okButtonLabel;
        private readonly string _cancelButtonLabel;

        private readonly Action<bool> _onResult;

        public FormattedDialogViewController(
            string title,
            NSMutableAttributedString message,
            string? okButtonLabel,
            string? cancelButtonLabel,
            Action<bool> onResult) : base("FormattedDialogViewController", null)
        {
            ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;

            _title = title;
            _message = message;
            _okButtonLabel = okButtonLabel ?? string.Empty;
            _cancelButtonLabel = cancelButtonLabel ?? string.Empty;
            _onResult = onResult;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TitleLabel.Text = _title;
            MessageLabel.AttributedText = _message;

            OkButton.SetTitle(_okButtonLabel, UIControlState.Normal);
            OkButton.TouchUpInside += (s, e) => {
                DismissViewController(true, () => _onResult(true));
            };

            CancelButton.SetTitle(_cancelButtonLabel, UIControlState.Normal);
            CancelButton.TouchUpInside += (s, e) => {
                DismissViewController(true, () => _onResult(false));
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

