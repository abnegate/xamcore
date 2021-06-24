using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using XamCore.Droid;
using XamCore.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using AndroidColor = Android.Graphics.Color;
using Platform = Xamarin.Essentials.Platform;
using XamarinColor = Xamarin.Forms.Color;

[assembly: Dependency(typeof(PlatformDialogService))]
namespace XamCore.Droid
{
    public class PlatformDialogService : IPlatformDialogService
    {
        private readonly Lazy<AndroidColor> _orangeColor =
            new(((XamarinColor)Application.Current.Resources["OrangeColor"]).ToAndroid());

        public Task<bool> ShowBoldFormattedConfirmAsync(
            string title,
            string messageFormatString,
            string[] boldFormatArgs,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null)
        {
            for (int i = 0; i < boldFormatArgs.Length; i++) {
                boldFormatArgs[i] = $"<b>{boldFormatArgs[i]}</b>";
            }

            okButtonLabel ??= "Ok";
            cancelButtonLabel ??= "Cancel";

            var formatted = string.Format(messageFormatString, boldFormatArgs);

            ISpanned toDisplay;

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N) {
                toDisplay = Html.FromHtml(formatted, FromHtmlOptions.ModeLegacy);
            } else {
                toDisplay = Html.FromHtml(formatted);
#pragma warning restore CS0618
#pragma warning restore CS8600 
            }

            var completionSource = new TaskCompletionSource<bool>();

            var dialog = new AlertDialog.Builder(Platform.CurrentActivity)
                .SetTitle(title)
                .SetMessage(toDisplay)
                .SetPositiveButton(okButtonLabel, (sender, args) => {
                    completionSource.SetResult(true);
                })
                .SetNegativeButton(cancelButtonLabel, (sender, args) => {
                    completionSource.SetResult(false);
                })
                .SetOnCancelListener(new OnDialogBackPressListener(() =>
                    completionSource.SetResult(false)))
                .Create();

            dialog.SetCanceledOnTouchOutside(false);
            dialog.Show();

            return completionSource.Task;
        }

        public Task<string?> ShowInputDialogAsync(
            string title,
            string message,
            string? inputHint,
            string? okButtonLabel = null,
            string? cancelButtonLabel = null)
        {
            var view = LayoutInflater
                .From(Android.App.Application.Context)
                ?.Inflate(Resource.Layout.dialog_input, null);

            var messageText = view?.FindViewById<TextView>(Resource.Id.tvMessage);
            var editText = view?.FindViewById<EditText>(Resource.Id.etInput);

            if (messageText is not null) {
                messageText.Text = message;
            }
            if (editText is not null) {
                editText.Hint = inputHint;
            }

            ViewCompat.SetBackgroundTintList(
                editText,
                ColorStateList.ValueOf(_orangeColor.Value));

            var completionSource = new TaskCompletionSource<string?>();

            var dialog = new AlertDialog.Builder(Platform.CurrentActivity)
                .SetTitle(title)
                .SetView(view)
                .SetPositiveButton(okButtonLabel, (sender, args) => {
                    completionSource.SetResult(editText?.Text ?? "");
                })
                .SetNegativeButton(cancelButtonLabel, (sender, args) => {
                    completionSource.SetResult(null);
                })
                .SetOnCancelListener(new OnDialogBackPressListener(() =>
                    completionSource.SetResult(null)))
                .Create();

            dialog.SetCanceledOnTouchOutside(false);
            dialog.Show();

            return completionSource.Task;
        }
    }

    internal class OnDialogBackPressListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
    {
        private readonly Action _onCancel;

        public OnDialogBackPressListener(Action onCancel) =>
            _onCancel = onCancel;

        public void OnCancel(IDialogInterface? dialog) =>
            _onCancel?.Invoke();
    }
}
