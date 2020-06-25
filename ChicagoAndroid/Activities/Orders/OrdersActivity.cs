using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoAndroid.Adapters.Orders;
using Tabs.Mobile.Shared.Models.Orders;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Orders
{
    [Activity(Label = "Drinks", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class OrdersActivity : BaseActivity
    {

        #region Constants, Enums, Variables

        public SwipeRefreshLayout refresher;
        private ListView ordersList;
        private IMenuItem filterMenu;
        private ICollection<ToasterOrder> toasterOrder = new List<ToasterOrder>();
        private Android.Support.V7.App.AlertDialog orderAlert;
        Android.Support.V7.App.AlertDialog.Builder orderBuilder;
        private ToasterOrder itemToCancel;

        #endregion

        #region Properties

        public ToasterOrder.ToasterOrderEnum ToasterOrderEnum { get; set; }

        public static bool RequiresRefresh { get; set; }

        private OrdersAdapter OrdersAdapter { get; set; }

        /// Gets or sets the search view
        /// </summary>
        public Android.Support.V7.Widget.SearchView SearchView { get; set; }

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
                SetContentView(Resource.Layout.Orders);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                ordersList = FindViewById<ListView>(Resource.Id.ordersList);

                this.ToasterOrderEnum = (ToasterOrder.ToasterOrderEnum)Intent.GetIntExtra("ToasterOrderEnum", (int)ToasterOrder.ToasterOrderEnum.Receiver);

                refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefresh);
                refresher.Refresh += HandleRefresh;

                switch (this.ToasterOrderEnum)
                {
                    case ToasterOrder.ToasterOrderEnum.Sender:
                        this.Title = AppText.SentDrinks;
                        break;
                    case ToasterOrder.ToasterOrderEnum.Receiver:
                        this.Title = AppText.ReceivedDrinks;
                        break;
                }
                orderBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);

                await LoadData();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu_with_text, menu);
            filterMenu = menu.FindItem(Resource.Id.menuAction);
            filterMenu.SetTitle(AppText.Filter);

            return base.OnCreateOptionsMenu(menu);
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
                    OpenDate();
                    return true;
                case Android.Resource.Id.Home:
                    this.Finish();
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
                new DatePickerDialog(this, FilterData, DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day).Show();
            }
        }

        private void FilterData(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            try
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
                        Toast.MakeText(this, ToastMessage.NoOrderFilterResult, ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void RemoveFilter()
        {
            LoadOrderTable(toasterOrder);
            filterMenu.SetTitle(AppText.Filter);
        }

        private void LoadOrderTable(ICollection<ToasterOrder> toasterOrder)
        {
            try
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
                    this.RunOnUiThread(() =>
                    {
                        this.OrdersAdapter.NotifyDataSetChanged();
                    });
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Called when the fragment is visible to the user and actively running.
        /// </summary>
        protected override void OnResume()
        {
            try
            {
                base.OnResume();

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    if (this.CheckNetworkConnectivity() == null)
                    {
                        Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
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
                    this.ShowProgressbar(true, "", ToastMessage.Loading);
                    switch (this.ToasterOrderEnum)
                    {
                        case ToasterOrder.ToasterOrderEnum.Sender:
                            toasterOrder = await App.ToasterOrderFactory.GetUserOrders(this.CurrentUser.UserId);
                            break;
                        case ToasterOrder.ToasterOrderEnum.Receiver:
                            toasterOrder = await App.ToasterOrderFactory.GetReceivedDrinks(this.CurrentUser.UserId);
                            break;
                    }

                    if (toasterOrder != null)
                    {
                        LoadOrderTable(toasterOrder);
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
        private async void GetRefreshedData()
        {
            try
            {
                switch (this.ToasterOrderEnum)
                {
                    case ToasterOrder.ToasterOrderEnum.Sender:
                        toasterOrder = await App.ToasterOrderFactory.GetUserOrders(this.CurrentUser.UserId);
                        break;
                    case ToasterOrder.ToasterOrderEnum.Receiver:
                        toasterOrder = await App.ToasterOrderFactory.GetReceivedDrinks(this.CurrentUser.UserId);
                        break;
                }

                if (toasterOrder != null)
                {
                    LoadOrderTable(toasterOrder);
                }
            }
            catch (Exception)
            {
            }
        }

        private void CancelClicked(object sender, DialogClickEventArgs arg) 
        {
            if(orderAlert != null)
            {
                orderAlert.Dismiss();
                orderAlert.Dispose();
                orderAlert = null;
            }
        }

        private async void CancelOrder(object sender, DialogClickEventArgs arg)
        {
            try
            {
                if (orderAlert != null)
                {
                    orderAlert.Dismiss();
                    orderAlert.Dispose();
                    orderAlert = null;
                    //orderAlert.IsShowing
                }

                if (itemToCancel != null)
                {
                    var order = await App.ToasterOrderFactory.GetByOrderId(itemToCancel.ToasterOrderId);
                    if (order != null && !order.Charged)
                    {
                        this.ShowProgressbar(true, "", ToastMessage.Cancelling);
                        await App.ToasterOrderFactory.Cancel(itemToCancel.ToasterOrderId);
                        Toast.MakeText(this, ToastMessage.DrinkCancelled, ToastLength.Short).Show();

                        GetRefreshedData();
                        this.ShowProgressbar(false, "", ToastMessage.Cancelling);

                    }
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        public void CancelDrink(ToasterOrder item)
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                    return;
                }

                if (orderAlert != null && orderAlert.IsShowing) {
                    orderAlert.Dismiss();
                    orderAlert.Dispose();
                } 
                itemToCancel = item;
                orderBuilder.SetMessage(ToastMessage.CancelOrderWarning);
                orderBuilder.SetCancelable(false);
                orderBuilder.SetNegativeButton(AppText.No, CancelClicked);
                orderBuilder.SetPositiveButton(AppText.Yes, CancelOrder);
                orderAlert = orderBuilder.Create();
                orderAlert.Show();
                    
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Cancelling);
            }
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
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
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