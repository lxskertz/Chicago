﻿using System;
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
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Individuals.Events
{
    public class LiveEventsAdapter : RecyclerView.Adapter
    {

        #region Constants, Enums, and Variables

        public event EventHandler<int> ItemClick;
        private View itemView;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessEvents> Rows { get; set; }

        /// <summary>
        /// Gets or sets load more
        /// </summary>
        public bool LoadMore { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; }

        /// <summary>
        /// Gets or sets the adapter context
        /// </summary>
        public LiveEventsFragment LiveEventsFragment { get; set; }

        /// <summary>
        /// Gets or sets view holders
        /// </summary>
        public LiveEventsViewHolder LiveEventsViewHolder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Activities.Individuals.IndividualHomeActivity MyContext { get; set; }

        #endregion

        #region Constructors

        public LiveEventsAdapter(Activities.Individuals.IndividualHomeActivity context, List<BusinessEvents> rows,
            LiveEventsFragment liveEventsFragment, List<ImageViewImage> ImageViewImage)
        {
            this.Rows = rows;
            this.MyContext = context;
            this.LiveEventsFragment = liveEventsFragment;
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
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LiveEventsListItem, parent, false);

            return new LiveEventsViewHolder(this, itemView, OnListItemClick, OnLikeBtnClick, OnCheckInBtnClick);
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
            this.LiveEventsViewHolder = holder as LiveEventsViewHolder;
            BindCardData(this.LiveEventsViewHolder, Rows.ElementAt(position));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        public async void AddRowItems(List<BusinessEvents> rows)
        {
            foreach (var row in rows)
            {
                Rows.Add(row);
            }

            await GetLogoUris(rows);
            await GetLikeEventList(rows);

            NotifyDataSetChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLogoUris(List<BusinessEvents> rows)
        {
            foreach (var b in rows)
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = b.EventId;
                Uri imageUri = new Uri(await BlobStorageHelper.GetEventLogoUri(b.EventId));
                logo.ImageUrl = imageUri;
                this.ImageViewImage.Add(logo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLikeEventList(List<BusinessEvents> rows)
        {
            foreach (var b in rows)
            {
                var liked = await App.EventLikesFactory.GetEventLike(this.LiveEventsFragment.HomeContext.CurrentUser.UserId, b.EventId);

                if (liked != null)
                {
                    this.LiveEventsFragment.AddRemoveLike(liked.Liked, b.EventId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="data"></param>
        private void BindCardData(LiveEventsViewHolder viewHolder, BusinessEvents bEvent)
        {
            viewHolder.ShareBtn.Visibility = ViewStates.Gone;
            if (bEvent != null)
            {
                var itemLogo = this.ImageViewImage.Where(x => x.Id == bEvent.EventId).FirstOrDefault();
                var userLikedEvent = this.LiveEventsFragment.LikeList.Where(x => x.Key == bEvent.EventId).FirstOrDefault();
                viewHolder.Title.Text = string.IsNullOrEmpty(bEvent.Title) ? "" : bEvent.Title;
                viewHolder.Venue.Text = string.IsNullOrEmpty(bEvent.Venue) ? "" : bEvent.Venue;
                var date = bEvent.StartDateTime.HasValue ? bEvent.StartDateTime.Value.ToLongDateString() : "";
                var time = bEvent.StartDateTime.HasValue ? bEvent.StartDateTime.Value.ToShortTimeString() : "";
                viewHolder.EventDate.Text = date + " " + time;
                viewHolder.Title.SetTag(Resource.Id.eventTitle, bEvent.EventId);

                if (userLikedEvent.Value)
                {
                    viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_black_24);
                }
                else
                {
                    viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_border_24);
                }

                if (bEvent.LikeCount == null)
                {
                    this.MyContext.GetLikeCount(bEvent, viewHolder.LikeCount);
                }
                this.MyContext.SetLikeCount(bEvent, viewHolder.LikeCount);

                // If the Image for this App has not been downloaded,
                // use the Placeholder image while we try to download
                // the real image from the web.
                if (itemLogo != null)
                {
                    if (itemLogo.ImageBitmap == null)
                    {
                        //app.Image = PlaceholderImage;
                        this.MyContext.BeginDownloadingImage(itemLogo, viewHolder.EventLogo);
                    }
                    viewHolder.EventLogo.SetImageBitmap(itemLogo.ImageBitmap);

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
        public void OnCheckInBtnClick(int position, LiveEventsViewHolder viewHolder)
        {
            var item = this.Rows.ElementAt(position);
            Intent activity = new Intent(this.MyContext, typeof(Activities.CheckIns.CheckInActivity));
            activity.PutExtra("BusinessEventInfo", JsonConvert.SerializeObject(item));
            activity.PutExtra("CheckInType", (int)Shared.Models.CheckIns.CheckIn.CheckInTypes.Event);
            this.MyContext.StartActivity(activity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="viewHolder"></param>
        public void OnLikeBtnClick(int position, LiveEventsViewHolder viewHolder)
        {
            var item = this.Rows.ElementAt(position);
            ToggleLike(item, viewHolder);
        }

        /// <summary>
        /// 
        /// </summary>
        private async void ToggleLike(BusinessEvents item, LiveEventsViewHolder viewHolder)
        {
            if (this.LiveEventsFragment.HomeContext.CheckNetworkConnectivity() == null)
            {
                this.LiveEventsFragment.HomeContext.ShowSnack(this.LiveEventsFragment.pageLayout, ToastMessage.NoInternet, "OK");
            }
            else
            {
                var LikedEvent = this.LiveEventsFragment.LikeList.Where(x => x.Key == item.EventId).FirstOrDefault();
                EventLikes eventLike = new EventLikes();
                eventLike.Liked = true;
                eventLike.EventId = item.EventId;
                eventLike.BusinessId = item.BusinessId; 
                eventLike.UserId = this.LiveEventsFragment.HomeContext.CurrentUser.UserId;

                this.LiveEventsFragment.HomeContext.ShowProgressbar(true, "", "...");

                if (LikedEvent.Key <= 0)
                {
                    await App.EventLikesFactory.LikeEvent(eventLike);
                    viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_black_24);
                    this.LiveEventsFragment.AddRemoveLike(true, item.EventId);
                }
                else
                {
                    var selected = LikedEvent.Value ? false : true;
                    await App.EventLikesFactory.UndoLikedEvent(selected, this.LiveEventsFragment.HomeContext.CurrentUser.UserId, item.EventId);

                    if (selected)
                    {
                        viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_black_24);
                    }
                    else
                    {
                        viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_border_24);
                    }
                    this.LiveEventsFragment.AddRemoveLike(selected, item.EventId);
                }

                this.MyContext.GetLikeCount(item, viewHolder.LikeCount);
                //var count = await App.EventLikesFactory.GetLikeCount(Item.BusinessId, Item.EventId);
                //Item.LikeCount = count;
                //this.DataSource.SetLikeCount(Item, _LikeCount);
                this.LiveEventsFragment.HomeContext.ShowProgressbar(false, "", "...");

            }
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
                var itemLogo = this.ImageViewImage.Where(x => x.Id == item.EventId).FirstOrDefault();

                Intent activity = new Intent(this.MyContext, typeof(Activities.Individuals.Events.EventInfoActivity));
                Activities.Individuals.Events.EventInfoActivity.ImageBitmap = itemLogo.ImageBitmap;
                activity.PutExtra("BusinessEventInfo", JsonConvert.SerializeObject(item));
                activity.PutExtra("ShowToolbar", false);
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