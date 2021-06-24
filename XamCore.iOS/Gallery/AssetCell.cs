using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace XamCore.iOS
{
    public class AssetCell : UICollectionViewCell
    {
        public UIImageView imageView;
        public UIImageView overlayImageView;

        static readonly Lazy<UIImage> defaultOverlayImage =
            new(() => UIImage.FromBundle("icon_checkbox")!);

        public override bool Selected
        {
            get => base.Selected;
            set
            {
                base.Selected = value;
                overlayImageView.Hidden = !value;
            }
        }

        public override NSString ReuseIdentifier
        {
            get => AssetCollectionViewController.identifier;
        }

        [Export("initWithFrame:")]
        public AssetCell(CGRect frame) : base(frame)
        {
            imageView = new UIImageView(frame);

            overlayImageView = new UIImageView(frame) {
                Image = defaultOverlayImage.Value,
                Hidden = true
            };

            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            overlayImageView.ContentMode = UIViewContentMode.ScaleAspectFill;

            imageView.Center = ContentView.Center;
            overlayImageView.Center = ContentView.Center;

            ContentView.AddSubview(imageView);
            ContentView.AddSubview(overlayImageView);
        }
    }
}
