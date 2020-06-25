using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Businesses;
using Plugin.Media;
using Newtonsoft.Json;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Business
{
    public class BusinessProfileFragment : BaseBusinessFragment
    {
        #region Constants, Enums, and Variables

        // Create a new instance field for this activity.
        private static BusinessProfileFragment instance;
        private TextView businessName;
        private TextView businessAddress;
        private TextView phoneNumber;
        private TextView businessTypes;
        private CoordinatorLayout parentLayout;
        private ImageView profilePicture;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Shared.Models.Businesses.Business BusinessInfo { get; set; }

        public BusinessTypes BusinessTypes { get; set; }

        public Address AddressInfo { get; set; }

        public static bool RequiresRefresh { get; set; }

        #endregion

        #region Constructors

        public BusinessProfileFragment(Activities.Businesses.BusinessHomeActivity context)
        {
            this.HomeContext = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create instance of this fragment
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static V4Fragment NewInstance(Activities.Businesses.BusinessHomeActivity context)
        {
            instance = new BusinessProfileFragment(context);

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.edit_icon_menu, menu);
            //menu.FindItem(Resource.Id.menuAction).SetTitle(AppText.AddDrink);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.edit:
                    Intent activity = new Intent(this.HomeContext, typeof(Activities.Businesses.RegisterBusinessActivity));
                    activity.PutExtra("EditProfile", true);
                    activity.PutExtra("BusinessInfo", JsonConvert.SerializeObject(BusinessInfo));
                    activity.PutExtra("AddressInfo", JsonConvert.SerializeObject(this.AddressInfo));
                    activity.PutExtra("BusinessTypes", JsonConvert.SerializeObject(this.BusinessTypes));
                    this.StartActivity(activity);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.BusinessProfile, container, false);

            try
            {
                businessName = view.FindViewById<TextView>(Resource.Id.businessName);
                businessAddress = view.FindViewById<TextView>(Resource.Id.businessAddress);
                phoneNumber = view.FindViewById<TextView>(Resource.Id.phoneNumber);
                businessTypes = view.FindViewById<TextView>(Resource.Id.businessType);
                parentLayout = view.FindViewById<CoordinatorLayout>(Resource.Id.businessPageLayout);
                var fabBtn = view.FindViewById<FloatingActionButton>(Resource.Id.postPicture);
                profilePicture = view.FindViewById<ImageView>(Resource.Id.userAvatar);

                fabBtn.Click += delegate
                {
                    Intent intent = new Intent(this.HomeContext, typeof(Activities.Businesses.BusinesPhotoActivity));
                //intent.PutExtra("IsBusiness", true);
                this.HomeContext.StartActivity(intent);
                };
                //GetProfileInfo();

                profilePicture.Click += delegate
                {
                    SelectPic();
                };
            }
            catch (Exception)
            {
            }

            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public async override void OnActivityCreated(Bundle savedInstanceState)
        {
            try
            {
                base.OnActivityCreated(savedInstanceState);

                await GetProfileInfo();
                //await GetProfilePicture();
            }
            catch (Exception ex) {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetProfilePicture()
        {
            try
            {
                ImageViewImage itemLogo = new ImageViewImage();
                itemLogo.Id = this.BusinessInfo.BusinessId;
                itemLogo.ImageUrl = await this.HomeContext.GetBusinessLogoUri(this.BusinessInfo.BusinessId);

                if (itemLogo.ImageBitmap == null)
                {
                    this.HomeContext.BeginDownloadingImage(itemLogo, profilePicture);
                }

                profilePicture.SetImageBitmap(itemLogo.ImageBitmap);
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
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(this.parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }

                this.HomeContext.ShowProgressbar(true, "", "");

                if (BusinessInfo != null)
                {
                    await BlobStorageHelper.SaveBusinessLogoBlob(picFile.Path, BusinessInfo.BusinessId);
                }

                this.HomeContext.ShowProgressbar(false, "", "");
            }
            catch (Exception)
            {
            }
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

                var ImageUri = Android.Net.Uri.Parse(file.Path);
                profilePicture.SetImageURI(ImageUri);
                await AddCheckInImage(file);
            }
            catch (Exception)
            {
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
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(this.parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    this.HomeContext.ShowProgressbar(true, "", ToastMessage.Loading);
                    BusinessInfo = await App.BusinessFactory.GetByUserId(this.HomeContext.CurrentUser.UserId);
                    if (BusinessInfo != null)
                    {
                        businessName.Text = string.IsNullOrEmpty(BusinessInfo.BusinessName) ? "" : BusinessInfo.BusinessName;
                        phoneNumber.Text = BusinessInfo.PhoneNumber.ToString();

                        this.AddressInfo = await App.AddressFactory.GetAddressByBusinessId(BusinessInfo.BusinessId);

                        if(this.AddressInfo != null)
                        {
                            var addy = string.IsNullOrEmpty(this.AddressInfo.StreetAddress) ? "" : this.AddressInfo.StreetAddress + ", ";
                            var city = string.IsNullOrEmpty(this.AddressInfo.City) ? "" : this.AddressInfo.City + ", ";
                            var state = string.IsNullOrEmpty(this.AddressInfo.State) ? "" : this.AddressInfo.State + " ";
                            var zipcode = string.IsNullOrEmpty(this.AddressInfo.ZipCode) ? "" : this.AddressInfo.ZipCode;
                            var address = addy + city + state + zipcode;
                            businessAddress.Text = address;
                        }

                        this.BusinessTypes = await App.BusinessTypesFactory.GetBusinessType(BusinessInfo.BusinessId);

                        if(this.BusinessTypes != null)
                        {
                            var bar = this.BusinessTypes.Bar ? AppText.Bar + ", " : "";
                            var club = this.BusinessTypes.Club ? AppText.Club + ", " : "";
                            var lounge = this.BusinessTypes.Lounge ? AppText.Lounge + ", " : "";
                            var restaurant = this.BusinessTypes.Restaurant ? AppText.Restaurant + ", " : "";
                            var other = this.BusinessTypes.Other ? AppText.Other : "";
                            businessTypes.Text = bar + club + lounge + restaurant + other;
                        }
                        await GetProfilePicture();
                    }
                    else
                    {
                        businessAddress.Text = string.Empty;
                        businessName.Text = string.Empty;
                        businessTypes.Text = string.Empty;
                        phoneNumber.Text = string.Empty;
                    }
                    this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Searching);
            }
        }

        /// <summary>
        /// Called when the fragment is visible to the user and actively running.
        /// </summary>
        public async override void OnResume()
        {
            try
            {
                base.OnResume();
                if (this.HomeContext.SupportActionBar.Title != "Profile")
                {
                    this.HomeContext.SupportActionBar.Title = "Profile";
                }
                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    await GetProfileInfo();
                }
            }
            catch (Exception) { }
        }



        #endregion

    }
}