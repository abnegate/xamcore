using System;

using UIKit;

namespace XamCore.iOS.Dialogs
{
    public partial class InputDialogViewController : UIViewController
    {
        private readonly string _title;
        private readonly string _message;
        private readonly string _inputHint;
        private readonly string _okButtonLabel;
        private readonly string _cancelButtonLabel;

        private readonly Action<string?> _onResult;

        public InputDialogViewController(
            string title,
            string message,
            string? inputHint,
            string? okButtonLabel,
            string? cancelButtonLabel,
            Action<string?> onResult) : base("InputDialogViewController", null)
        {
            ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;

            _title = title;
            _message = message;
            _inputHint = inputHint ?? string.Empty;
            _okButtonLabel = okButtonLabel ?? string.Empty;
            _cancelButtonLabel = cancelButtonLabel ?? string.Empty;
            _onResult = onResult;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TitleLabel.Text = _title;
            MessageLabel.Text = _message;

            OkButton.SetTitle(_okButtonLabel, UIControlState.Normal);
            OkButton.TouchUpInside += (s, e) => {
                DismissViewController(true, () => _onResult(InputText?.Text ?? ""));
            };

            CancelButton.SetTitle(_cancelButtonLabel, UIControlState.Normal);
            CancelButton.TouchUpInside += (s, e) => {
                DismissViewController(true, () => _onResult(null));
            };

            View?.AddGestureRecognizer(
                new UITapGestureRecognizer(() => InputText.ResignFirstResponder())
            );
        }
    }
}

