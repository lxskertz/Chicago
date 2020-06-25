using Foundation;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.ChicagoiOS.Delegates;
using BigTed;
using Plugin.Media;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToasterProfileController : BaseViewController
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool FromSearchedUser { get; set; }

        public bool FromRequestPending { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Shared.Models.Users.Users SearchedUser  { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Toasters Toaster { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool RequireRefresh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string UpdatedHeadline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool ProfilePicUpdated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string UpdatedHometown { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Individual IndividualInfo { get; set; }

        private Toasters ToasterInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Individual SearchedIndividualInfo { get; set; }

        public DataSource.Individuals.ToasterPhotosDataSource ToasterPhotosDataSource { get; set; }

        public List<ImageViewImage> ImageViewImages { get; set; } = new List<ImageViewImage>();

        public List<ToasterPhoto> ToasterPhotos { get; set; } = new List<ToasterPhoto>();

        public UIButton ToasterRequest { get; set; }

        public UIImageView ProfilePic { get; set; }

        public UILabel Name { get; set; }

        public UILabel HomeTown { get; set; }

        public UILabel Headline { get; set; }

        #endregion

        #region Constructors

        public ToasterProfileController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public async void LoadProfileInfo()
        {
            if (string.IsNullOrEmpty(this.Name.Text))
            {
                await GetProfileInfo();
                await GetProfilePic();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetProfileInfo()
        {
            try
            {
                if (!FromSearchedUser)
                {
                    var fname = string.IsNullOrEmpty(AppDelegate.CurrentUser.FirstName) ? string.Empty : AppDelegate.CurrentUser.FirstName;
                    var lname = string.IsNullOrEmpty(AppDelegate.CurrentUser.LastName) ? string.Empty : AppDelegate.CurrentUser.LastName;
                    Name.Text = fname + " " + lname;
                }
                else
                {
                    Name.Text = SearchedUser.FirstName;
                    this.NavigationItem.Title = string.IsNullOrEmpty(SearchedUser.Username) ? "Profile" : SearchedUser.Username;
                }

                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                    this.IndividualInfo = await AppDelegate.IndividualFactory.GetToasterByUserId(AppDelegate.CurrentUser.UserId);
                    //var userId = FromSearchedUser ? SearchedUser.UserId : AppDelegate.CurrentUser.UserId;

                    if (!FromSearchedUser && IndividualInfo != null)
                    {

                        HomeTown.Text = string.IsNullOrEmpty(this.IndividualInfo.HomeTown) ? string.Empty : this.IndividualInfo.HomeTown;
                        Headline.Text = string.IsNullOrEmpty(this.IndividualInfo.Headline) ? string.Empty : this.IndividualInfo.Headline;
                    }

                    if (SearchedUser != null)
                    {
                        this.SearchedIndividualInfo = await AppDelegate.IndividualFactory.GetToasterByUserId(SearchedUser.UserId);
                    }
                    if (this.SearchedIndividualInfo != null)
                    {
                        HomeTown.Text = string.IsNullOrEmpty(this.SearchedIndividualInfo.HomeTown) ? string.Empty : this.SearchedIndividualInfo.HomeTown;
                        Headline.Text = string.IsNullOrEmpty(this.SearchedIndividualInfo.Headline) ? string.Empty : this.SearchedIndividualInfo.Headline;

                        var individualId = this.IndividualInfo != null ? this.IndividualInfo.IndividualId : 0;
                        ToasterInfo = await AppDelegate.ToastersFactory.Connected(AppDelegate.CurrentUser.UserId, SearchedUser.UserId);

                        if (FromRequestPending)
                        {
                            ToasterRequest.SetTitle(AppText.AcceptRequest, UIControlState.Normal);
                        }
                        else if (ToasterInfo != null)
                        {
                            if (ToasterInfo.ToastersId > 0)
                            {
                                if (ToasterInfo.RequestStatus == Toasters.ToasterRequestStatus.Accepted )
                                {
                                    ToasterRequest.Hidden = true;
                                }
                                else
                                {
                                    ToasterRequest.SetTitle(AppText.Requested, UIControlState.Normal);
                                }
                            }
                        }
                    }

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }

        }

        private async Task GetToastersCount()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {

                    ToasterRequest.SetTitle("Toasters", UIControlState.Normal);
                    return;
                }
                else
                {
                    var count = await AppDelegate.ToastersFactory.GetTotalToastersCount(AppDelegate.CurrentUser.UserId);
                    ToasterRequest.SetTitle(count + " Toasters", UIControlState.Normal);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                //await GetProfileInfo();
                //await GetProfilePic();
                await this.GetPhotos();

                ToasterPhotoCollectionView.Delegate = new BusinessPhotoDelegate(this);
                ToasterPhotosDataSource = new DataSource.Individuals.ToasterPhotosDataSource(this, this.ToasterPhotos, this.ImageViewImages);
                ToasterPhotoCollectionView.DataSource = ToasterPhotosDataSource;

                if (!FromSearchedUser)
                {
                    await GetToastersCount();
                }

            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetPhotoUris()
        {
            try
            {
                foreach (var b in this.ToasterPhotos)
                {
                    ImageViewImage logo = new ImageViewImage();
                    logo.Id = b.ToasterPhotoId;
                    var uriString = await BlobStorageHelper.GetToasterPhotosUri(b.UserId, b.ToasterPhotoId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        logo.ImageUrl = imageUri;
                    }
                    this.ImageViewImages.Add(logo);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetPhotos()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    //BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    var userId = FromSearchedUser ? SearchedUser.UserId : AppDelegate.CurrentUser.UserId;
                    var photos = await AppDelegate.ToasterPhotoFactory.Get(userId);
                    if (photos != null)
                    {
                        this.ToasterPhotos = photos.ToList();
                        await GetPhotoUris();
                    }

                    //BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetProfilePic()
        {
            try
            {
                ImageViewImage itemLogo = new ImageViewImage();
                var userId = FromSearchedUser ? SearchedUser.UserId : AppDelegate.CurrentUser.UserId;
                itemLogo.Id = userId;
                itemLogo.ImageUrl = new Uri(await BlobStorageHelper.GetToasterBlobUri(userId));

                if (itemLogo.Image == null)
                {
                    this.BeginDownloadingImage(itemLogo, ProfilePic);
                }

                ProfilePic.Image = itemLogo.Image;
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            // Release all cached images. This will cause them to be redownloaded
            // later as they're displayed.
            if (this.ImageViewImages != null)
            {
                foreach (var v in this.ImageViewImages)
                    v.Image = null;
            }
        }

        /// <summary>
        /// Load view
        /// </summary>
        /// <param name="animated"></param>
        public async override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);
                if (RequireRefresh)
                {
                    RequireRefresh = false;
                    AppDelegate.CurrentUser = AppDelegate.GetCurrentUser();
                    var fname = string.IsNullOrEmpty(AppDelegate.CurrentUser.FirstName) ? string.Empty : AppDelegate.CurrentUser.FirstName;
                    var lname = string.IsNullOrEmpty(AppDelegate.CurrentUser.LastName) ? string.Empty : AppDelegate.CurrentUser.LastName;
                    Name.Text = fname + " " + lname;
                    HomeTown.Text = UpdatedHometown;
                    Headline.Text = UpdatedHeadline;
                }

                if (ProfilePicUpdated)
                {
                    ProfilePicUpdated = false;
                    await GetProfilePic();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);

                if (this.TabBarController != null)
                {
                    this.TabBarController.NavigationController.NavigationBarHidden = false;
                    this.TabBarController.NavigationItem.SearchController = null;
                    SetTitle();
                }
                else
                {
                    this.NavigationController.NavigationBarHidden = false;
                    this.NavigationItem.SearchController = null;
                }

                if (!FromSearchedUser)
                {
                    this.TabBarController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Edit Profile", UIBarButtonItemStyle.Plain, (sender, args) =>
                    {
                        var controller = this.Storyboard.InstantiateViewController("EditToasterProfileController") as EditToasterProfileController;
                        this.NavigationController.PushViewController(controller, true);

                    }), true);
                }
                else
                {
                    if (this.TabBarController != null)
                    {
                        this.TabBarController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender, args) =>
                        {
                            MoreMenuActions();
                        }), true);
                    } else
                    {
                        this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender, args) =>
                        {
                            MoreMenuActions();
                        }), true);
                    }
                }            
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Set title
        /// </summary>
        private void SetTitle()
        {
            this.TabBarController.NavigationItem.Title = string.IsNullOrEmpty(AppDelegate.CurrentUser.Username) ? "Profile" : AppDelegate.CurrentUser.Username;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal async Task AcceptRequest(UIButton toasterRequest)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    if (SearchedUser == null)
                    {
                        return;
                    }
                    await AppDelegate.ToastersFactory.AcceptRequest(SearchedUser.UserId, AppDelegate.CurrentUser.UserId, SearchedUser.UserId, Toasters.ToasterRequestStatus.Accepted);
                    toasterRequest.Hidden = true;
                    ToastersController.RequiresRefresh = true;
                    await new PushNotificationHelper(AppDelegate.NotificationRegisterFactory, PushNotificationHelper.PushPlatform.iOS).ToasterAcceptedPush(AppDelegate.CurrentUser.FirstName, SearchedUser.UserId);
                    NavigationController.PopViewController(false);
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal async Task SendAddToaster(UIButton toasterRequest)
        {
            try
            {
                if(toasterRequest.Title(UIControlState.Normal) == AppText.Requested)
                {
                    return;
                }
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    if(SearchedUser == null)
                    {
                        return;
                    }
                    Toasters toaster = new Toasters();

                    toaster.RequestStatus = SearchedIndividualInfo != null && !this.SearchedIndividualInfo.PrivateAccount ? Toasters.ToasterRequestStatus.Accepted : Toasters.ToasterRequestStatus.Pending; //true : false;
                    toaster.UserOneId = AppDelegate.CurrentUser.UserId;
                    toaster.UserTwoId = SearchedUser.UserId;
                    toaster.ActionUserId = AppDelegate.CurrentUser.UserId;

                    await AppDelegate.ToastersFactory.AddToaster(toaster);
                    if (this.IndividualInfo != null)
                    {
                        if (this.IndividualInfo.PrivateAccount)
                        {
                            toasterRequest.SetTitle(AppText.Requested, UIControlState.Normal);
                            await new PushNotificationHelper(AppDelegate.NotificationRegisterFactory, PushNotificationHelper.PushPlatform.iOS).ToasterRequestPush(AppDelegate.CurrentUser.FirstName, SearchedUser.UserId);
                        }
                        else
                        {
                            toasterRequest.Hidden = true;
                            await new PushNotificationHelper(AppDelegate.NotificationRegisterFactory, PushNotificationHelper.PushPlatform.iOS).ToasterAddedPush(AppDelegate.CurrentUser.FirstName, SearchedUser.UserId);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportReason"></param>
        public async void ReportUserToTABS()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    Shared.Models.Reports.Users.ReportedUser reportedUser = new Shared.Models.Reports.Users.ReportedUser();
                    reportedUser.BlockedByAdmin = false;
                    reportedUser.BlockedByAdminUserId = 0;
                    reportedUser.ReportDate = DateTime.Now;
                    reportedUser.ReporterUserId = AppDelegate.CurrentUser.UserId;
                    reportedUser.ReporterFirstName = AppDelegate.CurrentUser.FirstName;
                    reportedUser.ReporterLastName = AppDelegate.CurrentUser.LastName;

                    //var checkinuser = await App.UsersFactory.GetUser(SearchedUser.UserId);

                    if (SearchedUser != null)
                    {
                        reportedUser.SenderUserId = SearchedUser.UserId;
                        reportedUser.SenderFirstName = SearchedUser.FirstName;
                        reportedUser.SenderLastName = SearchedUser.LastName;
                    }

                    await AppDelegate.ReportedUserFactory.ReportUser(reportedUser);

                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.GenericAccountReportMessage, Helpers.ToastTime.ErrorTime);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        private void ReportUser()
        {

            UIAlertController uIAlertController = new UIAlertController();
            uIAlertController = UIAlertController.Create(ToastMessage.ReportUserTitle, ToastMessage.ReportUserMessage, UIAlertControllerStyle.Alert);
            uIAlertController.AddAction(UIAlertAction.Create(AppText.Report, UIAlertActionStyle.Default, (action) => ReportUserToTABS()));
            uIAlertController.AddAction(UIAlertAction.Create(AppText.Cancel, UIAlertActionStyle.Cancel, null));
            this.PresentViewController(uIAlertController, true, null);
        }

        private async void BlockUser()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    if (Toaster == null)
                    {
                        return;
                    }
                    await AppDelegate.ToastersFactory.BlockToaster(AppDelegate.CurrentUser.UserId, SearchedUser.UserId, AppDelegate.CurrentUser.UserId);
                    ToasterRequest.Hidden = true;
                    ToastersController.RequiresRefresh = true;
                    NavigationController.PopViewController(false);
                }
            }
            catch (Exception)
            {

            }
        }

        private async void UnfollowUser()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    if (Toaster == null)
                    {
                        return;
                    }
                    await AppDelegate.ToastersFactory.UnfollowToaster(AppDelegate.CurrentUser.UserId, SearchedUser.UserId, AppDelegate.CurrentUser.UserId);
                    ToasterRequest.Hidden = true;
                    ToastersController.RequiresRefresh = true;
                    NavigationController.PopViewController(false);
                }
            }
            catch (Exception)
            {

            }
        }

        public void MoreMenuActions()
        {
            UIAlertController actionSheetAlert = UIAlertController.Create("", "", UIAlertControllerStyle.ActionSheet);
            var alertAction = new UIAlertAction();

            alertAction = UIAlertAction.Create("Report User", UIAlertActionStyle.Destructive, (action) => ReportUser());
            actionSheetAlert.AddAction(alertAction);

            if (Toaster != null)
            {
                var blockText = Toaster.RequestStatus == Toasters.ToasterRequestStatus.Blocked ? AppText.Unblock : AppText.Block;
                alertAction = UIAlertAction.Create(blockText, UIAlertActionStyle.Default, (action) => BlockUser());
                actionSheetAlert.AddAction(alertAction);

                if (Toaster.RequestStatus == Toasters.ToasterRequestStatus.Accepted)
                {
                    alertAction = UIAlertAction.Create("Unfollow", UIAlertActionStyle.Default, (action) => UnfollowUser());
                    //(action) => ModifyContainer(ContainerAction.Clone));
                    actionSheetAlert.AddAction(alertAction);
                }
            }

            // Cancel button
            actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            // Required for iPad - You must specify a source for the Action Sheet since it is displayed as a popover
            UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                // Set the source so the arrow is pointing to it (and centered)
                presentationPopover.SourceView = this.View;
                presentationPopover.SourceRect = new CGRect(this.View.Bounds.GetMidX(), this.View.Bounds.Bottom, 0, 0);
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            actionSheetAlert.View.AccessibilityIdentifier = "FilterActionSheet";

            // Display the alert
            this.PresentViewController(actionSheetAlert, true, null);
            actionSheetAlert.View.TintColor = UIColor.FromRGB(84, 92, 165);
        }

        #endregion

    }
}