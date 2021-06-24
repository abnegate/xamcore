using System.Threading.Tasks;
using Android.Gms.Extensions;
using XamCore.Services;
using Xamarin.Forms;
using Firebase.Messaging;
using Android.OS;
using System;

[assembly: Dependency(typeof(XamCore.Droid.PlatformNotificationService))]
namespace XamCore.Droid
{
    public class PlatformNotificationService : IPlatformNotificationService
    {
        private readonly IKeyValueService _keyValueService;

        public PlatformNotificationService(
            IKeyValueService keyValueService)
        {
            _keyValueService = keyValueService;
        }

        public Task StartService()
        {
            // Doesn't need to do anything
            return Task.CompletedTask;
        }

        public async Task<string?> GetToken()
        {
            try {
                string token = string.Empty;

                var tokenResult = await FirebaseMessaging.Instance
                    .GetToken()
                    .AsAsync<Java.Lang.String>();

                if (tokenResult is not null
                    && (token = tokenResult.ToString()) is not null) {
                    return token;
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return _keyValueService.Get<string?>("fcm_token");
        }
    }
}
