using System.Collections.Generic;
using System.Threading.Tasks;
using XamCore.iOS;
using XamCore.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformMediaService))]
namespace XamCore.iOS
{
    public class PlatformMediaService : IPlatformMediaService
    {
        public Task<IEnumerable<string?>?> SelectPictures(int max = 15)
        {
            var picker = ImagePickerController.Create(max);

            picker.PickImageTaskCompletionSource =
                new TaskCompletionSource<IEnumerable<string?>?>();

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (topController?.PresentedViewController is not null) {
                topController = topController.PresentedViewController;
            }
            topController?.PresentViewController(picker, true, null);

            return picker.PickImageTaskCompletionSource.Task;
        }
    }
}