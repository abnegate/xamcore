using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Snackbar;
using Xamarin.Forms.Platform.Android;
using static Android.Provider.MediaStore.Images;

namespace XamCore.Droid
{
    [Activity(Label = "Choose Images", ParentActivity = typeof(MainActivityBase))]
    public class ImagePickerActivity : FormsAppCompatActivity
    {
        private RecyclerView? _recyclerView;
        private ImageAdapter? _imageAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_image_picker);

            var bar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(bar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.rv_gallery);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) {
                Window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

#pragma warning disable XA0001 // Find issues with Android API usage
#pragma warning disable CS0618 // Type or member is obsolete
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M) {
                    Window?.SetStatusBarColor(Resources!.GetColor(Resource.Color.colorPrimary, Theme));
                } else {
                    Window?.SetStatusBarColor(Resources!.GetColor(Resource.Color.colorPrimaryDark));
                }
#pragma warning restore XA0001
#pragma warning restore CS0618
            }
            PopulateImagesFromGallery();
        }

        public override bool OnCreateOptionsMenu(IMenu? menu)
        {
            MenuInflater.Inflate(Resource.Menu.image_picker, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) {
                case global::Android.Resource.Id.Home:
                    SetResult(Result.Canceled);
                    Finish();
                    return true;
                case Resource.Id.menu_done:
                    SetPhotosAndFinish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode != Constants.Android.StoragePermissionRequestCode ||
                grantResults.Length == 0) {
                return;
            }

            if (grantResults[0] == Permission.Granted) {
                PopulateImagesFromGallery();
            } else {
                Finish();
            }
        }

        public override void OnBackPressed()
        {
            SetResult(Result.Canceled);
            Finish();
        }

        private void InitializeRecyclerView(List<string> imageUrls)
        {
            _imageAdapter = new ImageAdapter(imageUrls);

            _recyclerView?.SetLayoutManager(new GridLayoutManager(this, 4));
            _recyclerView?.SetAdapter(_imageAdapter);
        }

        private bool MayRequestGalleryImages()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.M ||
                CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted) {
                return true;
            }

            RequestPermissions(
                new string[] { Manifest.Permission.WriteExternalStorage },
                Constants.Android.StoragePermissionRequestCode);

            return false;
        }

        private void PopulateImagesFromGallery()
        {
            if (!MayRequestGalleryImages()) {
                return;
            }

            InitializeRecyclerView(GetImagePaths());
        }
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS8604 // Possible null reference argument.
        private List<string> GetImagePaths()
        {
            string[] columns = {
                ImageColumns.Data,

                ImageColumns.Id
            };

            using var imagecursor = ContentResolver!.Query(
                Media.ExternalContentUri,
                columns,
                null,
                null,
                ImageColumns.DateTaken + " DESC");

            var imagePaths = new List<string>();

            for (var i = 0; i < imagecursor!.Count; i++) {
                imagecursor.MoveToPosition(i);

                var dataColumnIndex = imagecursor.GetColumnIndex(ImageColumns.Data);
                if (dataColumnIndex == -1) {
                    continue;
                }

                var path = imagecursor.GetString(dataColumnIndex);
                if (path is not null) {
                    imagePaths.Add(path);
                }
            }

            return imagePaths;
        }
#pragma warning restore CS0618
#pragma warning restore CS8604

        public void SetPhotosAndFinish()
        {
            var selectedItems = _imageAdapter?.GetCheckedItems();

            if (selectedItems is null || selectedItems.Count == 0) {
                SetResult(Result.Canceled);
                Finish();
                return;
            }

            var resultIntent = new Intent().PutStringArrayListExtra(
                Constants.Android.ImagePathsExtra,
                selectedItems);

            SetResult(Result.Ok, resultIntent);
            Finish();
        }

        private void ShowPermissionRationaleSnackBar()
        {
            Snackbar.Make(
                _recyclerView,
                GetString(Resource.String.permission_storage),
                BaseTransientBottomBar.LengthLong).Show();
        }
    }
}