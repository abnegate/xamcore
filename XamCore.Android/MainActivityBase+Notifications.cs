using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Common;
using AndroidX.AppCompat.App;
using Unity;
using XamCore.Models;
using XamCore.Services;

namespace XamCore.Droid
{
    public partial class MainActivityBase
    {
        [Dependency]
        public INotificationService? FirebaseNotificationService { get; set; }

        private async Task HandlePotentialNotification(Intent? intent)
        {
            var options = GetLaunchOptions(intent);
            if (!options.LaunchedFromNotification) {
                return;
            }

            await FirebaseNotificationService
                !.OpenNotification(options.Notification);
        }

        private LaunchOptions GetLaunchOptions(Intent? intent)
        {
            var options = new LaunchOptions();

            if (intent?.Extras is not null &&
                intent.HasExtra("TODO: What's a common key")) {
                options.Notification = FirebaseNotificationService
                    ?.ParseNotification(intent.Extras.ToDictionary()!);
            }

            return options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success) {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode)) {
                    GoogleApiAvailability.Instance.GetErrorDialog(this, resultCode, 100).Show();
                } else {
                    new AlertDialog.Builder(this)
                        .SetTitle("Sorry")
                        .SetMessage("Google play services is not supported on this device, you will not receive push notifications.")
                        .SetPositiveButton("OK", (object sender, DialogClickEventArgs e) => {
                            (sender as AlertDialog)?.Dismiss();
                        })
                        .Show();
                }
                return false;
            }
            return true;
        }
    }
}
