using Foundation;
using System;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoiOS.DataSource.Individuals.Events;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToastersEventsCell : UITableViewCell
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public UILabel _EventTitle
        {
            get
            {
                return EventTitle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UILabel _EventVenue
        {
            get
            {
                return EventLocation;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UILabel _EventDate
        {
            get
            {
                return EventDate;
            }
        }

        public UIImageView _EventLogo
        {
            get
            {
                return EventLogo;
            }
        }

        public UIButton _LikeBtn
        {
            get
            {
                return LIkeBtn;
            }
        }

        public UIButton _ShareBtn
        {
            get
            {
                return ShareBtn;
            }
        }

        public UILabel _LikeCount
        {
            get
            {
                return LikeCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ToastersEventDataSource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessEvents Item { get; set; }

        public KeyValuePair<int, bool> LikedEvent { get; set; }

        #endregion

        public ToastersEventsCell (IntPtr handle) : base (handle)
        {
        }

        partial void LIkeBtn_TouchUpInside(UIButton sender)
        {
            ToggleLike();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void ToggleLike()
        {
            if (AppDelegate.IsOfflineMode())
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
            }
            else
            {
                LikedEvent = this.DataSource.Controller.LikeList.Where(x => x.Key == Item.EventId).FirstOrDefault();
                BTProgressHUD.Show("...", -1f, ProgressHUD.MaskType.Black);
                EventLikes eventLike = new EventLikes();
                eventLike.Liked = true;
                eventLike.EventId = Item.EventId;
                eventLike.BusinessId = Item.BusinessId; //this.BusinessInfo != null ? this.BusinessInfo.BusinessId : 0;
                eventLike.UserId = AppDelegate.CurrentUser.UserId;

                if (LikedEvent.Key <= 0)
                {
                    await AppDelegate.EventLikesFactory.LikeEvent(eventLike);
                    _LikeBtn.SetImage(UIImage.FromFile("Favorites/favorite_24pt"), UIControlState.Normal);
                    this.DataSource.Controller.AddRemoveLike(true, Item.EventId);
                }
                else
                {
                    var selected = LikedEvent.Value ? false : true;
                    await AppDelegate.EventLikesFactory.UndoLikedEvent(selected, AppDelegate.CurrentUser.UserId, Item.EventId);

                    if (selected)
                    {
                        _LikeBtn.SetImage(UIImage.FromFile("Favorites/favorite_24pt"), UIControlState.Normal);
                    }
                    else
                    {
                        _LikeBtn.SetImage(UIImage.FromFile("Favorites/favorite_border_24pt"), UIControlState.Normal);
                    }
                    this.DataSource.Controller.AddRemoveLike(selected, Item.EventId);
                }
                var count = await AppDelegate.EventLikesFactory.GetLikeCount(Item.BusinessId, Item.EventId);
                Item.LikeCount = count;
                this.DataSource.SetLikeCount(Item, _LikeCount);
                BTProgressHUD.Dismiss();
            }
        }
    }
}