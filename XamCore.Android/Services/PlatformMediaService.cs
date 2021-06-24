using Xamarin.Forms;
using XamCore.Droid;
using Xamarin.Essentials;
using XamCore.Services;
using Android.Content;
using System.Threading.Tasks;
using System.Collections.Generic;

[assembly: Dependency(typeof(PlatformMediaService))]
namespace XamCore.Droid
{
    public class PlatformMediaService : IPlatformMediaService
    {
        public Task<IEnumerable<string?>?> SelectPictures(int max = 15)
        {
            var mainActivity = Platform.CurrentActivity as MainActivityBase;

            mainActivity!.PickImageTaskCompletionSource =
                new TaskCompletionSource<IEnumerable<string?>?>();

            var intent = new Intent(
                mainActivity,
                typeof(ImagePickerActivity));

            Platform.CurrentActivity.StartActivityForResult(
                intent,
                Constants.Android.SelectImageRequestCode);

            return mainActivity.PickImageTaskCompletionSource.Task;
        }
    }
}
