using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using Unity;
using XamCore.Services;

namespace XamCore.iOS
{
    public partial class AppDelegateBase : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IMessagingDelegate
    {
        [Dependency]
        public IMessageService? MessageService { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Firebase.Core.App.Configure();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Xam.Plugin.WebView.iOS.FormsWebViewRenderer.Initialize();
            Xamarin.Forms.Forms.Init();
            LoadApplication(new XamCoreApplication(new PlatformInitializer()));

            //_pushHandler = new PushNotificationHandler();

#if AUTOMATED_TEST
            Xamarin.Calabash.Start();
#endif

            return base.FinishedLaunching(app, options);
        }
    }
}
