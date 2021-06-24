using System.Threading.Tasks;
using Firebase.CloudMessaging;
using XamCore.Services;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(XamCore.iOS.PlatformNotificationService))]
namespace XamCore.iOS
{
    public class PlatformNotificationService : IPlatformNotificationService
    {
        public async Task StartService()
        {
            var appDelegate = (AppDelegateBase)UIApplication.SharedApplication.Delegate;

            Messaging.SharedInstance.Delegate = appDelegate;

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0)) {
                UNUserNotificationCenter.Current.Delegate = appDelegate;

                var result = await UNUserNotificationCenter.Current.RequestAuthorizationAsync(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge);

                if (!result.Item1 || result.Item2 is not null) {
                    return;
                }
            } else {
                UIApplication.SharedApplication.RegisterUserNotificationSettings(UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge,
                    null));
            }
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }

        public Task<string?> GetToken() =>
            Task.FromResult(Messaging.SharedInstance.FcmToken);
    }
}
