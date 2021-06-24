using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace XamCore.iOS
{
    public class AssetCollectionViewController : UICollectionViewController
    {
        public static NSString identifier = (NSString)"ImageAssetCellIdentifier";

        readonly AssetCollection items;

        public AssetCollectionViewController(UICollectionViewLayout layout, AssetCollection collection) : base(layout)
        {
            items = collection;

            CollectionView.RegisterClassForCell(typeof(AssetCell), identifier);
            CollectionView.AllowsMultipleSelection = true;

            var doneButtonItem = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, null);
            doneButtonItem.Clicked += async (sender, args) => await Done();

            NavigationItem.RightBarButtonItem = doneButtonItem;
        }

        public async Task Done()
        {
            foreach (var asset in ImagePickerController.Instance?.SelectedAssets ?? new()) {
                await asset.GetFullImage();
            }
            var task = ImagePickerController.Instance?.Done();
            if (task is not null) {
                await task;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.Title = "Choose Photos";
            CollectionView.BackgroundColor = UIColor.White;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section) =>
            items.assets.Count;

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell(identifier, indexPath) as AssetCell;

            Asset asset = items.assets[indexPath.Row];
            asset.GetThumbnail((image) => cell!.imageView.Image = image);

            return cell!;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var asset = items.assets[indexPath.Row];
            ImagePickerController.Instance?.SelectedAssets.Add(asset);
        }

        public override void ItemDeselected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var asset = items.assets[indexPath.Row];
            ImagePickerController.Instance?.SelectedAssets.Remove(asset);
        }

        /// <summary>
        /// Shows the image count alert when a user tries to select more than the max amount of images.
        /// </summary>
        public void ShowImageCountAlert()
        {
            var alert = UIAlertController.Create(
                "Sorry",
                "Too many images selected.",
                UIAlertControllerStyle.Alert
            );

            alert.AddAction(UIAlertAction.Create(
                "OK",
                UIAlertActionStyle.Default,
                null
            ));

            PresentViewController(alert, true, null);
        }
    }
}
