using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamCore.Extensions;
using XamCore.Models;
using XamCore.View;
using NETAPI.Services;
using Prism.Navigation;

namespace XamCore.Services
{

    public class NotificationService : INotificationService
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IApiService<ApiEnvironment, ApiEndpoint> _apiService;
        private readonly IDatabaseService _databaseService;
        private readonly INavigationService _navigationService;
        private readonly IKeyValueService _keyValueService;
        private readonly IDeviceIdService _deviceIdService;

        private readonly Lazy<IPlatformNotificationService> _platformNotificationService;


        public NotificationService(
            IAnalyticsService analyticsService,
            IApiService<ApiEnvironment, ApiEndpoint> apiService,
            IDatabaseService databaseService,
            INavigationService navigationService,
            IKeyValueService keyValueService,
            IDeviceIdService deviceIdService,
            IDependencyService dependencyService)
        {
            _analyticsService = analyticsService;
            _apiService = apiService;
            _databaseService = databaseService;
            _navigationService = navigationService;
            _keyValueService = keyValueService;
            _deviceIdService = deviceIdService;

            _platformNotificationService = new(() =>
                dependencyService.Get<IPlatformNotificationService>());
        }

        public async Task StartService()
        {
            await _platformNotificationService.Value.StartService();

            await OnTokenRefresh(await GetToken());
        }

        public Task<string?> GetToken() =>
            _platformNotificationService.Value.GetToken();

        public Notification ParseNotification(IDictionary<string, string> data)
        {
            var notification = new Notification();

            return notification;
        }

        public async Task OnTokenRefresh(string? token)
        {
            if (token is null) {
                return;
            }

            _keyValueService.Set(
                "fcm_token",
                token);

            if (await _keyValueService.GetSecureAsync<string?>(
                "user_token") is not null) {
                await PostFcmToken(token);
            }
        }

        public Task OnRevokeToken()
        {
            //await _apiService.TryPost<RegisterPushTokenRequest, string>(
            //    ApiEndpoint.RegisterPushToken,
            //    new(null, _deviceIdService.Id));
            return Task.CompletedTask;
        }

        public async Task OpenNotification(Notification? notification)
        {
            await _databaseService.InsertAsync(notification);

            await _navigationService.NavigateAsync($@"
                /{nameof(MenuPage)}
                /{notification?.NavigationLink?.ToNavigationTypeString()}
                /{notification?.NavigationLink}".TrimAllWhitespace());
        }

        private Task PostFcmToken(string token)
        {
            //await _apiService.TryPost<RegisterPushTokenRequest, string>(
            //    ApiEndpoint.RegisterPushToken,
            //    new(token, _deviceIdService.Id));
            return Task.CompletedTask;
        }
    }
}
