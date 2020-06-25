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
    public class ToastersAdapter : RecyclerView.Adapter
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
        public Activities.Individuals.ToastersActivity Owner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Toasters> Rows { get; set; } = new List<Toasters>();

        /// <summary>
        /// 
        /// </summary>
        public ToastersSearchViewHolder ToastersSearchViewHolder { get; set; }

        public List<ImageViewImage> ImageViewImages { get; set; }

        #endregion

        #region Constructors

        public ToastersAdapter(Activities.Individuals.ToastersActivity owner, List<Toasters> rows,
            List<ImageViewImage> imageViewImages)
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
        public async void AddRowItems(List<Toasters> rows)
        {
            foreach (var row in rows)
            {
                Rows.Add(row);
            }

            await GetPicUris(rows);
            NotifyDataSetChanged();
        }

        public async Task GetPicUris(List<Toasters> rows)
        {
            foreach (var b in rows)
            {
                ImageViewImage itemLogo = new ImageViewImage();
                var userId = b.UserOneId == this.Owner.CurrentUser.UserId ? b.UserTwoId : b.UserOneId;
                itemLogo.Id = userId;

                var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(userId);
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
        private void BindCardData(ToastersSearchViewHolder viewHolder, Toasters item)
        {
            viewHolder.ToastersAdapter = this;
            //viewHolder.RowItem = item;
            var fname = string.IsNullOrEmpty(item.FirstName) ? string.Empty : item.FirstName;
            var lname = string.IsNullOrEmpty(item.LastName) ? string.Empty : item.LastName;
            viewHolder.Name.Text = fname + " " + lname;
            viewHolder.Username.Text = string.IsNullOrEmpty(item.Username) ? string.Empty : item.Username;
            viewHolder.ToasterRequest.Visibility = ViewStates.Gone;

            //var itemLogo = this.ImageViewImages.Where(x => x.Id == item.UserOneId || x.Id == item.UserTwoId).FirstOrDefault();
            var itemLogo = this.ImageViewImages.Where(x => x.Id == item.UserId).FirstOrDefault();

            if (itemLogo != null)
            {
                if (itemLogo.ImageBitmap == null)
                {
                    this.Owner.BeginDownloadingImage(itemLogo, viewHolder.ProfilePic);
                }
                viewHolder.ProfilePic.SetImageBitmap(itemLogo.ImageBitmap);

                if (itemLogo.ImageBitmap != null)
                {
                    //itemLogo.ImageBitmap.Recycle();
                    //itemLogo.ImageBitmap = null;
                }

            }
        }


        #endregion

    }
}