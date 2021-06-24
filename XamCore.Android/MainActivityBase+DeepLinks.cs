using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Extensions;
using Firebase.DynamicLinks;
using Unity;
using XamCore.Services;

namespace XamCore.Droid
{
    public partial class MainActivityBase
    {
        [Dependency]
        public IDeepLinkService? DeepLinkService { get; set; }

        private async Task HandlePotentialDynamicLink(Intent? intent)
        {
            var link = await FirebaseDynamicLinks.Instance.GetDynamicLink(intent);
            if (link is PendingDynamicLinkData linkResult
                && linkResult.Link is not null) {
                DeepLinkService?.HandleDeepLink(linkResult.Link.ToString()!);
            }
        }
    }
}
