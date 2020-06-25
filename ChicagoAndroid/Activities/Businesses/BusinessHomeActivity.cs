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
using Tabs.Mobile.ChicagoAndroid.Fragments.Business;
using Tabs.Mobile.ChicagoAndroid.Fragments.Business.Events;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Businesses
{
    [Activity(Label = "Overview", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class BusinessHomeActivity : BaseActivity
    {
        #region Properties

        private BottomNavigationView bottomNavigation;

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
                SetContentView(Resource.Layout.BusinessHome);
                bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

                bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;
                Shared.MyEnvironment.Environment = this.MyPreferences.GetEnvironment();

                this.ShowProgressbar(true, "", ToastMessage.Loading);

                var stripeKey = await App.UsersFactory.GetStripeKey();
                App.CustomerPaymentInfoFactory.Initialize(stripeKey);
                var storageKey = await App.UsersFactory.GetStorageConnectionKey();
                Shared.Helpers.BlobStorageHelper.ConnectionString = storageKey;

                LoadFragment(Resource.Id.menu_home);

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
                        fragment = BusinessProfileFragment.NewInstance(this);
                        break;
                    case Resource.Id.menu_events:
                        fragment = BusinessEventsFragment.NewInstance(this);
                        break;
                    case Resource.Id.menu_drinks:
                        fragment = Fragments.Drinks.BusinessDrinksFragment.NewInstance(this);
                        break;
                    case Resource.Id.menu_orders:
                        fragment = Fragments.Orders.OrdersFragment.NewInstance(this);
                        break;
                    case Resource.Id.menu_more:
                        fragment = BusinessMoreFragment.NewInstance(this);
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
                    return "Overview";
                case 1:
                    return "Events";
                case 2:
                    return "Drinks";
                case 3:
                    return "Orders";
                case 4:
                    return "More";
            }

            return "";
        }

        #endregion

    }
}