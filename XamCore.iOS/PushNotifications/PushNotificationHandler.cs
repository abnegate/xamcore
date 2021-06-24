using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using XamCore.Services;

namespace XamCore.iOS
{
    public class PushNotificationHandler
    {
        private readonly INotificationService _notificationHandlerService;

        public PushNotificationHandler(INotificationService notificationHandlerService)
        {
            _notificationHandlerService = notificationHandlerService;
        }

        public async Task OnReceive(NSDictionary userInfo)
        {
            var data = GetParameters(userInfo);

            var notification = _notificationHandlerService.ParseNotification(data);

            await _notificationHandlerService.OpenNotification(notification);
        }

        private static IDictionary<string, string> GetParameters(NSDictionary data)
        {
            var parameters = new Dictionary<string, string>();
            var apsKey = new NSString("aps");
            var alertKey = new NSString("alert");

            foreach (var val in data) {
                if (val.Key.Equals(apsKey)) {
                    if (data.ValueForKey(apsKey) is NSDictionary aps) {
                        foreach (var apsVal in aps) {
                            if (apsVal.Value is NSDictionary) {
                                if (apsVal.Key.Equals(alertKey)) {
                                    foreach (var alertVal in apsVal.Value as NSDictionary ?? new()) {
                                        parameters.Add($"aps.alert.{alertVal.Key}", $"{alertVal.Value}");
                                    }
                                }
                            } else {
                                parameters.Add($"aps.{apsVal.Key}", $"{apsVal.Value}");
                            }
                        }
                    }
                } else {
                    parameters.Add($"{val.Key}", $"{val.Value}");
                }
            }
            return parameters;
        }
    }
}
