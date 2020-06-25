using Foundation;
using System;
using CoreGraphics;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.Shared.Models.Events;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class EventInfoController : BaseViewController
    {

        #region Constants, Enums, and Variables


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Business.Events.EventsInfoDataSource EventsInfoDataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessEvents BusinessEvents { get; set; } = new BusinessEvents();

        /// <summary>
        /// 
        /// </summary>
        public Business BusinessInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Rsvp ExisitingRsvp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowToolbar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UIImage EventImage { get; set; }

        public bool IsBusiness { get; set; } = false;

        public static BusinessEvents UpdateBusinessEvent { get; set; }

        public static bool RequiresRefresh { get; set; }

        #endregion

        #region Constructors

        public EventInfoController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                EventInfoToolbar.Hidden = !ShowToolbar;

                if (IsBusiness)
                {
                    this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender, args) =>
                    {
                        OpenActionMenu();
                    }), true);
                }

                LoadData();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenRsvps()
        {
            var controller = this.Storyboard.InstantiateViewController("BusinessRsvpController") as BusinessRsvpController;
            controller.BusinessEvents = this.BusinessEvents;
            this.NavigationController.PushViewController(controller, true);
        }

        private void OpenEvent()
        {
            var controller = this.Storyboard.InstantiateViewController("EventNameDescController") as EventNameDescController;
            controller.Mode = BusinessEvents.ActionMode.Edit;
            controller.BusinessEvent = this.BusinessEvents;
            this.NavigationController.PushViewController(controller, true);
        }

        private void OpenCheckIns()
        {
            var controller = this.Storyboard.InstantiateViewController("BusinessCheckInsController") as BusinessCheckInsController;
            controller.ScreenCheckInType = Shared.Models.CheckIns.CheckIn.CheckInTypes.Event;
            controller.EventId = this.BusinessEvents.EventId;
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

            //alertAction = UIAlertAction.Create(AppText.LiveEventCheckIns, UIAlertActionStyle.Default, (action) => OpenCheckIns());
            //actionSheetAlert.AddAction(alertAction);

            alertAction = UIAlertAction.Create(AppText.ViewRsvps, UIAlertActionStyle.Default, (action) => OpenRsvps());
            actionSheetAlert.AddAction(alertAction);

            alertAction = UIAlertAction.Create(AppText.EditEvent, UIAlertActionStyle.Default, (action) => OpenEvent());
            actionSheetAlert.AddAction(alertAction);

            // Cancel button
            actionSheetAlert.AddAction(UIAlertAction.Create(AppText.Cancel, UIAlertActionStyle.Cancel, null));

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
            base.ViewDidAppear(animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    if (UpdateBusinessEvent != null)
                    {
                        this.BusinessEvents = UpdateBusinessEvent;
                    }
                    LoadData();
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task GetEventPic()
        {
            try
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = this.BusinessEvents.EventId;
                Uri imageUri = new Uri(await BlobStorageHelper.GetEventLogoUri(this.BusinessEvents.EventId));
                logo.ImageUrl = imageUri;

                if (logo.Image == null)
                {
                    this.BeginDownloadingImage(logo, EVentLogo);
                }

                EVentLogo.Image = logo.Image;
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        private async void LoadData()
        {
            try
            {
                EventInfoTable.EstimatedRowHeight = 83f;

                if (this.BusinessEvents != null)
                {
                    EventTitle.Text = string.IsNullOrEmpty(this.BusinessEvents.Title) ? "" : this.BusinessEvents.Title;


                    if (!AppDelegate.IsOfflineMode())
                    {
                        if (this.EventImage != null)
                        {
                            EVentLogo.Image = this.EventImage;
                        }
                        else
                        {
                            await GetEventPic();
                        }

                        EVentLogo.ClipsToBounds = true;
                        this.BusinessInfo = await AppDelegate.BusinessFactory.Get(this.BusinessEvents.BusinessId);

                        if (this.BusinessInfo != null)
                        {
                            EventOwnerName.Text = string.IsNullOrEmpty(this.BusinessInfo.BusinessName) ? "" : "By " + this.BusinessInfo.BusinessName;
                        }

                        if (ShowToolbar)
                        {
                            var eventId = this.BusinessEvents == null ? 0 : this.BusinessEvents.EventId;
                            this.ExisitingRsvp = await AppDelegate.RsvpFactory.GetToasterRsvp(AppDelegate.CurrentUser.UserId, eventId);

                            if (this.ExisitingRsvp != null)
                            {
                                UpdateItemText(this.ExisitingRsvp.Going);
                            }
                        }

                    }
                }

                EventInfoTable.RowHeight = UITableView.AutomaticDimension;
                EventsInfoDataSource = new DataSource.Business.Events.EventsInfoDataSource(this, this.BusinessEvents);
                EventInfoTable.Source = EventsInfoDataSource;
                EventInfoTable.TableFooterView = new UIView();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            // Release all cached images. This will cause them to be redownloaded
            // later as they're displayed.
            if (this.EventImage != null)
            {
                this.EventImage = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        async partial void Yes_Activated(UIBarButtonItem sender)
        {
            if (Yes.Title.ToLower() == "yes")
            {
                await AddRSVP(true);
                UpdateItemText(true);
            } else
            {
                Change();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        async partial void No_Activated(UIBarButtonItem sender)
        {
            await AddRSVP(false);
            UpdateItemText(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yes"></param>
        private void UpdateItemText(bool yes)
        {
            if (yes)
            {
                AreYouGoing.Title = AppText.YouAreGoing;
                Yes.Title = AppText.Change;
                No.Title = "";
                No.Enabled = false;
            }
            else
            {
                AreYouGoing.Title = AppText.YouAreNotGoing;
                Yes.Title = AppText.Change;
                No.Title = "";
                No.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Change()
        {
            Yes.Title = AppText.Yes;
            No.Title = AppText.No;
            No.Enabled = true;
            Yes.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="going"></param>
        /// <returns></returns>
        private async Task AddRSVP(bool going)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                } else
                {
                    if (this.BusinessEvents != null)
                    {
                        BTProgressHUD.Show(ToastMessage.Saving, -1f, ProgressHUD.MaskType.Black);
                        Rsvp rsvp = new Rsvp();
                        rsvp.Going = going;
                        rsvp.EventId = this.BusinessEvents.EventId;
                        rsvp.BusinessId = this.BusinessInfo != null ? this.BusinessInfo.BusinessId : 0;
                        rsvp.UserId = AppDelegate.CurrentUser.UserId;

                        this.ExisitingRsvp = await AppDelegate.RsvpFactory.GetToasterRsvp(AppDelegate.CurrentUser.UserId, rsvp.EventId);

                        if (this.ExisitingRsvp == null)
                        {
                            await AppDelegate.RsvpFactory.Add(rsvp);
                        } else
                        {
                            await AppDelegate.RsvpFactory.Change(going, rsvp.UserId, rsvp.EventId);
                        }
                        BTProgressHUD.Dismiss();
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        #endregion

    }
}