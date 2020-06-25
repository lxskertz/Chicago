using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Individuals
{
    public class ToastersSearchAdapter : RecyclerView.Adapter
    {
        #region Constants, Enums, and Variables

        private View itemView;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets load more
        /// </summary>
        public bool LoadMore { get; set; } = true;

        /// <summary>
        /// Gets or sets the owner
        /// </summary>
        public Fragments.Individuals.ToastersSearchFragment Owner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ToastersSearchItem> Rows { get; set; } = new List<ToastersSearchItem>();

        /// <summary>
        /// 
        /// </summary>
        public ToastersSearchViewHolder ToastersSearchViewHolder { get; set; }

        public List<ImageViewImage> ImageViewImages { get; set; }

        #endregion

        #region Constructors

        public ToastersSearchAdapter(Fragments.Individuals.ToastersSearchFragment owner, 
            List<ToastersSearchItem> rows, List<ImageViewImage> imageViewImages)
        {
            this.Owner = owner;
            this.Rows = rows;
            this.ImageViewImages = imageViewImages;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        public async void AddRowItems(List<ToastersSearchItem> rows)
        {
            foreach (var row in rows)
            {
                Rows.Add(row);
            }

            await GetPicUris(rows);
            NotifyDataSetChanged();
        }

        public async Task GetPicUris(List<ToastersSearchItem> rows)
        {
            foreach (var b in rows)
            {
                ImageViewImage itemLogo = new ImageViewImage();
                itemLogo.Id = b.UserId;

                var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(b.UserId);
                if (!string.IsNullOrEmpty(uriString))
                {
                    Uri imageUri = new Uri(uriString);
                    itemLogo.ImageUrl = imageUri;
                    this.ImageViewImages.Add(itemLogo);
                }
            }
        }

        /// <summary>
        /// Return the view type of the item at position for the purposes of view recycling.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }

        /// <summary>
        /// Returns the total number of items in the data set hold by the adapter.
        /// </summary>
        public override int ItemCount
        {
            get
            {
                return Rows.Count;
            }
        }

        /// <summary>
        /// Create a new container/product CardView invoked by the layout manager
        /// Called when RecyclerView needs a new RecyclerView.ViewHolder of the given type to represent an item.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ToastersSearchListItem, parent, false);

            return new ToastersSearchViewHolder(this, itemView);
        }

        /// <summary>
        /// Fill in the contents of the container/product card invoked by the layout manager
        /// Called by RecyclerView to display the data at the specified position.
        /// This method should update the contents of the ItemView to reflect the item at the given position.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            this.ToastersSearchViewHolder = holder as ToastersSearchViewHolder;
            BindCardData(this.ToastersSearchViewHolder, Rows.ElementAt(position));
            this.ToastersSearchViewHolder.Name.SetTag(Resource.Id.name, Rows.ElementAt(position).UserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="data"></param>
        private void BindCardData(ToastersSearchViewHolder viewHolder, ToastersSearchItem item)
        {
            viewHolder.ToastersSearchAdapter = this;
            viewHolder.RowItem = item;
            viewHolder.Name.Text = string.IsNullOrEmpty(item.Name) ? string.Empty : item.Name;
            viewHolder.Username.Text = string.IsNullOrEmpty(item.Username) ? string.Empty : item.Username;
            viewHolder.ToasterRequest.Visibility = ViewStates.Gone;

            var itemLogo = this.ImageViewImages.Where(x => x.Id == item.UserId).FirstOrDefault();

            if (itemLogo != null)
            {
                if (itemLogo.ImageBitmap == null)
                {
                    this.Owner.HomeContext.BeginDownloadingImage(itemLogo, viewHolder.ProfilePic);
                }
                viewHolder.ProfilePic.SetImageBitmap(itemLogo.ImageBitmap);

                if (itemLogo.ImageBitmap != null)
                {
                    //itemLogo.ImageBitmap.Recycle();
                    //itemLogo.ImageBitmap = null;
                }
            } else
            {
                viewHolder.ProfilePic.SetImageBitmap(null);
            }
        }


        #endregion

    }
}