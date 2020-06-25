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
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.ChicagoAndroid.Activities.Businesses;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Business;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Business
{
    public class BusinessPhotoAdaper : RecyclerView.Adapter
    {
        #region Constants, Enums, and Variables

        public event EventHandler<int> ItemClick;
        private View itemView;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<BusinesPhoto> Rows { get; set; }

        /// <summary>
        /// Gets or sets the adapter context
        /// </summary>
        public BusinesPhotoActivity MyContext { get; set; }

        /// <summary>
        /// Gets or sets view holders
        /// </summary>
        public BusinessPhotoViewHolder BusinessPhotoViewHolder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImages { get; set; }

        #endregion

        #region Constructors

        public BusinessPhotoAdaper(BusinesPhotoActivity context, List<BusinesPhoto> rows,
            List<ImageViewImage> imageViewImages)
        {
            this.Rows = rows;
            this.MyContext = context;
            this.ImageViewImages = imageViewImages;
        }

        #endregion

        #region Methods

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
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.BusinessPhotosListitem, parent, false);

            return new BusinessPhotoViewHolder(this, itemView, OnListItemClick);
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
            this.BusinessPhotoViewHolder = holder as BusinessPhotoViewHolder;
            BindCardData(this.BusinessPhotoViewHolder, Rows.ElementAt(position));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="data"></param>
        private void BindCardData(BusinessPhotoViewHolder viewHolder, BusinesPhoto photo)
        {
            var itemLogo = this.ImageViewImages.Where(x => x.Id == photo.BusinessPhotoId).FirstOrDefault();
            if (itemLogo != null && itemLogo.ImageBitmap == null)
            {
                //app.Image = PlaceholderImage;
                this.MyContext.BeginDownloadingImage(itemLogo, viewHolder.Photo);
            }
            viewHolder.Photo.SetImageBitmap(itemLogo.ImageBitmap);

            if (itemLogo.ImageBitmap != null)
            {
                //itemLogo.ImageBitmap.Recycle();
                //itemLogo.ImageBitmap = null;
            }
        }

        public async Task AddNewImage(int businessId, int photoId)
        {
            if (this.Rows == null)
            {
                this.Rows = new List<BusinesPhoto>();
            }

            this.Rows.Add(new BusinesPhoto() { BusinessId = businessId, BusinessPhotoId = photoId });

            ImageViewImage logo = new ImageViewImage();
            logo.Id = photoId;
            Uri imageUri = new Uri(await BlobStorageHelper.GetBusinessPhotosUri(businessId, photoId));
            logo.ImageUrl = imageUri;
            this.ImageViewImages.Add(logo);

            NotifyDataSetChanged();
        }

        /// <summary>
        /// Invoke the OnItemClick event with positional data..
        /// </summary>
        /// <param name="position"></param>
        private void OnItemClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(int position)
        {
            try
            {
                //var item = this.Rows.ElementAt(position);
                //Intent activity = new Intent(this.MyContext, typeof(Activities.Individuals.Events.EventInfoActivity));
                ////Activities.Individuals.Events.EventInfoActivity.ImageBitmap = itemLogo.ImageBitmap;
                //activity.PutExtra("BusinessEventInfo", JsonConvert.SerializeObject(item));
                //activity.PutExtra("ShowToolbar", false);
                //activity.PutExtra("IsBusiness", true);
                //this.MyContext.StartActivity(activity);
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        #endregion

    }
}