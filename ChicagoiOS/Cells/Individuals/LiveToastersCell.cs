using Foundation;
using System;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoiOS.DataSource.Individuals.CheckIns;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class LiveToastersCell : UITableViewCell
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public UILabel _Username
        {
            get
            {
                return Username;
            }
        }

        public UIImageView _UserImage
        {
            get
            {
                return UserImage;
            }
        }

        public UIButton _SendDrinkBtn
        {
            get
            {
                return SendDrinkBtn;
            }
        }

        public UIButton _LikeBtn
        {
            get
            {
                return LikeBtn;
            }
        }

        public UIButton _MoreBtn
        {
            get
            {
                return MoreBtn;
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
        public LiveToastersDataSource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CheckIn Item { get; set; }

        public KeyValuePair<int, bool> LikedCheckIn { get; set; }

        #endregion

        #region Constructors

        public LiveToastersCell (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

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
                LikedCheckIn = this.DataSource.Controller.CheckInsLikeList.Where(x => x.Key == Item.CheckInId).FirstOrDefault();
                BTProgressHUD.Show("...", -1f, ProgressHUD.MaskType.Black);
                CheckInLikes checkInLikes = new CheckInLikes();
                checkInLikes.Liked = true;
                checkInLikes.CheckInId = Item.CheckInId;
                checkInLikes.BusinessId = Item.BusinessId;
                checkInLikes.UserId = AppDelegate.CurrentUser.UserId;

                if (LikedCheckIn.Key <= 0)
                {
                    await AppDelegate.CheckInLikesFactory.LikeChecKin(checkInLikes);
                    _LikeBtn.SetImage(UIImage.FromFile("Favorites/favorite_24pt"), UIControlState.Normal);
                    this.DataSource.Controller.AddRemoveCheckInLike(true, Item.CheckInId);
                    //await new Shared.Helpers.PushNotificationHelper(AppDelegate.NotificationRegisterFactory).LikedCheckInPush(Item.UserId);
                }
                else
                {
                    var selected = LikedCheckIn.Value ? false : true;
                    await AppDelegate.CheckInLikesFactory.UndoLikedCheckIn(selected, AppDelegate.CurrentUser.UserId, Item.CheckInId);

                    if (selected)
                    {
                        _LikeBtn.SetImage(UIImage.FromFile("Favorites/favorite_24pt"), UIControlState.Normal);
                    }
                    else
                    {
                        _LikeBtn.SetImage(UIImage.FromFile("Favorites/favorite_border_24pt"), UIControlState.Normal);
                    }
                    this.DataSource.Controller.AddRemoveCheckInLike(selected, Item.CheckInId);
                }

                var count = await AppDelegate.CheckInLikesFactory.GetLikeCount(Item.CheckInId);
                Item.LikeCount = count;
                this.DataSource.SetLikeCount(Item, _LikeCount);
                BTProgressHUD.Dismiss();
            }
        }

        partial void SendDrinkBtn_TouchUpInside(UIButton sender)
        {
            var controller = this.DataSource.Controller.Storyboard.InstantiateViewController("SendDrinkController") as SendDrinkController;
            controller.CheckInItem = Item;
            this.DataSource.Controller.NavigationController.PushViewController(controller, true);
        }

        partial void LikeBtn_TouchUpInside(UIButton sender)
        {
            ToggleLike();
        }

        partial void MoreBtn_TouchUpInside(UIButton sender)
        {
            this.DataSource.Controller.MoreMenuActions(Item);
        }

        #endregion
    }
}