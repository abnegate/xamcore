using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using NETAPI.Services;

namespace XamCore.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly Lazy<IAnalyticsService> _tracker;
        private readonly IApiService<ApiEnvironment, ApiEndpoint> _apiService;

        public AnalyticsService(
            IApiService<ApiEnvironment, ApiEndpoint> apiService,
            IDependencyService dependencyService)
        {
            _apiService = apiService;
            _tracker = new(() => dependencyService.Get<IAnalyticsService>());
        }

        public void Init() =>
            _tracker.Value?.Init();

        public void TrackPage(string pageName)
        {
            _tracker.Value?.TrackPage(pageName);

            Analytics.TrackEvent("screen_view", new Dictionary<string, string> {
                { "screen_name", pageName },
                { "environment", _apiService.Configuration.CurrentEnvironment.ToString() }
            });
        }

        public void TrackSelection(string itemName, string contentType)
        {
            _tracker.Value?.TrackSelection(itemName, contentType);
        }

        public void TrackEvent(string eventId)
        {
            _tracker.Value?.TrackEvent(eventId);

            Analytics.TrackEvent(eventId, new Dictionary<string, string> {
                { "environment", _apiService.Configuration.CurrentEnvironment.ToString() }
            });
        }

        public void TrackEvent(string eventId, string paramName, string? value)
        {
            _tracker.Value?.TrackEvent(eventId, paramName, value);

            Analytics.TrackEvent(eventId, new Dictionary<string, string> {
                { paramName, value ?? string.Empty },
                { "environment", _apiService.Configuration.CurrentEnvironment.ToString() }
            });
        }

        public void TrackEvent(string eventId, IDictionary<string, string?>? parameters)
        {
            parameters?.Add("environment", _apiService.Configuration.CurrentEnvironment.ToString());

            _tracker.Value?.TrackEvent(eventId, parameters);

            Analytics.TrackEvent(eventId, parameters);
        }

        public void TrackException(Exception ex) =>
            TrackException(ex, null);

        public void TrackException(Exception ex, string paramName, string? value) =>
            TrackException(ex, new Dictionary<string, string?> { { paramName, value ?? string.Empty } });

        public void TrackException(Exception ex, IDictionary<string, string?>? parameters)
        {
            (parameters ??= new Dictionary<string, string?>())
                .Add("environment", _apiService.Configuration.CurrentEnvironment.ToString());

            Crashes.TrackError(ex, parameters);
            _tracker.Value?.TrackException(ex);
        }

        public void TrackNullReference(
            MethodBase invoker,
            Type targetType,
            string parameterName)
        {
            var ex = new NullReferenceException($"{invoker.ReflectedType.Name}.{invoker.Name} initialized with null {parameterName}");
            Debug.Write(ex);
            TrackException(ex,
                new Dictionary<string, string?> {
                    { "TargetType", targetType.AssemblyQualifiedName }
                });
        }

        public void TrackInvalidType(
            MethodBase invoker,
            Type targetType,
            string parameterName)
        {
            var ex = new ArgumentException($"{invoker.ReflectedType.Name}.{invoker.Name} called with incorrect type of {parameterName}");
            Debug.Write(ex);
            TrackException(ex,
                new Dictionary<string, string?> {
                    { "TargetType", targetType.AssemblyQualifiedName }
                });
        }
    }
}
