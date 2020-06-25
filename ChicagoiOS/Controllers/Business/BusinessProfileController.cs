using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models;
using BigTed;
using Plugin.Media;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessProfileController : BaseViewController
    {

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Business.BusinessProfileDataSource BusinessProfileDataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Address AddressInfo { get; set; } = new Address();

        /// <summary>
        /// 
        /// </summary>
        public BusinessTypes BusinessTypes { get; set; } = new BusinessTypes();

        /// <summary>
        /// 
        /// </summary>
        public Business BusinessInfo { get; set; } = new Business();

        /// <summary>
        /// 
        /// </summary>
        public BusinessSearch BusinessSearchInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UIImage BusinessSearchInfoImage { get; set; }

        public static bool RequiresRefresh { get; set; }

        #endregion

        #region Constructors

        public BusinessProfileController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            if (this.BusinessSearchInfoImage != null)
            {
                this.BusinessSearchInfoImage = null;
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
                await GetProfileInfo();
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
                    await GetProfileInfo();
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
                    this.TabBarController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender, args) =>
                    {
                        OpenActionMenu();
                    }), true);
                    this.TabBarController.NavigationItem.SearchController = null;
                    SetTitle();
                }
                else
                {
                    this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender, args) =>
                    {
                        OpenActionMenu();
                    }), true);
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
            if (TabBarController != null)
            {
                this.TabBarController.NavigationItem.Title = "Profile";
            } else
            {
                this.NavigationItem.Title = "Profile";
            }
        }

        private void OpenEditProfile()
        {
            if (this.BusinessTypes != null && this.AddressInfo != null &&
                this.BusinessInfo != null)
            {
                var controller = this.Storyboard.InstantiateViewController("BusinessRegistrationController") as BusinessRegistrationController;
                controller.EditProfile = true;
                controller.BusinessInfo = this.BusinessInfo;
                controller.AddressInfo = this.AddressInfo;
                controller.BusinessTypes = this.BusinessTypes;
                this.NavigationController.PushViewController(controller, true);
            }
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

            if (this.BusinessSearchInfo == null)
            {
                alertAction = UIAlertAction.Create("Change Logo", UIAlertActionStyle.Default, (action) => SelectPic());
                actionSheetAlert.AddAction(alertAction);

                alertAction = UIAlertAction.Create("Upload Pictures", UIAlertActionStyle.Default, (action) => OpenPhotos());
                actionSheetAlert.AddAction(alertAction);


                alertAction = UIAlertAction.Create("Edit Profile", UIAlertActionStyle.Default, (action) => OpenEditProfile());
                actionSheetAlert.AddAction(alertAction);
            } else
            {
                alertAction = UIAlertAction.Create("Photos", UIAlertActionStyle.Default, (action) => OpenPhotos());
                actionSheetAlert.AddAction(alertAction);
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

        private void OpenPhotos()
        {
            var controller = this.Storyboard.InstantiateViewController("BusinessImagesController") as BusinessImagesController;
            controller.IsBusiness = this.BusinessSearchInfo == null;
            controller.BusinessId = this.BusinessSearchInfo == null ? 0 : this.BusinessSearchInfo.BusinessId;
            this.NavigationController.PushViewController(controller, true);
        }

        /// <summary>
        /// 
        /// </summary>
        private async void SelectPic()
        {
            try
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });


                if (file == null)
                {
                    return;
                }

                UIImage image = new UIImage(file.Path);
                ProfilePicture.Image = image;
                ProfilePicture.ClipsToBounds = true;
                await AddCheckInImage(file);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Add event logo
        /// </summary>
        private async Task AddCheckInImage(Plugin.Media.Abstractions.MediaFile picFile)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }

                BTProgressHUD.Show(ToastMessage.PleaseWait, -1f, ProgressHUD.MaskType.Black);

                if (BusinessInfo != null)
                {
                    await BlobStorageHelper.SaveBusinessLogoBlob(picFile.Path, BusinessInfo.BusinessId);
                }

                BTProgressHUD.Dismiss();
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
        private async Task GetProfilePic()
        {
            try
            {
                ImageViewImage itemLogo = new ImageViewImage();
                itemLogo.Id = BusinessInfo.BusinessId;
                itemLogo.ImageUrl = new Uri(await BlobStorageHelper.GetBusinessLogoUri(BusinessInfo.BusinessId));

                if (itemLogo.Image == null)
                {                    
                    this.BeginDownloadingImage(itemLogo, ProfilePicture);
                }
                ProfilePicture.ClipsToBounds = true;
                ProfilePicture.Image = itemLogo.Image;
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
        private async Task GetProfileInfo()
        {
            try
            {
                if (this.BusinessSearchInfo != null)
                {

                    BusinessProfileTable.EstimatedRowHeight = 44f;
                    BusinessProfileTable.RowHeight = UITableView.AutomaticDimension;
                    BusinessProfileDataSource = new DataSource.Business.BusinessProfileDataSource(this, this.BusinessSearchInfo);
                    BusinessProfileTable.Source = BusinessProfileDataSource;
                    BusinessProfileTable.TableFooterView = new UIView();
                    ProfilePicture.Image = this.BusinessSearchInfoImage;
                    ProfilePicture.ClipsToBounds = true;
                }
                else
                {
                    if (AppDelegate.IsOfflineMode())
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                        return;
                    }
                    else
                    {
                        BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                        BusinessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);
                        if (BusinessInfo != null)
                        {
                            this.BusinessInfo = BusinessInfo;
                            var addressInfo = await AppDelegate.AddressFactory.GetAddressByBusinessId(BusinessInfo.BusinessId);

                            if (addressInfo != null)
                            {
                                this.AddressInfo = addressInfo;
                            }

                            var bizTypes = await AppDelegate.BusinessTypesFactory.GetBusinessType(BusinessInfo.BusinessId);

                            if (bizTypes != null)
                            {
                                this.BusinessTypes = bizTypes;
                            }

                            BTProgressHUD.Dismiss();

                            if (this.BusinessProfileDataSource == null)
                            {
                                BusinessProfileTable.EstimatedRowHeight = 44f;
                                BusinessProfileTable.RowHeight = UITableView.AutomaticDimension;
                                BusinessProfileDataSource = new DataSource.Business.BusinessProfileDataSource(this, this.BusinessInfo,
                                    this.AddressInfo, this.BusinessTypes);
                                BusinessProfileTable.Source = BusinessProfileDataSource;
                                BusinessProfileTable.TableFooterView = new UIView();
                                await GetProfilePic();
                            }
                            else
                            {
                                this.BusinessProfileDataSource.BusinessInfo = BusinessInfo;
                                this.BusinessProfileDataSource.AddressInfo = AddressInfo;
                                this.BusinessProfileDataSource.BusinessTypes = BusinessTypes;
                                BusinessProfileTable.ReloadData();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        #endregion

    }
}