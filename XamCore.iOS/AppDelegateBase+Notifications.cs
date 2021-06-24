using System;
using System.Linq;
using Firebase.CloudMessaging;
using Foundation;
using XamCore.Models;
using XamCore.Services;
using UIKit;
using UserNotifications;
using Unity;

namespace XamCore.iOS
{
    public partial class AppDelegateBase : IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        [Dependency]
        public INotificationService FirebaseNotificationService { get; set; }


        private readonly PushNotificationHandler _pushHandler;

        [Export("messaging:didReceiveRegistrationToken:")]
        public async void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            await FirebaseNotificationService
                    .OnTokenRefresh(fcmToken);
        }

        public override async void DidReceiveRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler)
        {
            await _pushHandler.OnReceive(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }

        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public async void DidReceiveNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            Action completionHandler)
        {
            await _pushHandler.OnReceive(response.Notification.Request.Content.UserInfo);

            completionHandler();
        }

        // To receive notifications in foreground on iOS 10 devices.
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }

        private LaunchOptions GetLaunchOptions(NSDictionary options)
        {
            var launchOptions = new LaunchOptions();

            if (options is null ||
                !options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey)) {
                return launchOptions;
            }

            var nsDictionary = options[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
            var data = nsDictionary.ToDictionary(
                kvp => $"{kvp.Key}",
                kvp => $"{kvp.Value}");

            //launchOptions.Notification = DependencyContainer
            //    .Resolve<INotificationService>()
            //    .ParseNotification(data);

            return launchOptions;
        }
    }
}
