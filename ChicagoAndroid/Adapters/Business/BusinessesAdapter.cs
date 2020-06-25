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
using Android.Graphics;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Business;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Business
{
    public class BusinessesAdapter : RecyclerView.Adapter
    {

        #region Constants, Enums, and Variables

        public event EventHandler<int> ItemClick;
        private View itemView;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessSearch> Rows { get; set; }

        /// <summary>
        /// Gets or sets load more
        /// </summary>
        public bool LoadMore { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; }

        /// <summary>
        /// Gets or sets view holders
        /// </summary>
        public BusinessesViewHolder BusinessesViewHolder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Activities.Businesses.BusinessesActivity MyContext { get; set; }

        #endregion

        #region Constructors

        public BusinessesAdapter(Activities.Businesses.BusinessesActivity context, List<BusinessSearch> rows,
            List<ImageViewImage> ImageViewImage)
        {
            this.Rows = rows;
            this.MyContext = context;
            this.ImageViewImage = ImageViewImage;
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
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.BusinessesListItem, parent, false);

            return new BusinessesViewHolder(this, itemView, OnListItemClick, OnCheckInBtnClick);
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
            this.BusinessesViewHolder = holder as BusinessesViewHolder;
            BindCardData(this.BusinessesViewHolder, Rows.ElementAt(position));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        public async void AddRowItems(List<BusinessSearch> rows)
        {
            foreach (var row in rows)
            {
                Rows.Add(row);
            }

            await GetLogoUris(rows);

            NotifyDataSetChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLogoUris(List<BusinessSearch> rows)
        {
            foreach (var b in rows)
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = b.BusinessId;
                var uriString = await BlobStorageHelper.GetBusinessLogoUri(b.BusinessId);
                if (!string.IsNullOrEmpty(uriString))
                {
                    Uri imageUri = new Uri(uriString);
                    logo.ImageUrl = imageUri;
                }
                this.ImageViewImage.Add(logo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="data"></param>
        private void BindCardData(BusinessesViewHolder viewHolder, BusinessSearch bEvent)
        {
            if (bEvent != null)
            {
                var itemLogo = this.ImageViewImage.Where(x => x.Id == bEvent.BusinessId).FirstOrDefault();
                viewHolder.BusinessName.Text = string.IsNullOrEmpty(bEvent.BusinessName) ? "" : bEvent.BusinessName;
                var bar = bEvent.Bar ? AppText.Bar + ", " : "";
                var club = bEvent.Club ? AppText.Club + ", " : "";
                var lounge = bEvent.Lounge ? AppText.Lounge + ", " : "";
                var restaurant = bEvent.Restaurant ? AppText.Restaurant + ", " : "";
                var other = bEvent.Other ? AppText.Other : "";
                viewHolder.BusinessType.Text = bar + club + lounge + restaurant + other;
                viewHolder.BusinessName.SetTag(Resource.Id.businessName, bEvent.BusinessId);
               
                if (itemLogo != null)
                {
                    if (itemLogo.ImageBitmap == null && itemLogo.ImageUrl != null)
                    {
                        this.MyContext.BeginDownloadingImage(itemLogo, viewHolder.BusinessLogo);
                    }
                    viewHolder.BusinessLogo.SetImageBitmap(itemLogo.ImageBitmap);
                    viewHolder.BusinessLogo.Visibility = itemLogo.ImageUrl == null ? ViewStates.Gone : ViewStates.Visible;

                    if (itemLogo.ImageBitmap != null)
                    {
                        //itemLogo.ImageBitmap.Recycle();
                        //itemLogo.ImageBitmap = null;
                    }

                }
            }
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
        /// <param name="position"></param>
        /// <param name="viewHolder"></param>
        public void OnCheckInBtnClick(int position, BusinessesViewHolder viewHolder)
        {
            var item = this.Rows.ElementAt(position);
            Intent activity = new Intent(this.MyContext, typeof(Activities.CheckIns.CheckInActivity));
            activity.PutExtra("BusinessInfo", JsonConvert.SerializeObject(item));
            activity.PutExtra("CheckInType", (int)Shared.Models.CheckIns.CheckIn.CheckInTypes.Business);
            this.MyContext.StartActivity(activity);
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
                var item = this.Rows.ElementAt(position);
                var itemLogo = this.ImageViewImage.Where(x => x.Id == item.BusinessId).FirstOrDefault();

                Intent activity = new Intent(this.MyContext, typeof(Activities.Businesses.BusinessProfileActivity));
                Activities.Businesses.BusinessProfileActivity.ImageBitmap = itemLogo.ImageBitmap;
                activity.PutExtra("BusinessInfo", JsonConvert.SerializeObject(item));
                this.MyContext.StartActivity(activity);
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        #endregion

    }
}