using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Events;
using Plugin.Media;

namespace Tabs.Mobile.ChicagoAndroid.Activities.CheckIns
{
    [Activity(Label = "Check In", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class CheckInActivity : BaseActivity
    {

        #region Constants, Enums, Variables

        ImageView checkInPic;
        TextView addChkInPicHeader;
        Button checkInBtn;
        private Android.Support.V7.App.AlertDialog orderAlert;
        Android.Support.V7.App.AlertDialog.Builder orderBuilder;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        //public Business BusinessInfo { get; set; }

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

        #region Methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                SetContentView(Resource.Layout.CheckUserIn);

                checkInBtn = FindViewById<Button>(Resource.Id.checkInBtn);
                addChkInPicHeader = FindViewById<TextView>(Resource.Id.addChkInPicHeader);
                checkInPic = FindViewById<ImageView>(Resource.Id.checkInPic);

                this.CheckInType = (CheckIn.CheckInTypes)Intent.GetIntExtra("CheckInType", 0);

                if (this.CheckInType == CheckIn.CheckInTypes.Event)
                {
                    this.BusinessEvent = JsonConvert.DeserializeObject<BusinessEvents>(Intent.GetStringExtra("BusinessEventInfo"));
                }
                else if (this.CheckInType == CheckIn.CheckInTypes.Business)
                {
                    this.BusinessInfo = JsonConvert.DeserializeObject<BusinessSearch>(Intent.GetStringExtra("BusinessInfo"));
                }

                checkInBtn.Click += delegate
                {
                    CheckUserIn();
                };

                checkInPic.Click += delegate
                {
                    TakePic();
                };

                orderBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);

            }
            catch (Exception)
            {
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
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
                toasterPhoto.UserId = this.CurrentUser.UserId;
                var id = await App.ToasterPhotoFactory.Add(toasterPhoto);

                if (id > 0)
                {
                    await BlobStorageHelper.SaveToasterPhotosBlob(CheckInPicFile.Path, this.CurrentUser.UserId, id);
                }
            }
        }

        private void CancelClicked(object sender, DialogClickEventArgs arg)
        {
            if (orderAlert != null)
            {
                orderAlert.Dismiss();
                orderAlert.Dispose();
                orderAlert = null;
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
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
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


                    this.ShowProgressbar(true, "", ToastMessage.PleaseWait);

                    if (this.CheckInType == CheckIn.CheckInTypes.Event)
                    {
                        var eventExist = await App.CheckInFactory.EventCheckInExist(this.CurrentUser.UserId,
                            this.BusinessEvent.EventId, (int)CheckIn.CheckInTypes.Event);

                        if (eventExist)
                        {
                            this.ShowProgressbar(false, "", ToastMessage.PleaseWait);

                            if (orderAlert != null && orderAlert.IsShowing)
                            {
                                orderAlert.Dismiss();
                                orderAlert.Dispose();
                            }
                            orderBuilder.SetMessage(ToastMessage.ExistingEventCheckIn);
                            orderBuilder.SetCancelable(false);
                            //orderBuilder.SetNegativeButton(AppText.No, CancelClicked);
                            orderBuilder.SetPositiveButton(AppText.Ok, CancelClicked);
                            orderAlert = orderBuilder.Create();
                            orderAlert.Show();
                        } else
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
                this.ShowProgressbar(false, "", ToastMessage.Saving);
            }
        }

        private async void DoCheckIn()
        {
            var individual = await App.IndividualFactory.GetToasterByUserId(this.CurrentUser.UserId);

            CheckIn checkin = new CheckIn();
            checkin.UserId = this.CurrentUser.UserId;
            checkin.IndividualId = individual != null ? individual.IndividualId : 0;
            checkin.EventId = CheckInType == CheckIn.CheckInTypes.Event ? this.BusinessEvent.EventId : 0;
            checkin.BusinessId = CheckInType == CheckIn.CheckInTypes.Business ? this.BusinessInfo.BusinessId : this.BusinessEvent.BusinessId;
            checkin.CheckInType = CheckInType;
            checkin.CheckInDate = DateTime.Now;
            checkin.CheckedIn = true;
            checkin.Username = this.CurrentUser.Username;
            checkin.FirstName = this.CurrentUser.FirstName;
            checkin.LastName = this.CurrentUser.LastName;

            if (this.CheckInType == CheckIn.CheckInTypes.Event)
            {
                var business = await App.BusinessFactory.Get(this.BusinessEvent.BusinessId);

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

            var id = await App.CheckInFactory.CheckIn(checkin);

            if (id > 0)
            {
                await AddCheckInImage(id);

                Shared.Models.Points.Point point = new Shared.Models.Points.Point();
                point.UserId = this.CurrentUser.UserId;
                point.PointStatus = Shared.Models.Points.Point.ToasterPointStatus.Earned;
                point.EarnedDate = DateTime.Now;
                point.RedeemedDate = null;
                point.PointAmount = (int)Shared.Models.Points.Point.PointAmountScale.CheckIn;
                await App.ToasterPointsFactory.NewDailyPoint(point);
                await new PushNotificationHelper(App.NotificationRegisterFactory, PushNotificationHelper.PushPlatform.Android).NewPointsPush(point.UserId);
            }

            this.ShowProgressbar(false, "", ToastMessage.Saving);
            Fragments.Individuals.CheckIns.LiveToastersFragment.RequiresRefresh = true;
            this.Finish();
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
                    Toast.MakeText(this, "No camera available.", ToastLength.Short).Show();
                    return;
                }

                //var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                //{
                //    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                //});

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

                //CheckInNoteBtn.Hidden = true;
                this.CheckInPicFile = file;
                var ImageUri = Android.Net.Uri.Parse(file.Path);
                checkInPic.SetImageURI(ImageUri);
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }


        #endregion

    }
}