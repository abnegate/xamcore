using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Firebase.Messaging;
using Unity;
using XamCore.Services;

namespace XamCore.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class NotificationBackgroundService : FirebaseMessagingService
    {
        [Dependency]
        public INotificationService? FirebaseNotificationService { get; set; }

        private readonly PushNotificationHandler? _handler;

        public override void OnMessageReceived(RemoteMessage message)
        {
            var parameters = new Dictionary<string, string>();
            var notification = message.GetNotification();

            if (notification is not null) {
                if (!string.IsNullOrEmpty(notification.Title)) {
                    parameters.Add("title", notification.Title);
                }
                if (!string.IsNullOrEmpty(notification.Body)) {
                    parameters.Add("body", notification.Body);
                }
            }

            parameters = parameters
                .Concat(message.Data.Where(kvp => !parameters.ContainsKey(kvp.Key)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            _handler?.OnReceive(ApplicationContext!, parameters);
        }

        public override async void OnNewToken(string token) =>
            await FirebaseNotificationService!.OnTokenRefresh(token);
    }
}