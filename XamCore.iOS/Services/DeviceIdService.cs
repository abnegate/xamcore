using XamCore.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(XamCore.iOS.DeviceIdService))]
namespace XamCore.iOS
{
    public class DeviceIdService : IDeviceIdService
    {
        public string? Id =>
            UIDevice.CurrentDevice.IdentifierForVendor.AsString();
    }
}
