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
using Newtonsoft.Json;
using Tabs.Mobile.ChicagoAndroid.Adapters.Drinks;
using Tabs.Mobile.ChicagoAndroid.Fragments.Business;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Drinks
{
    public class BusinessDrinksFragment : BaseBusinessFragment
    {

        #region Constants, Enums, Variables

        private static BusinessDrinksFragment instance;
        private ListView drinksList;
        private BusinessDrinksAdapter BusinessDrinksAdapter;

        #endregion

        #region Properties

        public static bool RequiresRefresh = true;

        public Shared.Models.Businesses.Business BusinessInfo { get; set; }

        #endregion

        #region Constructors

        public BusinessDrinksFragment(Activities.Businesses.BusinessHomeActivity context)
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
            instance = new BusinessDrinksFragment(context);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.BusinessDrinks, container, false);
            try
            {
                drinksList = view.FindViewById<ListView>(Resource.Id.businessDrinksList);
                LoadData();
            }
            catch (Exception)
            {
            }

            return view;
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task LoadData()
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this.HomeContext, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    this.HomeContext.ShowProgressbar(true, "", ToastMessage.Loading);

                    this.BusinessInfo = await App.BusinessFactory.GetByUserId(this.HomeContext.CurrentUser.UserId);

                    if (this.BusinessInfo != null)
                    {
                        var drinks = await App.BusinessDrinkFactory.Get(this.BusinessInfo.BusinessId);

                        if (drinks != null)
                        {
                            BusinessDrinksAdapter = new BusinessDrinksAdapter(this, drinks.ToList());
                            drinksList.Adapter = BusinessDrinksAdapter;
                            drinksList.ItemClick += BusinessDrinksAdapter.OnListItemClick;
                            drinksList.DividerHeight = 2;
                        }
                        else
                        {
                            //BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                        }
                    }
                    this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="inflater"></param>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_with_text, menu);
            menu.FindItem(Resource.Id.menuAction).SetTitle(AppText.AddDrink);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuAction:
                    Intent activity = new Intent(this.HomeContext, typeof(Activities.Drinks.AddEditDrinkActivity));
                    activity.PutExtra("ScreenActionType", (int)Activities.Drinks.AddEditDrinkActivity.ActionType.Add);
                    var bizId = this.BusinessInfo != null ? this.BusinessInfo.BusinessId : 0;
                    activity.PutExtra("BusinessId", bizId); 
                    this.StartActivity(activity);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// Called when the fragment is visible to the user and actively running.
        /// </summary>
        public async override void OnResume()
        {
            try
            {
                base.OnResume();
                if (this.HomeContext.SupportActionBar.Title != "Drinks")
                {
                    this.HomeContext.SupportActionBar.Title = "Drinks";
                }

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    if (this.HomeContext.CheckNetworkConnectivity() == null)
                    {
                        Toast.MakeText(this.HomeContext, ToastMessage.NoInternet, ToastLength.Short).Show();
                        return;
                    }

                    if (this.BusinessInfo != null)
                    {
                        var drinks = await App.BusinessDrinkFactory.Get(this.BusinessInfo.BusinessId);

                        if (drinks != null)
                        {
                            if (BusinessDrinksAdapter == null)
                            {
                                BusinessDrinksAdapter = new BusinessDrinksAdapter(this, drinks.ToList());
                                drinksList.Adapter = BusinessDrinksAdapter;
                                drinksList.ItemClick += BusinessDrinksAdapter.OnListItemClick;
                                drinksList.DividerHeight = 2;
                            } else
                            {
                                this.BusinessDrinksAdapter.Drinks = drinks.ToList();
                                this.BusinessDrinksAdapter.NotifyDataSetChanged();
                                this.HomeContext.RunOnUiThread(() =>
                                {
                                    this.BusinessDrinksAdapter.NotifyDataSetChanged();
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception) { }
        }

        #endregion

    }
}