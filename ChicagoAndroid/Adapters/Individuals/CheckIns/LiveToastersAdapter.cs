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
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals.CheckIns;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals;
using Tabs.Mobile.Shared.Models.CheckIns;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Individuals.CheckIns
{
    public class LiveToastersAdapter : RecyclerView.Adapter
    {

        #region Constants, Enums, and Variables

        public event EventHandler<int> ItemClick;
        private View itemView;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<CheckIn> Rows { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; }

        /// <summary>
        /// Gets or sets the adapter context
        /// </summary>
        public LiveToastersFragment LiveToastersFragment { get; set; }

        /// <summary>
        /// Gets or sets view holders
        /// </summary>
        public LiveToastersViewHolder LiveToastersViewHolder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Activities.Individuals.IndividualHomeActivity MyContext { get; set; }

        public CheckIn SelectedItem { get; set; }

        #endregion

        #region Constructors

        public LiveToastersAdapter(Activities.Individuals.IndividualHomeActivity context, List<CheckIn> rows,
            LiveToastersFragment liveToastersFragment, List<ImageViewImage> ImageViewImage)
        {
            this.Rows = rows;
            this.MyContext = context;
            this.LiveToastersFragment = liveToastersFragment;
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
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LiveToastersListItem, parent, false);

            return new LiveToastersViewHolder(this, itemView, OnListItemClick, OnLikeBtnClick, OnSendDrinkBtnClick, OnMoreBtnClick);
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
            this.LiveToastersViewHolder = holder as LiveToastersViewHolder;
            BindCardData(this.LiveToastersViewHolder, Rows.ElementAt(position));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="data"></param>
        private void BindCardData(LiveToastersViewHolder viewHolder, CheckIn checkin)
        {
            if (checkin != null)
            {
                var itemLogo = this.ImageViewImage.Where(x => x.Id == checkin.CheckInId).FirstOrDefault();
                var userLikedEvent = this.LiveToastersFragment.LikeList.Where(x => x.Key == checkin.CheckInId).FirstOrDefault();

                var username = string.IsNullOrEmpty(checkin.Username) ? "" : checkin.Username;
                var businessName = string.IsNullOrEmpty(checkin.BusinessName) ? "" : " is at " + checkin.BusinessName;

                viewHolder.Username.Text = username + businessName;
                viewHolder.Username.SetTag(Resource.Id.eventTitle, checkin.CheckInId);
                viewHolder.SendDrinkBtn.Visibility = checkin.UserId ==
                    this.LiveToastersFragment.HomeContext.CurrentUser.UserId ? ViewStates.Gone : ViewStates.Visible;

                viewHolder.MoreBtn.Visibility = checkin.UserId ==
                 this.LiveToastersFragment.HomeContext.CurrentUser.UserId ? ViewStates.Gone : ViewStates.Visible;


                if (userLikedEvent.Value)
                {
                    viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_black_24);
                }
                else
                {
                    viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_border_24);
                }

                if (checkin.LikeCount == null)
                {
                    this.GetLikeCount(checkin, viewHolder.LikeCount);
                }
                this.SetLikeCount(checkin, viewHolder.LikeCount);

                // If the Image for this App has not been downloaded,
                // use the Placeholder image while we try to download
                // the real image from the web.
                if (itemLogo != null && itemLogo.ImageBitmap == null)
                {
                    //app.Image = PlaceholderImage;
                    this.MyContext.BeginDownloadingImage(itemLogo, viewHolder.CheckInPic);
                }
                viewHolder.CheckInPic.SetImageBitmap(itemLogo.ImageBitmap);

                if (itemLogo.ImageBitmap != null)
                {
                    //itemLogo.ImageBitmap.Recycle();
                    //itemLogo.ImageBitmap = null;
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
        public void OnLikeBtnClick(int position, LiveToastersViewHolder viewHolder)
        {
            var item = this.Rows.ElementAt(position);
            ToggleLike(item, viewHolder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="viewHolder"></param>
        public void OnSendDrinkBtnClick(int position, LiveToastersViewHolder viewHolder)
        {
            var item = this.Rows.ElementAt(position);
            Intent activity = new Intent(this.MyContext, typeof(Activities.Drinks.SendDrinkActivity));
            activity.PutExtra("CheckInItem", JsonConvert.SerializeObject(item));
            this.MyContext.StartActivity(activity);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="viewHolder"></param>
        public void OnMoreBtnClick(int position, LiveToastersViewHolder viewHolder)
        {
            var item = this.Rows.ElementAt(position);
            this.SelectedItem = item;
            var fragment = Fragments.Individuals.ToastersMoreBottomSheetFragment.NewInstance(this.MyContext, Fragments.Individuals.ToastersMoreBottomSheetFragment.Caller.LiveToasters);
            fragment.Show(this.MyContext.SupportFragmentManager, "1");
        }


        /// <summary>
        /// 
        /// </summary>
        private async void ToggleLike(CheckIn item, LiveToastersViewHolder viewHolder)
        {
            if (this.LiveToastersFragment.HomeContext.CheckNetworkConnectivity() == null)
            {
                this.LiveToastersFragment.HomeContext.ShowSnack(this.LiveToastersFragment.pageLayout, ToastMessage.NoInternet, "OK");
            }
            else
            {
                var LikedCheckIn = this.LiveToastersFragment.LikeList.Where(x => x.Key == item.CheckInId).FirstOrDefault();
                CheckInLikes checkInLikes = new CheckInLikes();
                checkInLikes.Liked = true;
                checkInLikes.CheckInId = item.CheckInId;
                checkInLikes.BusinessId = item.BusinessId;
                checkInLikes.UserId = this.LiveToastersFragment.HomeContext.CurrentUser.UserId;

                this.LiveToastersFragment.HomeContext.ShowProgressbar(true, "", "...");

                if (LikedCheckIn.Key <= 0)
                {
                    await App.CheckInLikesFactory.LikeChecKin(checkInLikes);
                    viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_black_24);
                    this.LiveToastersFragment.AddRemoveLike(true, item.CheckInId);
                }
                else
                {
                    var selected = LikedCheckIn.Value ? false : true;
                    await App.CheckInLikesFactory.UndoLikedCheckIn(selected, this.LiveToastersFragment.HomeContext.CurrentUser.UserId, item.CheckInId);

                    if (selected)
                    {
                        viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_black_24);
                    }
                    else
                    {
                        viewHolder.LikeBtn.SetImageResource(Resource.Drawable.favorite_border_24);
                    }
                    this.LiveToastersFragment.AddRemoveLike(selected, item.CheckInId);
                }

                this.GetLikeCount(item, viewHolder.LikeCount);
                //var count = await App.EventLikesFactory.GetLikeCount(Item.BusinessId, Item.EventId);
                //Item.LikeCount = count;
                //this.DataSource.SetLikeCount(Item, _LikeCount);
                this.LiveToastersFragment.HomeContext.ShowProgressbar(false, "", "...");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="likeCountTxt"></param>
        public void SetLikeCount(CheckIn item, TextView likeCountTxt)
        {
            this.LiveToastersFragment.HomeContext.RunOnUiThread(() =>
            {
                var text = item.LikeCount != null ? item.LikeCount > 1 ? item.LikeCount + " likes" : item.LikeCount + " like" : "";
                likeCountTxt.Text = text;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <param name="tableView"></param>
        public async void GetLikeCount(CheckIn item, TextView likeCountTxt)
        {
            var count = await App.CheckInLikesFactory.GetLikeCount(item.CheckInId);
            item.LikeCount = count;
            SetLikeCount(item, likeCountTxt);
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
                var itemLogo = this.ImageViewImage.Where(x => x.Id == item.CheckInId).FirstOrDefault();

                if (itemLogo != null && itemLogo.ImageBitmap != null)
                {
                    Intent activity = new Intent(this.MyContext, typeof(Activities.Individuals.MyImageActivity));
                    Activities.Individuals.MyImageActivity.SelectedImage = itemLogo.ImageBitmap;
                    this.MyContext.StartActivity(activity);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        #endregion

    }
}