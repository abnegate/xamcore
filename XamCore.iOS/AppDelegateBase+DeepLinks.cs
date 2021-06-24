using Firebase.DynamicLinks;
using Foundation;
using XamCore.Services;
using UIKit;
using Unity;

namespace XamCore.iOS
{
    public partial class AppDelegateBase
    {
        [Dependency]
        public IDeepLinkService? DeepLinkService { get; set; }


        public override bool ContinueUserActivity(
            UIApplication application,
            NSUserActivity userActivity,
            UIApplicationRestorationHandler completionHandler) =>
            DynamicLinks.SharedInstance?.HandleUniversalLink(
                userActivity.WebPageUrl!,
                (link, error) => {
                    if (error is not null || link?.Url is null) {
                        return;
                    }
                    DeepLinkService?.HandleDeepLink(link.Url.ToString());
                }) ?? false;

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var openOptions = new UIApplicationOpenUrlOptions(options);
            return OpenUrl(
                app,
                url,
                openOptions.SourceApplication ?? string.Empty,
                openOptions.Annotation ?? new NSObject());
        }

        public override bool OpenUrl(
            UIApplication application,
            NSUrl url,
            string sourceApplication,
            NSObject annotation)
        {
            var dynamicLink = DynamicLinks.SharedInstance?.FromCustomSchemeUrl(url);
            if (dynamicLink?.Url is not null) {
                DeepLinkService?.HandleDeepLink(dynamicLink.Url.ToString());
                return true;
            }
            return false;
        }
    }
}
