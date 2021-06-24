using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Photos;
using UIKit;

namespace XamCore.iOS
{
    public class ImagePickerController : UINavigationController
    {
        public static ImagePickerController? Instance { get; private set; }

        public PHCachingImageManager CacheManager { get; set; } = new PHCachingImageManager();

        public List<Asset> SelectedAssets { get; set; } = new List<Asset>();

        public int MaximumImagesCount { get; set; }

        public TaskCompletionSource<IEnumerable<string?>?>? PickImageTaskCompletionSource;

        private ImagePickerController(int maxImages) : base(new AlbumTableController())
        {
            MaximumImagesCount = maxImages;
        }

        public static ImagePickerController Create(int maxImages)
        {
            Instance = new ImagePickerController(maxImages);

            return Instance;
        }

        /// <summary>
        /// Called when users selects done.
        /// </summary>
        public async Task Done()
        {
            var paths = new ConcurrentQueue<string?>();

            await Task.WhenAll(
                SelectedAssets
                    .AsParallel()
                    .Select(async asset => {
                        paths.Enqueue(await GetPathToImage(
                            asset.Image,
                            asset.Image!.GetHashCode().ToString()));
                    })
                    .ToList());

            PickImageTaskCompletionSource?.SetResult(paths.ToList());
            DismissViewController(true, null);
        }

        /// <summary>
        /// Called when users select cancel.
        /// </summary>
        public void CancelledPicker()
        {
            PickImageTaskCompletionSource?.SetResult(null);
            DismissViewController(true, null);
        }

        private async Task<string?> GetPathToImage(UIImage? image, string? name)
        {
            // Ignore corrupt images
            if (image is null ||
                image.CGImage is null ||
                image.CGImage.Width == 0 ||
                image.CGImage.Height == 0) {
                return null;
            }

            var directory = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);

            var jpgFilename = Path.Combine(directory, name) + ".jpg";

            if (File.Exists(jpgFilename)) {
                System.Diagnostics.Debug.WriteLine("file exists " + jpgFilename);
                return jpgFilename;
            }

            // Calculate new size
            var currentWidth = image.CGImage.Width;
            var currentHeight = image.CGImage.Height;
            var imageQualityPercent = 2000 * (1f / currentWidth) > 1 ? 1 : 2000 * (1f / currentWidth);

            float width = image.CGImage.Width * imageQualityPercent;
            float height = image.CGImage.Height * imageQualityPercent;

            // Get resized image
            UIImage resizedImage = await ResizeImageWithAspectRatio(image, width, height);
            NSData imgData = resizedImage.AsJPEG();

            // Write image to file
            if (imgData.Save(jpgFilename, false, out NSError err)) {
                System.Diagnostics.Debug.WriteLine("saved as " + jpgFilename);
            } else {
                System.Diagnostics.Debug.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
            }

            // Recycle image memory
            resizedImage.Dispose();
            imgData.Dispose();

            return jpgFilename;
        }

        private Task<UIImage> ResizeImageWithAspectRatio(
            UIImage sourceImage,
            float maxWidth,
            float maxHeight) => Task.Run(() => {
                var sourceSize = sourceImage.Size;
                var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                if (maxResizeFactor > 1) {
                    return sourceImage;
                }

                var width = maxResizeFactor * sourceSize.Width;
                var height = maxResizeFactor * sourceSize.Height;

                UIGraphics.BeginImageContext(new CGSize(width, height));
                sourceImage.Draw(new CGRect(0, 0, width, height));
                var resultImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                return resultImage;
            });
    }
}