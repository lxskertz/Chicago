using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Widget;
using Newtonsoft.Json;
using Tabs.Mobile.ChicagoAndroid.Adapters.Drinks;
using Tabs.Mobile.Shared.Models.Drinks;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.CheckIns;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Drinks
{
    [Activity(Label = "Send Drink", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class SendDrinkActivity : BaseActivity
    {

        #region Constants, Enums, Variables

        private ListView drinksList;
        private View headerView;
        private SendDrinkAdapter SendDrinkAdapter;

        #endregion

        #region Properties

        public static bool DrinkSent { get; set; }

        public static bool RequiresRefresh = true;

        public bool FromBusiness { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CheckIn CheckInItem { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.BusinessDrinks);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                drinksList = FindViewById<ListView>(Resource.Id.businessDrinksList);
                await LoadData();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            if (DrinkSent)
            {
                DrinkSent = false;
                this.Finish();
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
                    this.CheckInItem = JsonConvert.DeserializeObject<CheckIn>(Intent.GetStringExtra("CheckInItem"));
                    this.FromBusiness = Intent.GetBooleanExtra("FromBusiness", false);

                    this.ShowProgressbar(true, "", ToastMessage.Loading);

                    var businessInfo = await App.BusinessFactory.Get(CheckInItem.BusinessId);

                    if (businessInfo != null)
                    {
                        if (headerView != null)
                        {
                            drinksList.RemoveHeaderView(headerView);
                        }
                        headerView = LayoutInflater.FromContext(this).Inflate(Resource.Layout.SendDrinkHeader, null);

                        var businessName = headerView.FindViewById<TextView>(Resource.Id.title);
                        var businessAddress = headerView.FindViewById<TextView>(Resource.Id.subTitle);
                        businessName.Text = string.IsNullOrEmpty(businessInfo.BusinessName) ? "" : businessInfo.BusinessName;

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

                        drinksList.AddHeaderView(headerView);

                        var drinks = await App.BusinessDrinkFactory.Get(businessInfo.BusinessId);

                        if (drinks != null)
                        {
                            SendDrinkAdapter = new SendDrinkAdapter(this, drinks.ToList());
                            drinksList.Adapter = SendDrinkAdapter;
                            drinksList.ItemClick += SendDrinkAdapter.OnListItemClick;
                            drinksList.DividerHeight = 2;
                        }
                        else
                        {
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

        #endregion

    }
}