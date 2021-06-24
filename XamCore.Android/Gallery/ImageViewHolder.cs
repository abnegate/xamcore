using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace XamCore.Droid
{
    public class ImageViewHolder : RecyclerView.ViewHolder
    {
        public ImageView? imageView;
        public ImageView? checkMark;

        public ImageViewHolder(Android.Views.View? itemView) : base(itemView)
        {
            imageView = itemView?.FindViewById<ImageView>(Resource.Id.img_image);
            checkMark = itemView?.FindViewById<ImageView>(Resource.Id.img_checkmark);
        }
    }
}
