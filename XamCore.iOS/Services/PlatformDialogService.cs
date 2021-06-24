using System.Threading.Tasks;
using Foundation;
using UIKit;
using XamCore.iOS.Dialogs;
using XamCore.Services;

[assembly: Xamarin.Forms.Dependency(typeof(XamCore.iOS.PlatformDialogService))]
namespace XamCore.iOS
{
    public class PlatformDialogService : IPlatformDialogService
    {
        public Task<bool> ShowBoldFormattedConfirmAsync(
            string title,
            string messageFormatString,
            string[] boldFormatArgs,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null)
        {
            var font = UIFont.BoldSystemFontOfSize(12);
            var formatted = string.Format(messageFormatString, boldFormatArgs);

            var attributedMessage = new NSMutableAttributedString(formatted);
            var style = new NSMutableParagraphStyle {
                Alignment = UITextAlignment.Center
            };

            foreach (var arg in boldFormatArgs) {
                var start = formatted.IndexOf(arg);
                var range = new NSRange(start, arg.Length);
                attributedMessage.AddAttribute(UIStringAttributeKey.Font, font, range);
            }

            attributedMessage.AddAttribute(
                UIStringAttributeKey.ParagraphStyle,
                style,
                new NSRange(0, formatted.Length));

            var completionSource = new TaskCompletionSource<bool>();

            var controller = new FormattedDialogViewController(
                title,
                attributedMessage,
                okButtonLabel,
                cancelButtonLabel,
                result => {
                    completionSource.SetResult(result);
                });

            UIApplication.SharedApplication.KeyWindow.RootViewController
                ?.PresentViewController(controller, true, null);

            return completionSource.Task;
        }

        public Task<string?> ShowInputDialogAsync(
            string title,
            string message,
            string? inputHint,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null)
        {
            var completionSource = new TaskCompletionSource<string?>();

            var controller = new InputDialogViewController(
                title,
                message,
                inputHint,
                okButtonLabel,
                cancelButtonLabel,
                result => {
                    completionSource.SetResult(result);
                });

            UIApplication.SharedApplication.KeyWindow.RootViewController
                ?.PresentViewController(controller, true, null);

            return completionSource.Task;
        }
    }
}
