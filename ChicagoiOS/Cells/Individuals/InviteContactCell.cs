using Foundation;
using System;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class InviteContactCell : UITableViewCell
    {

        public UILabel _Title
        {
            get
            {
                return Title;
            }
        }

        public UIButton _InviteBtn
        {
            get
            {
                return InviteBtn;
            }
        }

        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PhoneContact Item { get; set; }

        public InviteContactCell (IntPtr handle) : base (handle)
        {
        }

        async partial void InviteBtn_TouchUpInside(UIButton sender)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.PleaseWait, -1f, ProgressHUD.MaskType.Black);

                    await AppDelegate.SMSMessageFactory.SendInvitation(Item.PhoneNumber,
                        AppDelegate.CurrentUser.FirstName, Item.FirstName);

                    BTProgressHUD.Dismiss();

                    Shared.Models.Points.Point point = new Shared.Models.Points.Point();
                    point.UserId = AppDelegate.CurrentUser.UserId;
                    point.PointStatus = Shared.Models.Points.Point.ToasterPointStatus.Earned;
                    point.EarnedDate = DateTime.Now;
                    point.RedeemedDate = null;
                    point.PointAmount = (int)Shared.Models.Points.Point.PointAmountScale.Invite;
                    BTProgressHUD.ShowSuccessWithStatus(ToastMessage.InviteSent, Helpers.ToastTime.SuccessTime);
                    await AppDelegate.ToasterPointsFactory.NewDailyPoint(point);
                    await new Tabs.Mobile.Shared.Helpers.PushNotificationHelper(AppDelegate.NotificationRegisterFactory,
                        Tabs.Mobile.Shared.Helpers.PushNotificationHelper.PushPlatform.iOS).NewPointsPush(point.UserId);
                }
            }catch(Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }
    }
}