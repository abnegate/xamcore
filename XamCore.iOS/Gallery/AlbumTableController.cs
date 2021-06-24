using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Photos;
using UIKit;

namespace XamCore.iOS
{
    public class AlbumTableController : UITableViewController
    {
        readonly List<AssetCollection> AssetGroups = new();

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.Title = "Choose Album";

            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            cancelButton.Clicked += CancelClicked;
            NavigationItem.RightBarButtonItem = cancelButton;

            AssetGroups.Clear();

            bool allowedAccess = await CheckPermissions();

            GetAllPhotosAlbum();
            GetSmartAlbums();
            GetUserAlbums();

            if (allowedAccess) {
                TableView.ReloadData();
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            if (IsMovingFromParentViewController || IsBeingDismissed) {
                NavigationItem.RightBarButtonItem!.Clicked -= CancelClicked;
            }
        }

        /// <summary>
        /// Checks the gallery permissions.
        /// </summary>
        /// <returns>The permissions.</returns>
        public async Task<bool> CheckPermissions()
        {
            switch (PHPhotoLibrary.AuthorizationStatus) {
                case PHAuthorizationStatus.NotDetermined:
                    await PHPhotoLibrary.RequestAuthorizationAsync();
                    break;
                case PHAuthorizationStatus.Authorized:
                    return true;
                case PHAuthorizationStatus.Denied:
                    return false;
            }

            return await CheckPermissions();
        }

        /// <summary>
        /// Gets all photos.
        /// </summary>
        private void GetAllPhotosAlbum()
        {
            var allPhotosOptions = new PHFetchOptions {
                SortDescriptors = new NSSortDescriptor[] { new NSSortDescriptor("creationDate", false) }
            };

            var allPhotos = PHAsset.FetchAssets(PHAssetMediaType.Image, allPhotosOptions);
            if (allPhotos.Count > 0) {
                AssetGroups.Add(new AssetCollection("All Photos", allPhotos));
            }
        }

        /// <summary>
        /// Gets the smart albums.
        /// </summary>
        private void GetSmartAlbums()
        {
            PHFetchResult smartAlbums = PHAssetCollection.FetchAssetCollections(
                PHAssetCollectionType.SmartAlbum,
                PHAssetCollectionSubtype.AlbumRegular,
                null);

            foreach (PHCollection smartAlbum in smartAlbums) {
                if (smartAlbum is PHAssetCollection album && smartAlbum.LocalizedTitle != "All Photos") {
                    PHFetchResult albumResult = PHAsset.FetchAssets(album, null);

                    if (albumResult.Count > 0) {
                        AssetGroups.Add(new AssetCollection(smartAlbum.LocalizedTitle!, albumResult));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the user albums.
        /// </summary>
        private void GetUserAlbums()
        {
            PHFetchResult userAlbums = PHCollection.FetchTopLevelUserCollections(null);
            foreach (PHCollection userAlbum in userAlbums) {
                if (userAlbum is PHAssetCollection) {
                    PHFetchResult albumResult = PHAsset.FetchAssets((userAlbum as PHAssetCollection)!, null);
                    if (albumResult.Count > 0) {
                        AssetGroups.Add(new AssetCollection(userAlbum.LocalizedTitle!, albumResult));
                    }
                }
            }
        }

        /// <summary>
        /// Called when user select cancel.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void CancelClicked(object? sender = null, EventArgs? e = null)
        {
            ImagePickerController.Instance?.CancelledPicker();
        }

        public override nint NumberOfSections(UITableView tableView) => 1;

        public override nint RowsInSection(UITableView tableview, nint section) =>
            AssetGroups.Count;

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => 42;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            const string cellIdentifier = "Cell";

            var cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell is null) {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }

            var album = AssetGroups[indexPath.Row];
            var albumSize = album.assets.Count;
            cell.TextLabel.Text = string.Format("{0} ({1})", album.title, albumSize);
            cell.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            cell.ImageView.Frame = new CGRect(0.0, 0.0, 320.0, 320.0);

            var options = new PHImageRequestOptions {
                ResizeMode = PHImageRequestOptionsResizeMode.Exact
            };

            ImagePickerController.Instance?.CacheManager.RequestImageForAsset(
                album.FirstAsset(),
                new CGSize(320, 320),
                PHImageContentMode.AspectFill,
                options,
                (result, info) => {

                    var itemSize = new CGSize(200, 200);

                    UIGraphics.BeginImageContextWithOptions(itemSize, false, UIScreen.MainScreen.Scale);

                    var imageRect = new CGRect(0.0, 0.0, itemSize.Width, itemSize.Height);

                    if (result is not null) {
                        result.DrawAsPatternInRect(imageRect);
                    }
                    cell.ImageView.Image = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                }
            );

            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var assetGroup = AssetGroups[indexPath.Row];

            var flowLayout = new UICollectionViewFlowLayout() {
                ItemSize = new CGSize(
                    ((float)UIScreen.MainScreen.Bounds.Size.Width - 18f) / 4f,
                    ((float)UIScreen.MainScreen.Bounds.Size.Width - 18f) / 4f),
                SectionInset = new UIEdgeInsets(0, 5, 0, 5),
                ScrollDirection = UICollectionViewScrollDirection.Vertical,
                MinimumInteritemSpacing = 2f,
                MinimumLineSpacing = 2f
            };

            var assetPicker = new AssetCollectionViewController(flowLayout, assetGroup);

            NavigationItem.BackBarButtonItem = new UIBarButtonItem(
                "Back",
                UIBarButtonItemStyle.Plain,
                null);

            NavigationController.PushViewController(assetPicker, true);
        }
    }
}