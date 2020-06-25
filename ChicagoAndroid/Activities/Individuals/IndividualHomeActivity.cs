using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Individuals
{
    [Activity(Label = "Live", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class IndividualHomeActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        private BottomNavigationView bottomNavigation;      
        private Plugin.Geolocator.Abstractions.Address currentAddy;
        public bool locationAccessRequested;
        private static IndividualHomeActivity instance = new IndividualHomeActivity();

        #endregion

        #region Methods

        /// <summary>
        /// Return the current activity instance.
        /// </summary>
        public static IndividualHomeActivity IndividualMainActivity
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.IndividualHome);
                // Set the current instance of 
                instance = this;
                bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
                bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;
                Shared.MyEnvironment.Environment = this.MyPreferences.GetEnvironment();

                this.ShowProgressbar(true, "", ToastMessage.Loading);

                DetermineIfAccountIsLocked();

                var stripeKey = await App.UsersFactory.GetStripeKey();
                App.CustomerPaymentInfoFactory.Initialize(stripeKey);
                var storageKey = await App.UsersFactory.GetStorageConnectionKey();
                Shared.Helpers.BlobStorageHelper.ConnectionString = storageKey;

                LoadFragment(Resource.Id.menu_home);

                if ((int)Build.VERSION.SdkInt >= (int)BuildVersionCodes.M)
                {
                    this.CheckPermission();
                }
                if (permissionsToRequest.Any())
                {
                    locationAccessRequested = true;
                    this.waitingForPermission = Helpers.PlatformChecks.RequestPermissions(this, permissionsToRequest.ToArray(), 101);
                }
                else
                {
                    await GetLastKnownLocation();
                }

                await AddToMyNotificationHub();

                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void DetermineIfAccountIsLocked()
        {
            var user = await App.UsersFactory.GetUser(this.CurrentUser.UserId);
            if (user != null && user.AccountLocked)
            {
                Toast.MakeText(this, ToastMessage.AccountLockedMessage, ToastLength.Long).Show();
                this.Logout();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        /// <summary>
        /// Called when the activity title is changed
        /// </summary>
        /// <Param name="title"></Param>
        /// <Param name="color"></Param>
        protected override void OnTitleChanged(Java.Lang.ICharSequence title, Android.Graphics.Color color)
        {
            this.SupportActionBar.Title = title.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                this.FinishAffinity();
            }

            return base.OnKeyDown(keyCode, e);
        }

        ///// <summary>
        ///// Get barbers
        ///// </summary>
        ///// <returns></returns>
        //public async Task GetData()
        //{
        //    try
        //    {
        //        if (currentAddy == null)
        //        {
        //            currentAddy = await new LocationHelper().GetAddress();
        //        }
        //        if (currentAddy != null)
        //        {
        //            App.city = string.IsNullOrEmpty(currentAddy.Locality) ? "" : currentAddy.Locality;
        //            App.zipCode = string.IsNullOrEmpty(currentAddy.PostalCode) ? "" : currentAddy.PostalCode;
        //            App.state = string.IsNullOrEmpty(currentAddy.AdminArea) ? "" : currentAddy.AdminArea;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private async Task GeLocation()
        {
            currentAddy = await new LocationHelper().GetAddress();

            if (currentAddy != null)
            {
                App.city = string.IsNullOrEmpty(currentAddy.Locality) ? "" : currentAddy.Locality;
                App.zipCode = string.IsNullOrEmpty(currentAddy.PostalCode) ? "" : currentAddy.PostalCode;
                App.state = string.IsNullOrEmpty(currentAddy.AdminArea) ? "" : currentAddy.AdminArea;
            }
        }

        public async Task GetLastKnownLocation()
        {
            currentAddy = await new LocationHelper().GetLastKnownAddress();
            if (currentAddy == null)
            {
                currentAddy = await new LocationHelper().GetAddress();
            }

            if (currentAddy != null)
            {
                App.city = string.IsNullOrEmpty(currentAddy.Locality) ? "" : currentAddy.Locality;
                App.zipCode = string.IsNullOrEmpty(currentAddy.PostalCode) ? "" : currentAddy.PostalCode;
                App.state = string.IsNullOrEmpty(currentAddy.AdminArea) ? "" : currentAddy.AdminArea;
            }
        }


        /// <summary>
        /// Load fragment
        /// </summary>
        /// <param name="position"></param>
        private void LoadFragment(int position)
        {
            try
            {
                string ARG_MY_NUMBER = "menu_number";
                V4Fragment fragment = new V4Fragment();
                if (this.CurrentFocus != null)
                {
                    var imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                }
                switch (position)
                {
                    case Resource.Id.menu_home:
                        fragment = IndividualHomeFragment.NewInstance(this);
                        break;
                    case Resource.Id.menu_search:
                        fragment = ToastersSearchFragment.NewInstance(this);
                        break;
                    case Resource.Id.menu_profile:
                        fragment = ToasterProfileFragment.NewInstance(this);
                        break;
                    case Resource.Id.menu_more:
                        fragment = ToastersMoreFragment.NewInstance(this);
                        break;
                    case Resource.Id.menu_events:
                        fragment = Fragments.Individuals.Events.ToastersEventsFragment.NewInstance(this);
                        break;
                }

                Bundle args = new Bundle();
                args.PutInt(ARG_MY_NUMBER, position);
                fragment.Arguments = args;

                this.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragmentContainer, fragment).AddToBackStack(null).Commit();
                this.SupportActionBar.Title = GetPageTitle(position);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private string GetPageTitle(int position)
        {
            switch (position)
            {
                case 0:
                    return "Live";
                case 1:
                    return ""; //"Search";
                case 2:
                    return "Profile";
                case 3:
                    return "More";
            }

            return "";
        }
 
        #endregion

    }
}