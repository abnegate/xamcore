using Firebase.Analytics;
using Foundation;
using XamCore.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

[assembly: Xamarin.Forms.Dependency(typeof(XamCore.iOS.AnalyticsService))]
namespace XamCore.iOS
{
    public class AnalyticsService : IAnalyticsService
    {
        public void Init()
        {
            Analytics.SetAnalyticsCollectionEnabled(true);
        }

        public void TrackPage(string pageName)
        {
            Analytics.SetScreenNameAndClass(pageName, null);
        }

        public void TrackSelection(string itemName, string contentType)
        {

        }

        public void TrackEvent(string eventId) =>
            TrackEvent(eventId, null);

        public void TrackEvent(string eventId, string paramName, string? value) =>
            TrackEvent(eventId, new Dictionary<string, string?> {
                { paramName, value }
            });

        public void TrackEvent(string eventId, IDictionary<string, string?>? parameters)
        {
            if (parameters is null) {
                Analytics.LogEvent(eventId, (Dictionary<object, object>?)null);
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters) {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(
                values.ToArray(),
                keys.ToArray(),
                keys.Count
            );

            Analytics.LogEvent(eventId, parametersDictionary);
        }

        public void TrackException(string e)
        {
        }

        public void TrackException(Exception ex) =>
            TrackException(ex, null);

        public void TrackException(Exception ex, string paramName, string? value) =>
            TrackException(ex, new Dictionary<string, string?> {
                { paramName, value }
            });

        public void TrackException(Exception ex, IDictionary<string, string?>? parameters)
        {
        }

        public void TrackInvalidType(
            MethodBase invoker,
            Type targetType,
            string parameterName)
        {
        }

        public void TrackNullReference(
            MethodBase invoker,
            Type targetType,
            string parameterName)
        {
        }
    }
}

