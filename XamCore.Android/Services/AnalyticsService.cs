using System;
using System.Collections.Generic;
using System.Reflection;
using Android.OS;
using XamCore.Services;
using Firebase.Analytics;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(XamCore.Droid.AnalyticsService))]
namespace XamCore.Droid
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly FirebaseAnalytics _tracker = FirebaseAnalytics
            .GetInstance(Android.App.Application.Context);

        public void Init()
        {
            _tracker.SetAnalyticsCollectionEnabled(true);
        }

        public void TrackPage(string pageName)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _tracker.SetCurrentScreen(Platform.CurrentActivity, pageName, null);
#pragma warning restore CS0618
        }
        public void TrackSelection(string itemName, string contentType)
        {
            var bundle = new Bundle();

            bundle.PutString(FirebaseAnalytics.Param.ItemName, itemName);
            bundle.PutString(FirebaseAnalytics.Param.ContentType, contentType);

            _tracker.LogEvent(FirebaseAnalytics.Event.SelectContent, bundle);
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
                _tracker.LogEvent(eventId, null);
                return;
            }

            _tracker.LogEvent(eventId, parameters.ToBundle());
        }

        public void TrackException(string ex)
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

