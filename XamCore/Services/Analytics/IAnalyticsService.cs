using System;
using System.Collections.Generic;
using System.Reflection;

namespace XamCore.Services
{
    public interface IAnalyticsService
    {
        void Init();

        void TrackPage(string pageName);
        void TrackSelection(string itemName, string contentType);
        void TrackEvent(string eventId);
        void TrackEvent(string eventId, string paramName, string? value);
        void TrackEvent(string eventId, IDictionary<string, string?>? parameters);
        void TrackException(Exception ex);
        void TrackException(Exception ex, string paramName, string? value);
        void TrackException(Exception ex, IDictionary<string, string?>? parameters);

        void TrackNullReference(MethodBase invoker, Type targetType, string parameterName);
        void TrackInvalidType(MethodBase invoker, Type targetType, string parameterName);
    }
}


