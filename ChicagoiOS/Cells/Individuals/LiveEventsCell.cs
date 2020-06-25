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
    public partial class LiveEventsCell : UITableViewCell
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

        public UIButton _CheckInBtn
        {
            get
            {
                return CheckInBtn;
            }
        }

        public UIButton _LikeBtn
        {
            get
            {
                return LikeBtn;
            }
        }

        public UIButton ShareBtn
        {
            get
            {
                return _ShareBtn;
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
        public LiveEventsDataSource DataSource { get; set; }

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

        #region Constructors

        public LiveEventsCell (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        partial void CheckInBtn_TouchUpInside(UIButton sender)
        {
            OpenCheckIn();
        }

        partial void LikeBtn_TouchUpInside(UIButton sender)
        {
            ToggleLike();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenCheckIn()
        {
            var controller = this.DataSource.Controller.Storyboard.InstantiateViewController("CheckInController") as CheckInController;
            controller.BusinessEvent = Item;
            controller.CheckInType = Shared.Models.CheckIns.CheckIn.CheckInTypes.Event;
            this.DataSource.Controller.NavigationController.PushViewController(controller, true);
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
                LikedEvent = this.DataSource.Controller.EventsLikeList.Where(x => x.Key == Item.EventId).FirstOrDefault();
                BTProgressHUD.Show("...", -1f, ProgressHUD.MaskType.Black);

                if (LikedEvent.Key <= 0)
                {
                    EventLikes eventLike = new EventLikes();
                    eventLike.Liked = true;
                    eventLike.EventId = Item.EventId;
                    eventLike.BusinessId = Item.BusinessId; //this.BusinessInfo != null ? this.BusinessInfo.BusinessId : 0;
                    eventLike.UserId = AppDelegate.CurrentUser.UserId;

                    await AppDelegate.EventLikesFactory.LikeEvent(eventLike);
                    _LikeBtn.SetImage(UIImage.FromFile("Favorites/favorite_24pt"), UIControlState.Normal);
                    this.DataSource.Controller.AddRemoveEventLike(true, Item.EventId);
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
                    this.DataSource.Controller.AddRemoveEventLike(selected, Item.EventId);
                }

                var count = await AppDelegate.EventLikesFactory.GetLikeCount(Item.BusinessId, Item.EventId);
                Item.LikeCount = count;
                this.DataSource.SetLikeCount(Item, _LikeCount);

                BTProgressHUD.Dismiss();
            }
        }

        #endregion

    }
}