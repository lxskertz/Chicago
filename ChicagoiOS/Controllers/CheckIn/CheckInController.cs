using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models.CheckIns;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.ChicagoiOS.DataSource.Individuals.Events;
using Plugin.Media;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class CheckInController : BaseViewController
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public BusinessSearch BusinessInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessEvents BusinessEvent { get; set; }

        public CheckIn.CheckInTypes CheckInType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Plugin.Media.Abstractions.MediaFile CheckInPicFile;

        #endregion

        #region Constructors

        public CheckInController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(AppText.TakePhoto, UIBarButtonItemStyle.Plain, (sender, args) =>
                {
                    TakePic();

                }), true);

                UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() =>
                {
                    TakePic();
                });
                CheckInImage.AddGestureRecognizer(tapGesture);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void TakePic()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    BTProgressHUD.ShowErrorWithStatus("No camera available.", Helpers.ToastTime.ErrorTime);
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "TABSApp",
                    Name = "tabscheckin.jpg",
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    CompressionQuality = 50,
                    //RotateImage = false,
                    AllowCropping = true,
                    SaveMetaData = false
                });

                if (file == null)
                {
                    return;
                }

                //if (!CrossMedia.Current.IsPickPhotoSupported)
                //{
                //    return;
                //}
                //var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                //{
                //    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                //});


                //if (file == null)
                //{
                //    return;
                //}

                CheckInNoteBtn.Hidden = true;
                this.CheckInPicFile = file;
                UIImage image = new UIImage(file.Path);
                CheckInImage.Image = image;
                CheckInImage.ClipsToBounds = true;
            }
            catch (Exception)
            {
            }
        }

        partial void CheckInNoteBtn_TouchUpInside(UIButton sender)
        {
            TakePic();
        }

        /// <summary>
        /// Add event logo
        /// </summary>
        private async Task AddCheckInImage(int checkInId)
        {
            if (this.CheckInPicFile != null)
            {
                await BlobStorageHelper.SaveCheckinImageBlob(this.CheckInPicFile.Path, checkInId);
                
                ToasterPhoto toasterPhoto = new ToasterPhoto();
                toasterPhoto.UserId = AppDelegate.CurrentUser.UserId; 
                var id = await AppDelegate.ToasterPhotoFactory.Add(toasterPhoto);

                if (id > 0)
                {
                    await BlobStorageHelper.SaveToasterPhotosBlob(CheckInPicFile.Path, AppDelegate.CurrentUser.UserId, id);

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="going"></param>
        /// <returns></returns>
        private async void CheckUserIn()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    if (this.CheckInType == CheckIn.CheckInTypes.Event && this.BusinessEvent == null)
                    {
                        return;
                    }
                    if (this.CheckInType == CheckIn.CheckInTypes.Business && this.BusinessInfo == null)
                    {
                        return;
                    }

                    BTProgressHUD.Show(ToastMessage.PleaseWait, -1f, ProgressHUD.MaskType.Black);

                    if (this.CheckInType == CheckIn.CheckInTypes.Event)
                    {
                        var eventExist = await AppDelegate.CheckInFactory.EventCheckInExist(AppDelegate.CurrentUser.UserId,
                            this.BusinessEvent.EventId, (int)CheckIn.CheckInTypes.Event);

                        if (eventExist)
                        {
                            BTProgressHUD.Dismiss();

                            UIAlertController uIAlertController = new UIAlertController();
                            uIAlertController = UIAlertController.Create("", ToastMessage.ExistingEventCheckIn, UIAlertControllerStyle.Alert);
                            uIAlertController.AddAction(UIAlertAction.Create(AppText.Ok, UIAlertActionStyle.Cancel, null));
                            this.PresentViewController(uIAlertController, true, null);
                        }
                        else
                        {
                            DoCheckIn();
                        }
                    }
                    else
                    {
                        DoCheckIn();
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        private async void DoCheckIn()
        {
            var individual = await AppDelegate.IndividualFactory.GetToasterByUserId(AppDelegate.CurrentUser.UserId);

            CheckIn checkin = new CheckIn();
            checkin.UserId = AppDelegate.CurrentUser.UserId;
            checkin.IndividualId = individual != null ? individual.IndividualId : 0;
            checkin.EventId = CheckInType == CheckIn.CheckInTypes.Event ? this.BusinessEvent.EventId : 0;
            checkin.BusinessId = CheckInType == CheckIn.CheckInTypes.Business ? this.BusinessInfo.BusinessId : this.BusinessEvent.BusinessId;
            checkin.CheckInType = CheckInType;
            checkin.CheckInDate = DateTime.Now;
            checkin.CheckedIn = true;
            checkin.Username = AppDelegate.CurrentUser.Username;
            checkin.FirstName = AppDelegate.CurrentUser.FirstName;
            checkin.LastName = AppDelegate.CurrentUser.LastName;

            if (this.CheckInType == CheckIn.CheckInTypes.Event)
            {
                var business = await AppDelegate.BusinessFactory.Get(this.BusinessEvent.BusinessId);

                if (business != null)
                {
                    checkin.BusinessName = business.BusinessName;
                }
            }
            else
            {
                if (BusinessInfo != null)
                {
                    checkin.BusinessName = this.BusinessInfo.BusinessName;
                }
            }

            var id = await AppDelegate.CheckInFactory.CheckIn(checkin);

            if (id > 0)
            {
                await AddCheckInImage(id);

                Shared.Models.Points.Point point = new Shared.Models.Points.Point();
                point.UserId = AppDelegate.CurrentUser.UserId;
                point.PointStatus = Shared.Models.Points.Point.ToasterPointStatus.Earned;
                point.EarnedDate = DateTime.Now;
                point.RedeemedDate = null;
                point.PointAmount = (int)Shared.Models.Points.Point.PointAmountScale.CheckIn;
                await AppDelegate.ToasterPointsFactory.NewDailyPoint(point);
                await new PushNotificationHelper(AppDelegate.NotificationRegisterFactory, PushNotificationHelper.PushPlatform.iOS).NewPointsPush(point.UserId);
            }

            BTProgressHUD.Dismiss();

            LiveToastersEventsController.RequiresRefresh = true;
            this.NavigationController.PopViewController(true);

            BTProgressHUD.Dismiss();
        }

        partial void CheckInBtn_TouchUpInside(UIButton sender)
        {
            CheckUserIn();
        }

        #endregion

    }
}