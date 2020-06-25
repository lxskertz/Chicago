using Foundation;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Models.Orders;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoiOS.DataSource.Orders;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class OrdersController : BaseViewController
    {

        #region Constants, Enums, and Variables

        private string filterText = AppText.Filter;

        #endregion

        #region Properties

        public ToasterOrder.ToasterOrderEnum ToasterOrderEnum { get; set; }

        public static bool RequiresRefresh { get; set; }
        
        private OrdersDataSource OrdersDataSource { get; set; }

        private UIRefreshControl RefreshControl;

        public static DateTime? FilterDate { get; set; }

        public static bool ApplyDateFilter { get; set; }

        private ICollection<ToasterOrder> toasterOrder = new List<ToasterOrder>();

        #endregion

        #region Constructors

        public OrdersController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                if (this.TabBarController != null)
                {
                    this.ToasterOrderEnum = ToasterOrder.ToasterOrderEnum.Business;
                }

                RefreshControl = new UIRefreshControl();
                RefreshControl.ValueChanged += HandleValueChanged;
                OrdersTable.AddSubview(RefreshControl);

                await GetOrders();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);

                if (this.ToasterOrderEnum == ToasterOrder.ToasterOrderEnum.Business)
                {
                    this.TabBarController.NavigationItem.SearchController = null;
                    this.TabBarController.NavigationController.NavigationBarHidden = false;
                    //this.TabBarController.NavigationItem.RightBarButtonItem = null;

                    this.TabBarController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(AppText.Filter, UIBarButtonItemStyle.Plain, (sender, args) =>
                    {
                        OpenDate();
                    }), true);

                }
                else
                {
                    this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(filterText, UIBarButtonItemStyle.Plain, (sender, args) =>
                    {
                        OpenDate();
                    }), true);
                }
                SetTitle();
            }
            catch (Exception)
            {
            }
        }

        private void OpenDate()
        {

            if (this.NavigationItem.RightBarButtonItem != null && this.NavigationItem.RightBarButtonItem.Title == AppText.RemoveFilter &&
                this.ToasterOrderEnum != ToasterOrder.ToasterOrderEnum.Business)
            {
                RemoveFilter();
            }
            else
            {
                var pickerController = this.Storyboard.InstantiateViewController("MyPickerController") as MyPickerController;
                pickerController.ControllerCaller = MyPickerController.Caller.OrdersController;
                pickerController.PickerComponentType = MyPickerController.ComponentType.Date;
                pickerController.ControllerPickerViewType = MyPickerController.PickerViewType.DatePicker;
                this.NavigationController.PushViewController(pickerController, false);
            }     
        }

        /// <summary>
        /// Set title
        /// </summary>
        private void SetTitle()
        {
            switch (this.ToasterOrderEnum)
            {
                case ToasterOrder.ToasterOrderEnum.Sender:
                    this.NavigationItem.Title = AppText.SentDrinks;
                    break;
                case ToasterOrder.ToasterOrderEnum.Receiver:
                    this.NavigationItem.Title = AppText.ReceivedDrinks;
                    break;
                case ToasterOrder.ToasterOrderEnum.Business:
                    this.TabBarController.NavigationItem.Title = AppText.Orders;
                    break;
            }
        }

        private void LoadOrderTable(ICollection<ToasterOrder> toasterOrder)
        {
            if (OrdersDataSource == null)
            {
                OrdersTable.EstimatedRowHeight = 101f;
                OrdersTable.RowHeight = UITableView.AutomaticDimension;
                OrdersDataSource = new OrdersDataSource(this, toasterOrder.ToList(), ToasterOrderEnum);
                OrdersTable.Source = OrdersDataSource;
                OrdersTable.TableFooterView = new UIView();
            }
            else
            {
                this.OrdersDataSource.ToasterOrders = toasterOrder.ToList();
                OrdersTable.ReloadData();
            }
        }

        private void RemoveFilter()
        {
            LoadOrderTable(toasterOrder);
            if (this.ToasterOrderEnum == ToasterOrder.ToasterOrderEnum.Business)
            {
                this.TabBarController.NavigationItem.LeftBarButtonItem = null;
            }
            else
            {
                filterText = AppText.Filter;
                this.NavigationItem.RightBarButtonItem.Title = filterText;
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
                        toasterOrder = await AppDelegate.ToasterOrderFactory.GetUserOrders(AppDelegate.CurrentUser.UserId);
                        break;
                    case ToasterOrder.ToasterOrderEnum.Receiver:
                        toasterOrder = await AppDelegate.ToasterOrderFactory.GetReceivedDrinks(AppDelegate.CurrentUser.UserId);
                        break;
                    case ToasterOrder.ToasterOrderEnum.Business:
                        var businessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);
                        toasterOrder = await AppDelegate.ToasterOrderFactory.GetBusinessOrders(businessInfo.BusinessId);
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

        private void FilterData()
        {
            try
            {
                if (toasterOrder != null && toasterOrder.Count > 0)
                {
                    var filteredOrders = toasterOrder.Where(x => x.OrderDate == FilterDate.Value.Date).ToList();

                    if (filteredOrders != null && filteredOrders.Count > 0)
                    {
                        LoadOrderTable(filteredOrders);

                        if (this.ToasterOrderEnum == ToasterOrder.ToasterOrderEnum.Business)
                        {
                            this.TabBarController.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(AppText.RemoveFilter, UIBarButtonItemStyle.Plain, (sender, args) =>
                            {
                                RemoveFilter();
                            }), true);

                        }
                        else
                        {
                            filterText = AppText.RemoveFilter;
                            this.NavigationItem.RightBarButtonItem.Title = filterText;
                        }
                    }
                    else
                    {
                        BTProgressHUD.ShowSuccessWithStatus(ToastMessage.NoOrderFilterResult, Helpers.ToastTime.SuccessTime);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    if (AppDelegate.IsOfflineMode())
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                        return;
                    }
                    GetRefreshedData();
                }

                if (ApplyDateFilter)
                {
                    ApplyDateFilter = false;

                    if (FilterDate != null && FilterDate.HasValue)
                    {
                        FilterData();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async void CancelOrder(ToasterOrder item)
        {
            try
            {
                var order = await AppDelegate.ToasterOrderFactory.GetByOrderId(item.ToasterOrderId);
                if (order != null && !order.Charged)
                {
                    BTProgressHUD.Show(ToastMessage.Cancelling, -1f, ProgressHUD.MaskType.Black);
                    await AppDelegate.ToasterOrderFactory.Cancel(item.ToasterOrderId);
                    BTProgressHUD.ShowSuccessWithStatus(ToastMessage.DrinkCancelled, Helpers.ToastTime.SuccessTime);

                    GetRefreshedData();
                }
            }
            catch (Exception)
            {
            }
        }

        public void CancelDrink(ToasterOrder item)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }

                UIAlertController uIAlertController = new UIAlertController();
                uIAlertController = UIAlertController.Create("", ToastMessage.CancelOrderWarning, UIAlertControllerStyle.Alert);
                uIAlertController.AddAction(UIAlertAction.Create(AppText.No, UIAlertActionStyle.Cancel, null));
                uIAlertController.AddAction(UIAlertAction.Create(AppText.Yes, UIAlertActionStyle.Default, alertAction => CancelOrder(item)));
                this.PresentViewController(uIAlertController, true, null);
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        public async void MarkPickedUp(ToasterOrder item)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }

                BTProgressHUD.Show(ToastMessage.Charging, -1f, ProgressHUD.MaskType.Black);

                var order = await AppDelegate.ToasterOrderFactory.GetByOrderId(item.ToasterOrderId);
                if (order != null && !order.Cancelled && !item.PickedUp)
                {
                    await AppDelegate.ToasterOrderFactory.Charged(item.ToasterOrderId);
                    GetRefreshedData();
                }
                BTProgressHUD.Dismiss();
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.SuccessTime);
            }

        }

        public async void ChargeDrink(ToasterOrder item)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }

                BTProgressHUD.Show(ToastMessage.Charging, -1f, ProgressHUD.MaskType.Black);

                var order = await AppDelegate.ToasterOrderFactory.GetByOrderId(item.ToasterOrderId);
                if (order != null && !order.Cancelled && !item.Charged)
                {

                    var stripeCustomerInfo = await AppDelegate.StripeCustomerInfoFactory.Get(item.SenderUserId);

                    if (stripeCustomerInfo != null)
                    {
                        item.CardChargeAmount = Math.Round(item.TotalOrderAmount - item.StripeFee, 2);
                        var charge = await AppDelegate.CustomerPaymentInfoFactory.ChargeCustomer(stripeCustomerInfo.StripeCustomerId, item.CardChargeAmount, "");

                        if (charge != null && charge.Paid)
                        {
                            await AppDelegate.ToasterOrderFactory.Charged(item.ToasterOrderId);

                            if (item.UsedPointType != ToasterOrder.PointType.None)
                            {
                                await AppDelegate.ToasterPointsFactory.MarkAsRedeemed(item.ToasterOrderId);
                            }

                            BTProgressHUD.ShowSuccessWithStatus(ToastMessage.ChargedSuccess, Helpers.ToastTime.SuccessTime);
                            GetRefreshedData();
                            await new Shared.Helpers.PushNotificationHelper(AppDelegate.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.iOS).DrinkPickedUpPush(item.SenderUserId);
                        }
                        else
                        {
                            if(charge != null)
                            {
                                if (!string.IsNullOrEmpty(charge.FailureMessage))
                                {
                                    BTProgressHUD.ShowErrorWithStatus(charge.FailureMessage, Helpers.ToastTime.SuccessTime);
                                } else
                                {
                                    if (charge.Outcome != null)
                                    {
                                        BTProgressHUD.ShowErrorWithStatus(charge.Outcome.SellerMessage, Helpers.ToastTime.SuccessTime);
                                    }
                                }
                            }                     
                        }

                    } else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.SuccessTime);
                    }

                } else
                {
                    BTProgressHUD.Dismiss();
                }
            }
            catch (Stripe.StripeException ex)
            {
                var a = ex;
                BTProgressHUD.ShowErrorWithStatus(ex.Message, Helpers.ToastTime.SuccessTime);
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.SuccessTime);
            }

        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetOrders()
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

                    switch (this.ToasterOrderEnum)
                    {
                        case ToasterOrder.ToasterOrderEnum.Sender:
                            toasterOrder = await AppDelegate.ToasterOrderFactory.GetUserOrders(AppDelegate.CurrentUser.UserId);
                            break;
                        case ToasterOrder.ToasterOrderEnum.Receiver:
                            toasterOrder = await AppDelegate.ToasterOrderFactory.GetReceivedDrinks(AppDelegate.CurrentUser.UserId);
                            break;
                        case ToasterOrder.ToasterOrderEnum.Business:
                            var businessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);
                            toasterOrder = await AppDelegate.ToasterOrderFactory.GetBusinessOrders(businessInfo.BusinessId);
                            break;
                    }

                    if (toasterOrder != null)
                    {
                        LoadOrderTable(toasterOrder);
                    }
                    else
                    {
                        //BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    //try
                    //{
                    RefreshControl.BeginRefreshing();

                    //BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    GetRefreshedData();

                    RefreshControl.EndRefreshing();

                    //BTProgressHUD.Dismiss();

                }
            }
            catch (Exception)
            {
            }

        }


        #endregion

    }
}