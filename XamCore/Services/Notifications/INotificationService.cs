using System.Collections.Generic;
using System.Threading.Tasks;
using XamCore.Models;

namespace XamCore.Services
{
    public interface INotificationService : IPlatformNotificationService
    {
        Task OnTokenRefresh(string token);

        Task OnRevokeToken();

        Notification ParseNotification(IDictionary<string, string> data);

        Task OpenNotification(Notification? notification);
    }
}
