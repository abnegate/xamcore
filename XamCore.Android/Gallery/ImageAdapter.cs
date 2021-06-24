using System;
using System.Collections.Generic;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;

namespace XamCore.Droid
{
    class ImageAdapter : RecyclerView.Adapter
    {
        public override int ItemCount { get => _imagesList.Count; }

        public List<string> _imagesList;
        public SparseBooleanArray _selectionArray;

        public ImageAdapter(List<string> imageList)
        {
            _selectionArray = new SparseBooleanArray();
            _imagesList = imageList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater
                .From(parent.Context)
                ?.Inflate(Resource.Layout.item_image, parent, false);

            return new ImageViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            holder.ItemView.SetTag(Resource.Id.cell_id, position);
            holder.ItemView.Click += (object sender, EventArgs e) => {
                var pos = (int)(sender as Android.Views.View)!.GetTag(Resource.Id.cell_id)!;
                var isContained = _selectionArray.Get(pos);

                (holder as ImageViewHolder)!.checkMark!.Visibility =
                    isContained ? ViewStates.Gone : ViewStates.Visible;

                _selectionArray.Put(pos, !isContained);
                NotifyItemChanged(position);
            };

            bool contained = _selectionArray.Get(position);
            (holder as ImageViewHolder)!.checkMark!.Visibility =
                contained ? ViewStates.Visible : ViewStates.Gone;

            string imagePath = _imagesList[position];

            Glide.With(holder.ItemView.Context)
                    .Load("file://" + imagePath)
                    .Thumbnail(0.3f)
                    .Apply(RequestOptions.SizeMultiplierOf(0.8f))
                    .Into((holder as ImageViewHolder)!.imageView);
        }

        public List<string> GetCheckedItems()
        {
            var paths = new List<string>();
            for (int i = 0; i < _imagesList.Count; i++) {
                if (_selectionArray.Get(i)) {
                    paths.Add(_imagesList[i]);
                }
            }
            return paths;
        }
    }
}
