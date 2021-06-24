using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using System.Threading.Tasks;
using System.Collections.Generic;
using XamCore.Services;
using Unity;

namespace XamCore.Droid
{
    [Activity(
        Label = "XamCore",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize
            | ConfigChanges.Orientation
            | ConfigChanges.UiMode
            | ConfigChanges.ScreenLayout
            | ConfigChanges.SmallestScreenSize)]
    public partial class MainActivityBase : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        [Dependency]
        public IMessageService? MessageService { get; set; }

        public TaskCompletionSource<IEnumerable<string?>?>? PickImageTaskCompletionSource { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Xam.Plugin.WebView.Droid.FormsWebViewRenderer.Initialize();
            Acr.UserDialogs.UserDialogs.Init(this);
            LoadApplication(new XamCoreApplication(new PlatformInitializer()));

#pragma warning disable CS4014
            HandlePotentialDynamicLink(Intent);
            HandlePotentialNotification(Intent);
#pragma warning restore CS4014

            IsPlayServicesAvailable();

        }

        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(
            int requestCode,
            Result resultCode,
            Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == Constants.Android.SelectImageRequestCode) {
                if (resultCode == Result.Ok) {
                    PickImageTaskCompletionSource?
                        .SetResult(data?.GetStringArrayListExtra(Constants.Android.ImagePathsExtra));
                    return;
                }
                PickImageTaskCompletionSource?.SetResult(null);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
#pragma warning disable CS4014
            HandlePotentialDynamicLink(intent);
            HandlePotentialNotification(intent);
#pragma warning restore CS4014
        }

        public override void OnBackPressed()
        {
            MessageService?.Send(MessageType.AndroidBackPress);
        }
    }
}