using Android.OS;
using Xamarin.Forms;
using XamCore.Services;
using static Android.Provider.Settings;


[assembly: Dependency(typeof(XamCore.Droid.DeviceIdService))]
namespace XamCore.Droid
{
    public class DeviceIdService : IDeviceIdService
    {
        public string? Id
        {
            get
            {
                var secureId = Secure.GetString(
                    Android.App.Application.Context.ContentResolver,
                    Secure.AndroidId);

                if (!string.IsNullOrEmpty(secureId)) {
                    return secureId;
                }

                if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
#pragma warning disable XA0001 // Find issues with Android API usage
                    return Build.GetSerial();
#pragma warning restore XA0001
                }

#pragma warning disable CS0618 // Type or member is obsolete
                return Build.Serial;
#pragma warning restore CS0618
            }
        }
    }
}
