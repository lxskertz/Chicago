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
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoAndroid.Adapters.Business;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Businesses
{
    [Activity(Label = "business Profile", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class BusinessProfileActivity : BaseActivity
    {
        #region Constants, Enums, and Variables

        // Create a new instance field for this activity.
        private TextView businessName;
        private TextView businessAddress;
        private TextView phoneNumber;
        private TextView businessTypes;
        private CoordinatorLayout parentLayout;
        private ImageView profilePicture;

        private BusinessSearch BusinessSearchInfo;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static Android.Graphics.Bitmap ImageBitmap { get; set; }

        #endregion

        #region Methods

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.BusinessProfile);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                businessName = FindViewById<TextView>(Resource.Id.businessName);
                businessAddress = FindViewById<TextView>(Resource.Id.businessAddress);
                phoneNumber = FindViewById<TextView>(Resource.Id.phoneNumber);
                businessTypes = FindViewById<TextView>(Resource.Id.businessType);
                parentLayout = FindViewById<CoordinatorLayout>(Resource.Id.businessPageLayout);
                var fabBtn = FindViewById<FloatingActionButton>(Resource.Id.postPicture);
                profilePicture = FindViewById<ImageView>(Resource.Id.userAvatar);

                //fabBtn.Visibility = ViewStates.Invisible;
                //GetProfileInfo();

                this.BusinessSearchInfo = JsonConvert.DeserializeObject<BusinessSearch>(Intent.GetStringExtra("BusinessInfo"));

                fabBtn.Click += delegate
                {
                    var businessId = BusinessSearchInfo != null ? BusinessSearchInfo.BusinessId : 0;
                    Intent intent = new Intent(this, typeof(BusinesPhotoActivity));
                    intent.PutExtra("IsBusiness", false);
                    intent.PutExtra("BusinessId", businessId);
                    this.StartActivity(intent);
                };

                await GetProfileInfo();
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
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetProfileInfo()
        {
            try
            {
                if (BusinessSearchInfo != null)
                {
                    businessName.Text = string.IsNullOrEmpty(BusinessSearchInfo.BusinessName) ? "" : BusinessSearchInfo.BusinessName;
                    phoneNumber.Text = BusinessSearchInfo.PhoneNumber.ToString();

                    var addressInfo = await App.AddressFactory.GetAddressByBusinessId(BusinessSearchInfo.BusinessId);

                    if (addressInfo != null)
                    {
                        var addy = string.IsNullOrEmpty(addressInfo.StreetAddress) ? "" : addressInfo.StreetAddress + ", ";
                        var city = string.IsNullOrEmpty(addressInfo.City) ? "" : addressInfo.City + ", ";
                        var state = string.IsNullOrEmpty(addressInfo.State) ? "" : addressInfo.State + " ";
                        var zipcode = string.IsNullOrEmpty(addressInfo.ZipCode) ? "" : addressInfo.ZipCode;
                        var address = addy + city + state + zipcode;
                        businessAddress.Text = address;
                    }

                    var bar = BusinessSearchInfo.Bar ? AppText.Bar + ", " : "";
                    var club = BusinessSearchInfo.Club ? AppText.Club + ", " : "";
                    var lounge = BusinessSearchInfo.Lounge ? AppText.Lounge + ", " : "";
                    var restaurant = BusinessSearchInfo.Restaurant ? AppText.Restaurant + ", " : "";
                    var other = BusinessSearchInfo.Other ? AppText.Other : "";
                    businessTypes.Text = bar + club + lounge + restaurant + other;

                    profilePicture.SetImageBitmap(ImageBitmap);

                    if (ImageBitmap != null)
                    {
                        //ImageBitmap.Recycle();
                        //ImageBitmap = null;
                    }
                }

                else
                {
                    if (this.CheckNetworkConnectivity() == null)
                    {
                        this.ShowSnack(this.parentLayout, ToastMessage.NoInternet, "OK");
                        return;
                    }
                    else
                    {
                        this.ShowProgressbar(true, "", ToastMessage.Loading);
                        var businessInfo = await App.BusinessFactory.GetByUserId(this.CurrentUser.UserId);
                        if (businessInfo != null)
                        {
                            businessName.Text = string.IsNullOrEmpty(businessInfo.BusinessName) ? "" : businessInfo.BusinessName;
                            phoneNumber.Text = businessInfo.PhoneNumber.ToString();

                            var addressInfo = await App.AddressFactory.GetAddressByBusinessId(businessInfo.BusinessId);

                            if (addressInfo != null)
                            {
                                var addy = string.IsNullOrEmpty(addressInfo.StreetAddress) ? "" : addressInfo.StreetAddress + ", ";
                                var city = string.IsNullOrEmpty(addressInfo.City) ? "" : addressInfo.City + ", ";
                                var state = string.IsNullOrEmpty(addressInfo.State) ? "" : addressInfo.State + " ";
                                var zipcode = string.IsNullOrEmpty(addressInfo.ZipCode) ? "" : addressInfo.ZipCode;
                                var address = addy + city + state + zipcode;
                                businessAddress.Text = address;
                            }

                            var bizTypes = await App.BusinessTypesFactory.GetBusinessType(businessInfo.BusinessId);

                            if (bizTypes != null)
                            {
                                var bar = bizTypes.Bar ? AppText.Bar + ", " : "";
                                var club = bizTypes.Club ? AppText.Club + ", " : "";
                                var lounge = bizTypes.Lounge ? AppText.Lounge + ", " : "";
                                var restaurant = bizTypes.Restaurant ? AppText.Restaurant + ", " : "";
                                var other = bizTypes.Other ? AppText.Other : "";
                                businessTypes.Text = bar + club + lounge + restaurant + other;
                            }
                        }
                        else
                        {
                            businessAddress.Text = string.Empty;
                            businessName.Text = string.Empty;
                            businessTypes.Text = string.Empty;
                            phoneNumber.Text = string.Empty;
                        }
                        this.ShowProgressbar(false, "", ToastMessage.Loading);
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.Searching);
            }
        }

        #endregion

    }
}