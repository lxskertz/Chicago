using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Content.PM;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Newtonsoft.Json;
using Plugin.Media;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoAndroid.Adapters.Business;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Businesses
{
    [Activity(Label = "photos", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class BusinesPhotoActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        public SwipeRefreshLayout refresher;
        public FrameLayout pageLayout;
        private bool isBusiness = true;
        private int businessId = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Layout manager that lays out each card in the RecyclerView:
        /// </summary>
        private RecyclerView.LayoutManager PhotosLayoutManager { get; set; }

        /// <summary>
        /// Gets or sets the recycler view
        /// </summary>
        private RecyclerView PhotosRecycler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinesPhoto> BusinesPhotos { get; set; } = new List<BusinesPhoto>();

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImages { get; set; } = new List<ImageViewImage>();

        public Business BusinessInfo { get; set; }

        /// <summary>
        /// Gets or sets the adapater
        /// </summary>
        private BusinessPhotoAdaper BusinessPhotoAdaper { get; set; }

        #endregion

        #region Methods

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.BusinessPhotos);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                this.PhotosRecycler = FindViewById<RecyclerView>(Resource.Id.photosCardRecycler);
                //pageLayout = FindViewById<FrameLayout>(Resource.Id.photosLayout);

                //refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefresh);
                //refresher.Refresh += HandleRefresh;

                isBusiness = Intent.GetBooleanExtra("IsBusiness", true);
                businessId = Intent.GetIntExtra("BusinessId", 0);

                await LoadData();
            }
            catch (Exception) { }

        }

        /// <summary>
        /// Load data
        /// </summary>
        private async Task LoadData()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.Loading);

                    if (this.BusinessInfo == null)
                    {
                        if (!isBusiness)
                        {
                            BusinessInfo = await App.BusinessFactory.Get(businessId);
                        }
                        else
                        {

                            BusinessInfo = await App.BusinessFactory.GetByUserId(this.CurrentUser.UserId);
                        }
                    }

                    if (BusinessInfo != null)
                    {
                        var photos = await App.BusinessPhotoFactory.Get(BusinessInfo.BusinessId);
                        if (photos != null)
                        {
                            this.BusinesPhotos = photos.ToList();
                            await GetPhotoUris();
                            this.BusinessPhotoAdaper = new BusinessPhotoAdaper(this, BusinesPhotos, ImageViewImages);
                            this.PhotosLayoutManager = new LinearLayoutManager(this);
                            this.PhotosRecycler.SetItemAnimator(new DefaultItemAnimator());
                            this.PhotosRecycler.AddItemDecoration(new Helpers.PhotoItemDecorator(10));
                            this.PhotosRecycler.HasFixedSize = true;
                            this.PhotosRecycler.SetLayoutManager(this.PhotosLayoutManager);
                            this.PhotosRecycler.SetAdapter(this.BusinessPhotoAdaper);
                        }
                    }

                   

                    this.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetPhotoUris()
        {
            foreach (var b in this.BusinesPhotos)
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = b.BusinessPhotoId;
                Uri imageUri = new Uri(await Shared.Helpers.BlobStorageHelper.GetBusinessPhotosUri(b.BusinessId, b.BusinessPhotoId));         
                logo.ImageUrl = imageUri;
                this.ImageViewImages.Add(logo);
            }         
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuAction:
                    SelectPic();
                    return true;
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

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

                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.Loading);

                    if (this.BusinessInfo == null)
                    {
                        BusinessInfo = await App.BusinessFactory.GetByUserId(this.CurrentUser.UserId);
                    }

                    if (BusinessInfo != null)
                    {
                        BusinesPhoto businesPhoto = new BusinesPhoto();
                        businesPhoto.BusinessId = this.BusinessInfo.BusinessId;
                        var id = await App.BusinessPhotoFactory.Add(businesPhoto);

                        if (id > 0)
                        {
                            await Shared.Helpers.BlobStorageHelper.SaveBusinessPhotosBlob(file.Path, BusinessInfo.BusinessId, id);

                            if (BusinessPhotoAdaper == null)
                            {
                                await LoadData();
                            }
                            else
                            {
                                await BusinessPhotoAdaper.AddNewImage(BusinessInfo.BusinessId, id); //LoadData();
                            }
                        }
                    }
                    this.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            isBusiness = Intent.GetBooleanExtra("IsBusiness", true);
            if (isBusiness)
            {
                this.MenuInflater.Inflate(Resource.Menu.menu_with_text, menu);

                menu.FindItem(Resource.Id.menuAction).SetTitle(AppText.AddPhoto);
            }

            return base.OnCreateOptionsMenu(menu);
        }


        #endregion

    }
}