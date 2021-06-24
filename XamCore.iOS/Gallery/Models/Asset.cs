using System;
using System.Threading.Tasks;
using CoreGraphics;
using Photos;
using UIKit;

namespace XamCore.iOS
{
    public class Asset
    {
        public readonly PHAsset assetRef;

        public UIImage? Thumbnail { get; set; }
        public UIImage? Image { get; set; }

        public Asset(PHAsset asset)
        {
            assetRef = asset;
        }

        public void GetThumbnail(Action<UIImage> completionHandler)
        {
            var options = new PHImageRequestOptions {
                ResizeMode = PHImageRequestOptionsResizeMode.Exact,
                DeliveryMode = PHImageRequestOptionsDeliveryMode.HighQualityFormat,
                NetworkAccessAllowed = true
            };

            ImagePickerController.Instance?.CacheManager.RequestImageForAsset(
                assetRef,
                new CGSize(160, 160),
                PHImageContentMode.AspectFill,
                options,
                (result, info) => {
                    Thumbnail = result;
                    completionHandler(result);
                }
            );
        }

        public Task GetFullImage()
        {
            var options = new PHImageRequestOptions {
                ResizeMode = PHImageRequestOptionsResizeMode.None,
                DeliveryMode = PHImageRequestOptionsDeliveryMode.HighQualityFormat,
                NetworkAccessAllowed = true,
                Synchronous = true
            };

            var tcs = new TaskCompletionSource<bool>();

            ImagePickerController.Instance?.CacheManager.RequestImageForAsset(
                assetRef,
                new CGSize(2160, 3840),
                PHImageContentMode.AspectFill,
                options,
                (result, info) => {
                    tcs.TrySetResult(true);
                    Image = result;
                }
            );

            return tcs.Task;
        }
    }

}
