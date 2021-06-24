using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Java.Lang;
using XamCore.Services;

namespace XamCore.Droid
{
    public class PushNotificationHandler
    {
        const string CHANNEL_ID = "Rothbury";
        const string CHANNEL_NAME = "Rothbury";
        const string CHANNEL_DESCRIPTION = "Rothbury Updates";

        private readonly INotificationService _notificationHandlerService;

        public PushNotificationHandler(INotificationService notificationHandlerService)
        {
            _notificationHandlerService = notificationHandlerService;
        }

        public void OnReceive(Context context, IDictionary<string, string> data)
        {
            var notificationManager = NotificationManager.FromContext(context);
            if (notificationManager is null) {
                return;
            }

            var pendingIntent = PendingIntent.GetActivity(
                context,
                0,
                GetIntentWithData(context, data),
                PendingIntentFlags.OneShot);

            var notification = _notificationHandlerService
                .ParseNotification(data);

            var notificationBuilder = new NotificationCompat.Builder(context, CHANNEL_ID)
                .SetSmallIcon(context.ApplicationInfo!.Icon)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Message)
                .SetContentIntent(pendingIntent)
                .SetWhen(JavaSystem.CurrentTimeMillis())
                .SetShowWhen(true)
                .SetAutoCancel(true);

            BuildChannel(notificationManager);

            notificationManager.Notify(0, notificationBuilder.Build());
        }

        private Intent GetIntentWithData(Context context, IDictionary<string, string> data)
        {
            var intent = new Intent(context, typeof(MainActivityBase));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtras(data!.ToBundle()!);
            return intent;
        }

        /// <summary>
        /// Builds the notification channel for Android 8+
        /// </summary>
        /// <param name="manager">Manager.</param>
        private void BuildChannel(NotificationManager manager)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
                var channel = new NotificationChannel(
                    CHANNEL_ID,
                    CHANNEL_NAME,
                    NotificationImportance.Default) {
                    Description = CHANNEL_DESCRIPTION,
                    LightColor = Android.Graphics.Color.Cyan
                };
                channel.SetShowBadge(true);
                manager.CreateNotificationChannel(channel);
            }
        }
    }
}
