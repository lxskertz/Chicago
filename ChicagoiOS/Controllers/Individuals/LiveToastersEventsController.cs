using Foundation;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Models.Reports.Spams;
using BigTed;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class LiveToastersEventsController : BaseViewController
    {

        #region Constants, Enums, and Variables

        private UIRefreshControl RefreshControl;     
        public SearchParameters param = new SearchParameters();
        public bool loadMore = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Individuals.Events.LiveEventsDataSource LiveEventsDataSource { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Individuals.CheckIns.LiveToastersDataSource LiveToastersDataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessEvents> BusinessEvents { get; set; } = new List<BusinessEvents>();

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> EventsImageViewImage { get; set; } = new List<ImageViewImage>();

        /// <summary>
        /// 
        /// </summary>
        public List<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> CheckInsImageViewImage { get; set; } = new List<ImageViewImage>();

        public Dictionary<int, bool> EventsLikeList = new Dictionary<int, bool>();

        public Dictionary<int, bool> CheckInsLikeList = new Dictionary<int, bool>();

        public static bool RequiresRefresh { get; set; }

        #endregion

        #region Constructors

        public LiveToastersEventsController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                NoResultMessage.Hidden = true;
                RefreshControl = new UIRefreshControl();
                RefreshControl.ValueChanged += HandleValueChanged;
                ToastersLiveTable.AddSubview(RefreshControl);

                //this.TabBarController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Check-In", UIBarButtonItemStyle.Plain, async (sender, args) => {                
                //    var controller = this.Storyboard.InstantiateViewController("BusinessListController") as BusinessListController;
                //    this.NavigationController.PushViewController(controller, true);
                //}), true);
                //ToasterEventSegmentCtrl.SelectedSegment = 0;

                if (!AppDelegate.IsOfflineMode())
                {
                    var stripeKey = await AppDelegate.UsersFactory.GetStripeKey();
                    AppDelegate.CustomerPaymentInfoFactory.Initialize(stripeKey);
                    var storageKey = await AppDelegate.UsersFactory.GetStorageConnectionKey();
                    Shared.Helpers.BlobStorageHelper.ConnectionString = storageKey;

                    //await GeLocation();
                }
                await GetLastKnownLocation();

                LoadCheckIns();

            }
            catch (Exception)
            {
            }
        }

        private async Task GeLocation()
        {
            AppDelegate.CurrentAddy = await new LocationHelper().GetAddress();

            if (AppDelegate.CurrentAddy != null)
            {
                AppDelegate.City = string.IsNullOrEmpty(AppDelegate.CurrentAddy.Locality) ? "" : AppDelegate.CurrentAddy.Locality;
                AppDelegate.ZipCode = string.IsNullOrEmpty(AppDelegate.CurrentAddy.PostalCode) ? "" : AppDelegate.CurrentAddy.PostalCode;
                //state = string.IsNullOrEmpty(currentAddy.SubLocality) ? "" : currentAddy.AdminArea;
            }
        }

        private async Task GetLastKnownLocation()
        {
            //AppDelegate.CurrentAddy = new Plugin.Geolocator.Abstractions.Address();
            //AppDelegate.CurrentAddy.Locality = AppDelegate.City;
            //AppDelegate.CurrentAddy.PostalCode = AppDelegate.ZipCode;

            AppDelegate.CurrentAddy = await new LocationHelper().GetLastKnownAddress();
            if (AppDelegate.CurrentAddy == null)
            {
                AppDelegate.CurrentAddy = await new LocationHelper().GetAddress();
            }

            if (AppDelegate.CurrentAddy != null)
            {
                AppDelegate.City = string.IsNullOrEmpty(AppDelegate.CurrentAddy.Locality) ? "" : AppDelegate.CurrentAddy.Locality;
                AppDelegate.ZipCode = string.IsNullOrEmpty(AppDelegate.CurrentAddy.PostalCode) ? "" : AppDelegate.CurrentAddy.PostalCode;
                //state = string.IsNullOrEmpty(currentAddy.SubLocality) ? "" : currentAddy.AdminArea;
            }
        }

        private void ReportPost(CheckIn item)
        {
            ReportPostActions(item);
        }

        private async void BlockUser(CheckIn item)
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
                    if (item == null)
                    {
                        return;
                    }
                    await AppDelegate.ToastersFactory.BlockToaster(AppDelegate.CurrentUser.UserId, item.UserId, AppDelegate.CurrentUser.UserId);
                    await RefreshCheckIns();
                }
            }
            catch (Exception)
            {

            }
        }

        private async void UnfollowUser(CheckIn item)
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
                    if (item == null)
                    {
                        return;
                    }
                    await AppDelegate.ToastersFactory.UnfollowToaster(AppDelegate.CurrentUser.UserId, item.UserId, AppDelegate.CurrentUser.UserId);
                    await RefreshCheckIns();
                }
            }
            catch (Exception)
            {

            }
        }

        private async void ReportSpam(CheckIn item)
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
                    ReportedSpamCheckIn spamCheckIn = new ReportedSpamCheckIn();
                    spamCheckIn.BlockedByAdmin = false;
                    spamCheckIn.BlockedByAdminUserId = 0;
                    spamCheckIn.BusinessId = item.BusinessId;
                    spamCheckIn.BusinessName = item.BusinessName;
                    spamCheckIn.CheckInDate = item.CheckInDate;
                    spamCheckIn.CheckInDateString = item.CheckInDate.ToString();
                    spamCheckIn.CheckInId = item.CheckInId;
                    spamCheckIn.CheckInType = item.CheckInType;
                    spamCheckIn.CheckInUserId = item.UserId;
                    spamCheckIn.EventId = item.EventId;
                    spamCheckIn.ReporterUserId = AppDelegate.CurrentUser.UserId;
                    spamCheckIn.ReporterFirstName = AppDelegate.CurrentUser.FirstName;
                    spamCheckIn.ReporterLastName = AppDelegate.CurrentUser.LastName;

                    var checkinuser = await AppDelegate.UsersFactory.GetUser(item.UserId);

                    if (checkinuser != null)
                    {
                        spamCheckIn.SenderFirstName = checkinuser.FirstName;
                        spamCheckIn.SenderLastName = checkinuser.LastName;
                    }

                    await AppDelegate.ReportedSpamCheckInFactory.ReportSpam(spamCheckIn);

                    UIAlertController uIAlertController = new UIAlertController();
                    uIAlertController = UIAlertController.Create("", ToastMessage.SpamReportMessage, UIAlertControllerStyle.Alert);
                    uIAlertController.AddAction(UIAlertAction.Create(AppText.Ok, UIAlertActionStyle.Default, null));
                    this.PresentViewController(uIAlertController, true, null);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        private void InappropriatePost(CheckIn item)
        {
            var controller = this.Storyboard.InstantiateViewController("InappropraiteOptionsController") as InappropraiteOptionsController;
            controller.CheckInItem = item;
            this.NavigationController.PushViewController(controller, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void MoreMenuActions(CheckIn item)
        {
            UIAlertController actionSheetAlert = UIAlertController.Create("", "", UIAlertControllerStyle.ActionSheet);
            var alertAction = new UIAlertAction();

            alertAction = UIAlertAction.Create("Report Post", UIAlertActionStyle.Destructive, (action) => ReportPost(item));
            actionSheetAlert.AddAction(alertAction);

            alertAction = UIAlertAction.Create("Block", UIAlertActionStyle.Default, (action) => BlockUser(item));
            actionSheetAlert.AddAction(alertAction);

            alertAction = UIAlertAction.Create("Unfollow", UIAlertActionStyle.Default, (action) => UnfollowUser(item));
            //(action) => ModifyContainer(ContainerAction.Clone));
            actionSheetAlert.AddAction(alertAction);

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

        public void ReportPostActions(CheckIn item)
        {
            UIAlertController actionSheetAlert = UIAlertController.Create("", "", UIAlertControllerStyle.ActionSheet);
            var alertAction = new UIAlertAction();

            alertAction = UIAlertAction.Create(AppText.ItsInappropriate, UIAlertActionStyle.Destructive, (action) => InappropriatePost(item));
            actionSheetAlert.AddAction(alertAction);

            alertAction = UIAlertAction.Create(AppText.ItsSpam, UIAlertActionStyle.Destructive, (action) => ReportSpam(item));
            actionSheetAlert.AddAction(alertAction);

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
        private void OpenCheckIn()
        {
            var controller = this.Storyboard.InstantiateViewController("BusinessListController") as BusinessListController;
            this.NavigationController.PushViewController(controller, true);
        }

        private void OpenSentDrinks()
        {
            var controller = this.Storyboard.InstantiateViewController("OrdersController") as OrdersController;
            controller.ToasterOrderEnum = Shared.Models.Orders.ToasterOrder.ToasterOrderEnum.Sender;
            this.NavigationController.PushViewController(controller, true);
        }

        private void OpenReceivedDrinks()
        {
            var controller = this.Storyboard.InstantiateViewController("OrdersController") as OrdersController;
            controller.ToasterOrderEnum = Shared.Models.Orders.ToasterOrder.ToasterOrderEnum.Receiver;
            this.NavigationController.PushViewController(controller, true);
        }


        /// <summary>
        /// Container action
        /// </summary>
        /// <param name="navController"></param>
        /// <param name="sender"></param>
        private void OpenActionMenu()
        {
            UIAlertController actionSheetAlert = UIAlertController.Create("", "", UIAlertControllerStyle.ActionSheet);
            var alertAction = new UIAlertAction();

            alertAction = UIAlertAction.Create("Check In", UIAlertActionStyle.Default, (action) => OpenCheckIn());
            actionSheetAlert.AddAction(alertAction);

            alertAction = UIAlertAction.Create("Received Drinks", UIAlertActionStyle.Default, (action) => OpenReceivedDrinks());
            //(action) => ModifyContainer(ContainerAction.Clone));
            actionSheetAlert.AddAction(alertAction);

            alertAction = UIAlertAction.Create("Sent Drinks", UIAlertActionStyle.Default, (action) => OpenSentDrinks());
            //(action) => ModifyContainer(ContainerAction.Clone));
            actionSheetAlert.AddAction(alertAction);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);
                //this.TabBarController.NavigationController.NavigationBarHidden = true;

                //this.TabBarController.NavigationController.NavigationBarHidden = false;
                this.TabBarController.NavigationItem.SearchController = null;
               
                var drinkImage = UIButton.FromType(UIButtonType.RoundedRect);
                drinkImage.Frame = new CGRect(0, 0, 30, 30);
                drinkImage.SizeToFit();
                drinkImage.SetImage(UIImage.FromBundle("local_bar_white_24pt"), UIControlState.Normal);
                var drinkItem = new UIBarButtonItem(drinkImage);

                drinkImage.TouchUpInside += delegate
                {
                    OpenActionMenu();
                };
                this.TabBarController.NavigationItem.SetRightBarButtonItem(drinkItem, true);

                SetTitle();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public async override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    ToasterEventSegmentCtrl.SelectedSegment = 0;

                    if (AppDelegate.IsOfflineMode())
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                        return;
                    }

                    await this.GetCheckIns();
                    if (this.CheckIns != null && this.CheckIns.Count > 0)
                    {
                        //if (LiveToastersDataSource == null)
                        //{
                        HideShowNoResult(false, true);
                        ToastersLiveTable.EstimatedRowHeight = 258f;
                        ToastersLiveTable.RowHeight = UITableView.AutomaticDimension;
                        LiveToastersDataSource = new DataSource.Individuals.CheckIns.LiveToastersDataSource(this, this.CheckIns, this.CheckInsImageViewImage);
                        ToastersLiveTable.Source = LiveToastersDataSource;
                        ToastersLiveTable.TableFooterView = new UIView();
                        //} else
                        //{
                        //    this.LiveToastersDataSource.CheckIns = this.CheckIns;
                        //    this.LiveToastersDataSource.ImageViewImage = this.CheckInsImageViewImage;
                        //    ToastersLiveTable.ReloadData();
                        //}
                    }
                    else
                    {
                        HideShowNoResult(true, true);
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        private void HideShowNoResult(bool show, bool checkIn)
        {
            NoResultMessage.Hidden = !show;
            NoResultMessage.Text = checkIn ? ToastMessage.NoLiveCheckin : ToastMessage.NoLiveEvent;
            ToastersLiveTable.Hidden = !NoResultMessage.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        private async void LoadCheckIns()
        {
            try
            {
                await this.GetCheckIns();
                if (this.CheckIns != null && this.CheckIns.Count > 0)
                {
                    HideShowNoResult(false, true);
                    ToastersLiveTable.EstimatedRowHeight = 258f;
                    ToastersLiveTable.RowHeight = UITableView.AutomaticDimension;
                    LiveToastersDataSource = new DataSource.Individuals.CheckIns.LiveToastersDataSource(this, this.CheckIns, this.CheckInsImageViewImage);
                    ToastersLiveTable.Source = LiveToastersDataSource;
                    ToastersLiveTable.TableFooterView = new UIView();
                }
                else
                {
                    HideShowNoResult(true, true);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void LoadLiveEvents()
        {
            try
            {
                await this.GetEvents();
                if (this.BusinessEvents != null && this.BusinessEvents.Count > 0)
                {
                    HideShowNoResult(false, false);
                    ToastersLiveTable.EstimatedRowHeight = 299f;
                    ToastersLiveTable.RowHeight = UITableView.AutomaticDimension;
                    LiveEventsDataSource = new DataSource.Individuals.Events.LiveEventsDataSource(this, this.BusinessEvents, this.EventsImageViewImage);
                    ToastersLiveTable.Source = LiveEventsDataSource;
                    ToastersLiveTable.TableFooterView = new UIView();
                }
                else
                {
                    HideShowNoResult(true, false);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        private void InitSearchParameters(string searchTerm)
        {
            param = new SearchParameters();
            param.PageSize = 4;
            param.SearchTerm = searchTerm;
            param.PageNumber = 0;
            param.City = AppDelegate.City;
            param.ZipCode = AppDelegate.ZipCode;
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetEvents()
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
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    loadMore = true;
                    InitSearchParameters("");
                    var events = await AppDelegate.BusinessEventsFactory.GetLiveEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize);
                    if (events != null)
                    {
                        this.BusinessEvents = events.ToList();
                        await GetEventLogoUris();
                        await GetLikeEventList();
                    }

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// Called when recycler view is scrolled to the bottom 
        /// </summary>
        public async Task ScrolledToBottom()
        {
            if (!AppDelegate.IsOfflineMode() && loadMore)
            {
                try
                {
                    param.PageNumber += this.LiveEventsDataSource.BusinessEvents.Count;
                    var events = await AppDelegate.BusinessEventsFactory.GetLiveEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize);
                    if (events != null && events.Count > 0)
                    {
                        this.LiveEventsDataSource.AddRowItems(events.ToList());
                        ToastersLiveTable.ReloadData();
                    }
                    else
                    {
                        loadMore = false;
                    }
                }
                catch (Exception) { }
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetCheckIns()
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
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    var checkIns = await AppDelegate.CheckInFactory.GetCheckIns(AppDelegate.CurrentUser.UserId);
                    if (checkIns != null)
                    {
                        this.CheckIns = checkIns.ToList();
                        await GetCheckInLogoUris();
                        await GetLikeCheckInList();
                    }

                    BTProgressHUD.Dismiss();
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
        public override void DidReceiveMemoryWarning()
        {
            // Release all cached images. This will cause them to be redownloaded
            // later as they're displayed.
            if (this.EventsImageViewImage != null)
            {
                foreach (var v in this.EventsImageViewImage)
                    v.Image = null;
            }
            if (this.CheckInsImageViewImage != null)
            {
                foreach (var v in this.CheckInsImageViewImage)
                    v.Image = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetEventLogoUris()
        {
            try
            {
                foreach (var b in this.BusinessEvents)
                {
                    ImageViewImage logo = new ImageViewImage();
                    logo.Id = b.EventId;
                    var uriString = await Shared.Helpers.BlobStorageHelper.GetEventLogoUri(b.EventId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        logo.ImageUrl = imageUri;
                        this.EventsImageViewImage.Add(logo);
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
        /// <returns></returns>
        public async Task GetLikeEventList()
        {
            foreach (var b in this.BusinessEvents)
            {
                var liked = await AppDelegate.EventLikesFactory.GetEventLike(AppDelegate.CurrentUser.UserId, b.EventId);

                if (liked != null)
                {
                    AddRemoveEventLike(liked.Liked, b.EventId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <param name="eventId"></param>
        public void AddRemoveEventLike(bool like, int eventId)
        {
            if (EventsLikeList.ContainsKey(eventId))
            {
                EventsLikeList[eventId] = like;
            } else
            {
                EventsLikeList.Add(eventId, like);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetCheckInLogoUris()
        {
            foreach (var b in this.CheckIns)
            {

                ImageViewImage logo = new ImageViewImage();
                logo.Id = b.CheckInId;

                var uriString = await Shared.Helpers.BlobStorageHelper.GetCheckIntLogoUri(b.CheckInId);
                if (!string.IsNullOrEmpty(uriString))
                {
                    Uri imageUri = new Uri(uriString);
                    logo.ImageUrl = imageUri;
                    this.CheckInsImageViewImage.Add(logo);
                } else
                {

                    var userUriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(b.UserId);
                    if (!string.IsNullOrEmpty(userUriString))
                    {
                        Uri imageUri = new Uri(userUriString);
                        logo.ImageUrl = imageUri;
                        this.CheckInsImageViewImage.Add(logo);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLikeCheckInList()
        {
            foreach (var b in this.CheckIns)
            {
                var liked = await AppDelegate.CheckInLikesFactory.GetCheckInLike(AppDelegate.CurrentUser.UserId, b.CheckInId);

                if (liked != null)
                {
                    AddRemoveCheckInLike(liked.Liked, b.CheckInId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <param name="eventId"></param>
        public void AddRemoveCheckInLike(bool like, int checkinId)
        {
            if (CheckInsLikeList.ContainsKey(checkinId))
            {
                CheckInsLikeList[checkinId] = like;
            }
            else
            {
                CheckInsLikeList.Add(checkinId, like);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task RefreshLiveEvents()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    //try
                    //{
                    RefreshControl.BeginRefreshing();

                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    LoadLiveEvents();
                    //var events = await AppDelegate.BusinessEventsFactory.GetLiveEvents("60616", "Chicago");
                    //if (events != null && events.Count > 0)
                    //{
                    //    this.BusinessEvents = events.ToList();
                    //    await GetEventLogoUris();
                    //    await GetLikeEventList();
                    //    this.LiveEventsDataSource.BusinessEvents = this.BusinessEvents;
                    //    this.LiveEventsDataSource.ImageViewImage = this.EventsImageViewImage;
                    //    ToastersLiveTable.ReloadData();
                    //}

                    RefreshControl.EndRefreshing();

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task RefreshCheckIns()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    //try
                    //{
                    RefreshControl.BeginRefreshing();

                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    LoadCheckIns();

                    RefreshControl.EndRefreshing();

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void HandleValueChanged(object sender, EventArgs e)
        {
            try
            {
                var index = ToasterEventSegmentCtrl.SelectedSegment;
                if (index == 1)
                {
                    await RefreshLiveEvents();
                }
                else if (index == 0)
                {
                    await RefreshCheckIns();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        async partial void ToasterEventSegmentCtrl_ValueChanged(UIKit.UISegmentedControl sender)
        {
            try
            {
                var index = ToasterEventSegmentCtrl.SelectedSegment;
                if (index == 1)
                {
                    await RefreshLiveEvents();
                }
                else if (index == 0)
                {
                    await RefreshCheckIns();
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
            this.TabBarController.NavigationItem.Title = "Live";
        }

        #endregion

    }
}