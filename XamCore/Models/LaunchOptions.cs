using System;

namespace XamCore.Models
{
    public class LaunchOptions
    {
        public Notification? Notification { get; set; }

        public bool LaunchedFromNotification => Notification is not null;
    }
}
