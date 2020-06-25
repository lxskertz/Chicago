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
    public partial class BusinessImagesController : BaseViewController
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
        private DataSource.Business.BusinessImagesDataSource BusinessImagesDataSource { get; set; }

        public List<BusinesPhoto> BusinesPhotos { get; set; } = new List<BusinesPhoto>();

        public Business BusinessInfo { get; set; }

        public bool IsBusiness { get; set; } = true;

        public int BusinessId { get; set; }

        public List<ImageViewImage> ImageViewImages { get; set; } = new List<ImageViewImage>();

        #endregion

        #region Constructors

        public BusinessImagesController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                if (IsBusiness)
                {
                    this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Add Photo", UIBarButtonItemStyle.Plain, async (sender, args) =>
                    {
                        AddPhoto();
                    }), true);
                }

                await LoadData();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void AddPhoto()
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

                if (!AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    if(this.BusinessInfo == null)
                    {
                        BusinessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);
                    }

                    if (BusinessInfo != null)
                    {
                        BusinesPhoto businesPhoto = new BusinesPhoto();
                        businesPhoto.BusinessId = this.BusinessInfo.BusinessId;
                        var id = await AppDelegate.BusinessPhotoFactory.Add(businesPhoto);

                        if(id > 0)
                        {
                            await BlobStorageHelper.SaveBusinessPhotosBlob(file.Path, BusinessInfo.BusinessId, id);

                            if (BusinessImagesDataSource == null)
                            {
                                await LoadData();
                            }
                            else
                            {
                                //this.BeginInvokeOnMainThread(() =>
                                //{
                                await AddNewImage(BusinessInfo.BusinessId, id);
                                    BusinessImagesDataSource.BusinesPhotos = BusinesPhotos;
                                    BusinessImagesDataSource.ImageViewImages = ImageViewImages;
                                    PhotosCollectionView.ReloadData();
                                //});
                            }
                        }
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
        private async Task LoadData()
        {
            try
            {
                await this.GetPhotos();
                PhotosCollectionView.Delegate = new BusinessPhotoDelegate();
                BusinessImagesDataSource = new DataSource.Business.BusinessImagesDataSource(this, this.BusinesPhotos, this.ImageViewImages);
                PhotosCollectionView.DataSource = BusinessImagesDataSource;
            }
            catch (Exception)
            {
            }
        }

        public async Task AddNewImage(int businessId, int photoId)
        {
            try
            {
                if (this.BusinesPhotos == null)
                {
                    this.BusinesPhotos = new List<BusinesPhoto>();
                }

                this.BusinesPhotos.Add(new BusinesPhoto() { BusinessId = businessId, BusinessPhotoId = photoId });

                ImageViewImage logo = new ImageViewImage();
                logo.Id = photoId;
                Uri imageUri = new Uri(await BlobStorageHelper.GetBusinessPhotosUri(businessId, photoId));
                logo.ImageUrl = imageUri;
                this.ImageViewImages.Add(logo);
            }
            catch (Exception)
            {
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
                foreach (var b in this.BusinesPhotos)
                {
                    ImageViewImage logo = new ImageViewImage();
                    logo.Id = b.BusinessPhotoId;
                    Uri imageUri = new Uri(await BlobStorageHelper.GetBusinessPhotosUri(b.BusinessId, b.BusinessPhotoId));
                    logo.ImageUrl = imageUri;
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
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);


                    if (this.BusinessInfo == null)
                    {
                        if (!IsBusiness)
                        {
                            BusinessInfo = await AppDelegate.BusinessFactory.Get(BusinessId);
                        }
                        else
                        {
                            BusinessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);
                        }
                    }

                    if (BusinessInfo != null)
                    {
                        var photos = await AppDelegate.BusinessPhotoFactory.Get(BusinessInfo.BusinessId);
                        if (photos != null)
                        {
                            this.BusinesPhotos = photos.ToList();
                            await GetPhotoUris();
                        }
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
            if (this.ImageViewImages != null)
            {
                foreach (var v in this.ImageViewImages)
                    v.Image = null;
            }
        }

        #endregion

    }
}