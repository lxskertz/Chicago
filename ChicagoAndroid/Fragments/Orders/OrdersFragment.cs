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
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Newtonsoft.Json;
using Tabs.Mobile.ChicagoAndroid.Adapters.Orders;
using Tabs.Mobile.ChicagoAndroid.Fragments.Business;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Orders;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Orders
{
    public class OrdersFragment : BaseBusinessFragment
    {

        #region Constants, Enums, Variables

        private static OrdersFragment instance;
        public SwipeRefreshLayout refresher;
        private ListView ordersList;
        private IMenuItem filterMenu;
        private ICollection<ToasterOrder> toasterOrder = new List<ToasterOrder>();

        #endregion

        #region Properties

        public ToasterOrder.ToasterOrderEnum ToasterOrderEnum { get; set; }

        public static bool RequiresRefresh { get; set; }

        private OrdersAdapter OrdersAdapter { get; set; }

        #endregion

        #region Constructors

        public OrdersFragment(Activities.Businesses.BusinessHomeActivity context)
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
            instance = new OrdersFragment(context);

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
            var view = inflater.Inflate(Resource.Layout.Orders, container, false);
            ordersList = view.FindViewById<ListView>(Resource.Id.ordersList);
            this.ToasterOrderEnum = ToasterOrder.ToasterOrderEnum.Business;

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefresh);
            refresher.Refresh += HandleRefresh;

            LoadData();

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            //try
            //{
            inflater.Inflate(Resource.Menu.menu_with_text, menu);
            filterMenu = menu.FindItem(Resource.Id.menuAction);
            filterMenu.SetTitle(AppText.Filter);

            base.OnCreateOptionsMenu(menu, inflater);
            //}
            //catch (Exception) { }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuAction:
                    OpenDate();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void OpenDate()
        {

            if (filterMenu.TitleFormatted.ToString() == AppText.RemoveFilter)
            {
                RemoveFilter();
            }
            else
            {
                new DatePickerDialog(this.HomeContext, FilterData, DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day).Show();
            }
        }

        private void FilterData(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            if (toasterOrder != null && toasterOrder.Count > 0)
            {
                var filteredOrders = toasterOrder.Where(x => x.OrderDate == e.Date).ToList();

                if (filteredOrders != null && filteredOrders.Count > 0)
                {
                    LoadOrderTable(filteredOrders);

                    filterMenu.SetTitle(AppText.RemoveFilter);
                }
                else
                {
                    Toast.MakeText(this.HomeContext, ToastMessage.NoOrderFilterResult, ToastLength.Short).Show();
                }
            }

        }

        private void RemoveFilter()
        {
            LoadOrderTable(toasterOrder);
            filterMenu.SetTitle(AppText.Filter);
        }

        private void LoadOrderTable(ICollection<ToasterOrder> toasterOrder)
        {
            if (OrdersAdapter == null)
            {

                OrdersAdapter = new OrdersAdapter(this, toasterOrder.ToList(), ToasterOrderEnum);
                ordersList.Adapter = OrdersAdapter;
                ordersList.ItemClick += OrdersAdapter.OnListItemClick;
                ordersList.DividerHeight = 2;
            }
            else
            {
                this.OrdersAdapter.ToasterOrders = toasterOrder.ToList();
                this.OrdersAdapter.NotifyDataSetChanged();
                this.HomeContext.RunOnUiThread(() =>
                {
                    this.OrdersAdapter.NotifyDataSetChanged();
                });
            }
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

                    var businessInfo = await App.BusinessFactory.GetByUserId(this.HomeContext.CurrentUser.UserId);
                    toasterOrder = await App.ToasterOrderFactory.GetBusinessOrders(businessInfo.BusinessId);

                    if (toasterOrder != null)
                    {
                        LoadOrderTable(toasterOrder);
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
        private async void GetRefreshedData()
        {

            var businessInfo = await App.BusinessFactory.GetByUserId(this.HomeContext.CurrentUser.UserId);
            toasterOrder = await App.ToasterOrderFactory.GetBusinessOrders(businessInfo.BusinessId);

            if (toasterOrder != null)
            {
                LoadOrderTable(toasterOrder);
            }
        }

        public async void MarkPickedUp(ToasterOrder item)
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this.HomeContext, ToastMessage.NoInternet, ToastLength.Short).Show();
                    return;
                }

                this.HomeContext.ShowProgressbar(true, "", ToastMessage.Charging);

                var order = await App.ToasterOrderFactory.GetByOrderId(item.ToasterOrderId);
                if (order != null && !order.Cancelled && !item.PickedUp)
                {
                    await App.ToasterOrderFactory.Charged(item.ToasterOrderId);
                    GetRefreshedData();
                }
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Charging);
            }
            catch (Exception ex)
            {
                var a = ex;
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Charging);
                Toast.MakeText(this.HomeContext, ToastMessage.ServerError, ToastLength.Short).Show();
            }

        }

        public async void ChargeDrink(ToasterOrder item)
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this.HomeContext, ToastMessage.NoInternet, ToastLength.Short).Show();
                    return;
                }

                this.HomeContext.ShowProgressbar(true, "", ToastMessage.Charging);

                var order = await App.ToasterOrderFactory.GetByOrderId(item.ToasterOrderId);
                var businessInfo = await App.BusinessFactory.GetByUserId(this.HomeContext.CurrentUser.UserId);
                string businessName = businessInfo != null ? businessInfo?.BusinessName : "";

                if (order != null && !order.Cancelled && !item.Charged)
                {

                    var stripeCustomerInfo = await App.StripeCustomerInfoFactory.Get(item.SenderUserId);

                    if (stripeCustomerInfo != null)
                    {
                        item.CardChargeAmount = Math.Round(item.TotalOrderAmount - item.StripeFee, 2);
                        var charge = await App.CustomerPaymentInfoFactory.ChargeCustomer(stripeCustomerInfo.StripeCustomerId, item.CardChargeAmount, "", businessName);

                        if (charge != null && charge.Paid)
                        {
                            await App.ToasterOrderFactory.Charged(item.ToasterOrderId);

                            if (item.UsedPointType != ToasterOrder.PointType.None)
                            {
                                await App.ToasterPointsFactory.MarkAsRedeemed(item.ToasterOrderId);
                            }

                            Toast.MakeText(this.HomeContext, ToastMessage.ChargedSuccess, ToastLength.Short).Show();
                            GetRefreshedData();
                            await new Shared.Helpers.PushNotificationHelper(App.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.Android).DrinkPickedUpPush(item.SenderUserId);
                        }
                        else
                        {
                            if (charge != null)
                            {
                                if (!string.IsNullOrEmpty(charge.FailureMessage))
                                {
                                    Toast.MakeText(this.HomeContext, charge.FailureMessage, ToastLength.Short).Show();
                                }
                                else
                                {
                                    if (charge.Outcome != null)
                                    {
                                        Toast.MakeText(this.HomeContext, charge.Outcome.SellerMessage, ToastLength.Short).Show();
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        Toast.MakeText(this.HomeContext, ToastMessage.ServerError, ToastLength.Short).Show();
                    }

                    this.HomeContext.ShowProgressbar(false, "", ToastMessage.Charging);

                } else
                {
                    this.HomeContext.ShowProgressbar(false, "", ToastMessage.Charging);
                }
            }
            catch(Stripe.StripeException ex)
            {
                var a = ex;
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Charging);
                Toast.MakeText(this.HomeContext, ex.Message, ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                var a = ex;
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Charging);
                Toast.MakeText(this.HomeContext, ex.Message, ToastLength.Short).Show();
            }

        }

        /// <summary>
        /// Called when the fragment is visible to the user and actively running.
        /// </summary>
        public override void OnResume()
        {
            try
            {
                base.OnResume();
                if (this.HomeContext.SupportActionBar.Title != AppText.Orders)
                {
                    this.HomeContext.SupportActionBar.Title = AppText.Orders;
                }

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    if (this.HomeContext.CheckNetworkConnectivity() == null)
                    {
                        Toast.MakeText(this.HomeContext, ToastMessage.NoInternet, ToastLength.Short).Show();
                        return;
                    }

                    GetRefreshedData();
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleRefresh(object sender, EventArgs e)
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this.HomeContext, ToastMessage.NoInternet, ToastLength.Short).Show();
                 }
                else
                {
                    GetRefreshedData();
                }
            }
            catch (Exception)
            {
                refresher.Refreshing = false;
            }
            refresher.Refreshing = false;
        }

        #endregion

    }
}