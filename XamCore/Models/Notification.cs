using System;
using SQLite;

namespace XamCore.Models
{
    public class Notification
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? Title { get; set; }

        public string? Message { get; set; }

        [Indexed]
        public NotificationType? Type { get; set; }

        public string? NavigationLink { get; set; }

        [Indexed]
        public DateTimeOffset Timestamp { get; set; }

        public string ImagePath => Type switch {
            NotificationType.General => "icon_notification_general",
            _ => "icon_claims",
        };
    }
}
