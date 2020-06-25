using Foundation;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;
using CoreGraphics;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToastersController : BaseViewController
    {

        #region Constants, Enums, and Variables

        //UISearchController search;
        public SearchParameters param = new SearchParameters();
        public bool loadMore = true;
        public bool pendingRequestShown;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Individuals.ToastersDataSource ToastersDataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Individual IndividualInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool RequiresRefresh { get; set; }

        private ICollection<Toasters> Toasters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private List<ImageViewImage> ImageViewImages { get; set; } = new List<ImageViewImage>();

        #endregion

        #region Constructors

        public ToastersController(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        partial void InviteContact_TouchUpInside(UIButton sender)
        {
            var controller = this.Storyboard.InstantiateViewController("InviteContactController") as InviteContactController;
            this.NavigationController.PushViewController(controller, true);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitTableView(List<Toasters> data)
        { 
            ToastersTable.EstimatedRowHeight = 88f;
            ToastersTable.RowHeight = UITableView.AutomaticDimension;
            ToastersDataSource = new DataSource.Individuals.ToastersDataSource(this, data, this.ImageViewImages);
            ToastersTable.Source = ToastersDataSource;
            ToastersTable.TableFooterView = new UIView();
        }

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(AppText.More, UIBarButtonItemStyle.Plain, (sender, args) =>
                {
                    ResetTable();
                }), true);

                //this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender, args) =>
                //{
                //    //MoreMenuActions();
                //    ResetTable();
                //}), true);

                await RetriveToasters();
            }
            catch (Exception)
            {
            }
        }

        public async override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    await RetriveToasters();
                }
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
            if (this.ImageViewImages != null)
            {
                foreach (var v in this.ImageViewImages)
                    v.Image = null;
            }
        }

        private async void ResetTable()
        {
            try
            {
                if (!pendingRequestShown || this.NavigationItem.RightBarButtonItem.Title.ToLower() == AppText.More)
                {
                    //await GetPendingRequest();
                    MoreMenuActions();
                }
                else
                {
                    this.Title = "Toasters";
                    await RetriveToasters();
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
            base.ViewDidAppear(animated);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        private void InitSearchParameters(string searchTerm)
        {
            param = new SearchParameters();
            param.PageSize = 15;
            param.SearchTerm = searchTerm;
            param.PageNumber = 0;
            param.Id = AppDelegate.CurrentUser.UserId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetPicUris()
        {
            try
            {
                foreach (var b in this.Toasters)
                {
                    ImageViewImage itemLogo = new ImageViewImage();
                    itemLogo.Id = b.UserId;

                    var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(b.UserId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        itemLogo.ImageUrl = imageUri;
                        this.ImageViewImages.Add(itemLogo);
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
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task RetriveToasters()
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
                    loadMore = true;
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                    this.IndividualInfo = await AppDelegate.IndividualFactory.GetToasterByUserId(AppDelegate.CurrentUser.UserId);
                    InitSearchParameters("");
                    Toasters = await AppDelegate.ToastersFactory.GetToasters(param);

                    if (Toasters != null && Toasters.Count > 0)
                    {
                        pendingRequestShown = false;
                        this.NavigationItem.RightBarButtonItem.Title = AppText.More;
                        await GetPicUris();
                        if (ToastersDataSource == null)
                        {
                            InitTableView(Toasters.ToList());
                        }
                        else
                        {
                            this.InvokeOnMainThread(() =>
                            {
                                this.ToastersDataSource.Rows = Toasters.ToList();
                                this.ToastersDataSource.ImageViewImages = this.ImageViewImages;
                                ToastersTable.ReloadData();
                            });
                        }
                        BTProgressHUD.Dismiss();
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NullResult, Helpers.ToastTime.ErrorTime);
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async void GetPendingRequest()
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
                    loadMore = true;
                    InitSearchParameters("");
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                    Toasters = await AppDelegate.ToastersFactory.GetPendingToasters(param);

                    if (Toasters != null && Toasters.Count > 0)
                    {
                        this.Title = "Requests";
                        pendingRequestShown = true;
                        this.NavigationItem.RightBarButtonItem.Title = AppText.AcceptedRequest;
                        await GetPicUris();
                        if (ToastersDataSource == null)
                        {
                            InitTableView(Toasters.ToList());
                        }
                        else
                        {
                            this.InvokeOnMainThread(() =>
                            {
                                this.ToastersDataSource.Rows = Toasters.ToList();
                                this.ToastersDataSource.ImageViewImages = this.ImageViewImages;
                                ToastersTable.ReloadData();
                            });
                        }
                        BTProgressHUD.Dismiss();
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoPendingRequests, Helpers.ToastTime.ErrorTime);
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        private void BlockedToasters()
        {
            var controller = this.Storyboard.InstantiateViewController("BlockedUsersController") as BlockedUsersController;
            this.NavigationController.PushViewController(controller, true);
        }

        public void MoreMenuActions()
        {
            UIAlertController actionSheetAlert = UIAlertController.Create("", "", UIAlertControllerStyle.ActionSheet);
            var alertAction = new UIAlertAction();

            alertAction = UIAlertAction.Create(AppText.PendingRequest, UIAlertActionStyle.Default, (action) => GetPendingRequest());
            actionSheetAlert.AddAction(alertAction);

            alertAction = UIAlertAction.Create(AppText.BlockedToasters, UIAlertActionStyle.Default, (action) => BlockedToasters());
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
        /// Called when recycler view is scrolled to the bottom 
        /// </summary>
        public async Task ScrolledToBottom()
        {
            if (!AppDelegate.IsOfflineMode() && loadMore)
            {
                try
                {
                    ICollection<Toasters> results = null;
                    param.PageNumber += this.ToastersDataSource.Rows.Count;

                    if (pendingRequestShown)
                    {
                        results = await AppDelegate.ToastersFactory.GetPendingToasters(param);
                    }
                    else
                    {
                        results = await AppDelegate.ToastersFactory.GetToasters(param);
                    }

                    if (results != null && results.Count > 0)
                    {
                        this.ToastersDataSource.AddRowItems(results.ToList());
                        ToastersTable.ReloadData();
                    }
                    else
                    {
                        loadMore = false;
                    }
                }
                catch (Exception) { }
            }
        }

        #endregion

    }
}