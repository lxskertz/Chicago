using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Drinks;
using Tabs.Mobile.Shared.Models.Drinks;
using BigTed;
using Stripe;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models.Payment;
using Tabs.Mobile.Shared.Models.Orders;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class DrinkQuantityController : BaseViewController
    {

        #region Constants, Enums, and Variables

        public static double totalAmount = 0;
        public static double drinkAmount = 0;
        public static double tabFee = 0;
        private int totalEarnedPoints = 0;
        private bool usePoint = false;
        public double discountAmount = 0;
        private double stripeFee = 0;
        private double tipAmount = 0;

        #endregion

        #region Properties

        public bool FromBusiness { get; set; }

        public static bool RequiresRefresh { get; set; }

        public static Card defaultPayment;

        public StripeCustomerInfo StripeCustomerInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessDrink Drink { get; set; }


        public CheckIn CheckInItem { get; set; }

        #endregion

        #region Constructors

        public DrinkQuantityController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                SendDrinkHelper.counter = 1;
                SendDrinkHelper.SelectedPointToUse = SendDrinkHelper.PointToUse.None;
                PointsView.Hidden = true;
                SendDrinkHelper.tipCounter = 0;

                if (Drink != null)
                {
                    DrinkName.Text = string.IsNullOrEmpty(Drink.DrinkName) ? "" : Drink.DrinkName;
                    Price.Text = SendDrinkHelper.CurrencySymbol + Drink.Price.ToString() + SendDrinkHelper.TabsServiceFeeText;
                    SetPrices();
                    SetUpdatedPrice(totalAmount);
                }

                TipView.Hidden = true;
                tipHeader.Hidden = true;

                if (FromBusiness)
                {
                    StartTabBtn.SetTitle(AppText.Send, UIControlState.Normal);
                    PaymentOptionText.Hidden = true;
                    PaymentOptionView.Hidden = true;
                    PaymentDisclaimer.Hidden = true;

                    tipHeader.Hidden = true;
                    TipView.Hidden = true;
                }
                else
                {
                    this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(AppText.UsePoints, UIBarButtonItemStyle.Plain, (sender, args) =>
                    {
                        GetEarnedPoints();
                    }), true);
                }

                await GetDefaultCard();
            }
            catch (Exception)
            {
            }
        }

        private void SetUpdatedPrice(double amt)
        {
            UpdatedPriceText.Text = SendDrinkHelper.CurrencySymbol + amt.ToString();
        }

        private void SetPrices()
        {
            drinkAmount = this.Drink.Price;
            tabFee = SendDrinkHelper.tabServiceFee * this.Drink.Price;
            stripeFee = (SendDrinkHelper.stripeServiceFee * tabFee) + SendDrinkHelper.additionalStripeServiceFee;
            totalAmount = SendDrinkHelper.InitialUpdatedPrice(Drink.Price, usePoint);
        }

        private void ApplyRemovePoints()
        {
            if (SendDrinkHelper.counter <= 0)
            {
                SetPrices();
                SetUpdatedPrice(totalAmount);
            }
            else
            {
                var prices = SendDrinkHelper.CalculateUpdatedPrice(Drink.Price, usePoint);

                drinkAmount = prices.Item2;
                tabFee = prices.Item1;
                totalAmount = prices.Item3;
                discountAmount = prices.Item4;
                stripeFee = prices.Item5;
                tipAmount = prices.Item6;
                SetUpdatedPrice(totalAmount);
            }
        }

        private async void GetEarnedPoints()
        {
            try
            {
                if (this.NavigationItem.RightBarButtonItem.Title == AppText.DontUsePoints)
                {
                    usePoint = false;
                    this.NavigationItem.RightBarButtonItem.Title = AppText.UsePoints;
                    PointsView.Hidden = true;
                    ApplyRemovePoints();
                }
                else
                {
                    if (AppDelegate.IsOfflineMode())
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                        return;
                    }
                    else
                    {
                        BTProgressHUD.Show(ToastMessage.PleaseWait, -1f, ProgressHUD.MaskType.Black);
                        totalEarnedPoints = await AppDelegate.ToasterPointsFactory.GetTotalEarnedPoints(AppDelegate.CurrentUser.UserId);

                        if (totalEarnedPoints < SendDrinkHelper.QuarterPercentOff)
                        {
                            PointsView.Hidden = true;
                            BTProgressHUD.ShowErrorWithStatus(ToastMessage.NotEnoughPoints, Helpers.ToastTime.ErrorTime);
                        }
                        else
                        {
                            this.NavigationItem.RightBarButtonItem.Title = AppText.DontUsePoints;
                            usePoint = true;
                            if (totalEarnedPoints >= SendDrinkHelper.FullOff)
                            {
                                SendDrinkHelper.SelectedPointToUse = SendDrinkHelper.PointToUse.Full;
                                BTProgressHUD.Dismiss();
                            }
                            else
                            {
                                SendDrinkHelper.SelectedPointToUse = SendDrinkHelper.PointToUse.Quarter;
                                BTProgressHUD.Dismiss();
                            }
                            PointsView.Hidden = false;
                            PointDiscountLbl.Text = SendDrinkHelper.GetPointToUseText();
                            ApplyRemovePoints();

                        }
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        private async Task GetDefaultCard()
        {
            try
            {
                this.StripeCustomerInfo = await AppDelegate.StripeCustomerInfoFactory.Get(AppDelegate.CurrentUser.UserId);

                if (this.StripeCustomerInfo != null)
                {
                    var customer = await AppDelegate.CustomerPaymentInfoFactory.GetCustomer(StripeCustomerInfo.StripeCustomerId);
                    if (customer != null)
                    {
                        defaultPayment = await AppDelegate.CustomerPaymentInfoFactory.GetDefaultCard(this.StripeCustomerInfo.StripeCustomerId, customer.DefaultSourceId);

                        if (defaultPayment != null)
                        {
                            //HideShowPaymentOption(false);
                            var text = string.IsNullOrEmpty(defaultPayment.Last4) ? "" :
                                AppText.Asterisks + defaultPayment.Last4 + "    " + defaultPayment.ExpMonth + "/" + defaultPayment.ExpYear;
                            SelectPaymentOption.Text = text;
                        }
                    }
                    else
                    {
                        ChangePaymentBtn.SetTitle(AppText.AddPaymentText, UIControlState.Normal);
                    }
                }
                else
                {
                    ChangePaymentBtn.SetTitle(AppText.AddPaymentText, UIControlState.Normal);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void HideShowPaymentOption(bool hide)
        {

            if (defaultPayment != null)
            {
                ChangePaymentBtn.SetTitle(AppText.Change, UIControlState.Normal);
                var text = string.IsNullOrEmpty(defaultPayment.Last4) ? "" : AppText.Asterisks + defaultPayment.Last4 + "    " + defaultPayment.ExpMonth + "/" + defaultPayment.ExpYear;
                SelectPaymentOption.Text = !hide ? text : "";
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        partial void MinusBtn_TouchUpInside(UIButton sender)
        {
            Quantity.Text = SendDrinkHelper.ModifyCounter(false).ToString();
            var prices = SendDrinkHelper.CalculateUpdatedPrice(Drink.Price, usePoint);

            drinkAmount = prices.Item2;
            tabFee = prices.Item1;
            totalAmount = prices.Item3;
            discountAmount = prices.Item4;
            stripeFee = prices.Item5;
            tipAmount = prices.Item6;
            SetUpdatedPrice(totalAmount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        partial void AddBtn_TouchUpInside(UIButton sender)
        {
            Quantity.Text = SendDrinkHelper.ModifyCounter(true).ToString();
            var prices = SendDrinkHelper.CalculateUpdatedPrice(Drink.Price, usePoint);

            drinkAmount = prices.Item2;
            tabFee = prices.Item1;
            totalAmount = prices.Item3;
            discountAmount = prices.Item4;
            stripeFee = prices.Item5;
            tipAmount = prices.Item6;
            SetUpdatedPrice(totalAmount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        partial void StartTabBtn_TouchUpInside(UIButton sender)
        {
            if (FromBusiness)
            {
                CreateBusinessOrder();
            }
            else
            {
                if (defaultPayment == null)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoPaymentMethod, 3000);
                    //var controller = this.Storyboard.InstantiateViewController("PaymentMethodController") as PaymentMethodController;
                    //controller.FromQuantityController = true;
                    //this.NavigationController.PushViewController(controller, true);
                }
                else
                {
                    CreateToasterOrder();
                }
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

                    if (defaultPayment != null)
                    {
                        HideShowPaymentOption(false);
                        //CreateOrder();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async void CreateBusinessOrder()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }

                BTProgressHUD.Show(ToastMessage.SendingDrink, -1f, ProgressHUD.MaskType.Black);

                var receiverInfo = await AppDelegate.UsersFactory.GetUser(this.CheckInItem.UserId);
                var businessInfo = await AppDelegate.BusinessFactory.Get(this.Drink.BusinessId);

                if (receiverInfo == null || businessInfo == null || Drink == null
                    || CheckInItem == null)
                {
                    return;
                }

                ToasterOrder toasterOrder = new ToasterOrder();
                toasterOrder.BusinessDrinkId = this.Drink.BusinessDrinkId;
                toasterOrder.BusinessId = this.Drink.BusinessId;
                toasterOrder.BusinessName = businessInfo.BusinessName;
                toasterOrder.Charged = true;
                toasterOrder.DrinkName = this.Drink.DrinkName;
                toasterOrder.OrderDate = DateTime.Now;
                toasterOrder.PickedUp = false;
                toasterOrder.PickedUpDate = DateTime.Now;
                toasterOrder.ReceiverName = receiverInfo.FirstName + " " + receiverInfo.LastName;
                toasterOrder.ReceiverUserId = this.CheckInItem.UserId;
                toasterOrder.SenderName = businessInfo.BusinessName;
                toasterOrder.SenderStripeCustomerId = string.Empty;
                toasterOrder.SenderUserId = AppDelegate.CurrentUser.UserId;
                toasterOrder.DrinkAmount = drinkAmount;
                toasterOrder.TabsServiceFee = tabFee;
                toasterOrder.TipAmount = 0;
                toasterOrder.TotalOrderAmount = totalAmount;
                toasterOrder.FromBusiness = true;
                toasterOrder.SalesTax = 0;
                toasterOrder.StripeFee = 0;
                toasterOrder.CardChargeAmount = 0;

                await AppDelegate.ToasterOrderFactory.New(toasterOrder);


                BTProgressHUD.ShowSuccessWithStatus(ToastMessage.DrinkSentMsg, Helpers.ToastTime.SuccessTime);

                SendDrinkController.DrinkSent = true;
                var PushNotificationHelper = new PushNotificationHelper(AppDelegate.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.iOS);
                await PushNotificationHelper.SentDrinkPush(AppDelegate.CurrentUser.FirstName, toasterOrder.ReceiverUserId);

                this.NavigationController.PopViewController(true);
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        private async void CreateToasterOrder()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }

                BTProgressHUD.Show(ToastMessage.SendingDrink, -1f, ProgressHUD.MaskType.Black);

                var receiverInfo = await AppDelegate.UsersFactory.GetUser(this.CheckInItem.UserId);
                var businessInfo = await AppDelegate.BusinessFactory.Get(this.Drink.BusinessId);

                if(receiverInfo == null || businessInfo == null || Drink == null 
                    || CheckInItem == null || StripeCustomerInfo == null)
                {
                    return;
                }

                ToasterOrder toasterOrder = new ToasterOrder();
                toasterOrder.BusinessDrinkId = this.Drink.BusinessDrinkId;
                toasterOrder.BusinessId = this.Drink.BusinessId;
                toasterOrder.BusinessName = businessInfo.BusinessName;
                toasterOrder.Charged = false;
                toasterOrder.DrinkName = this.Drink.DrinkName;
                toasterOrder.OrderDate = DateTime.Now;
                toasterOrder.PickedUp = false;
                toasterOrder.PickedUpDate = DateTime.Now;
                toasterOrder.ReceiverName = receiverInfo.FirstName + " " + receiverInfo.LastName;
                toasterOrder.ReceiverUserId = this.CheckInItem.UserId;
                toasterOrder.SenderName = AppDelegate.CurrentUser.FirstName + " " + AppDelegate.CurrentUser.LastName;
                toasterOrder.SenderStripeCustomerId = StripeCustomerInfo.StripeCustomerId;
                toasterOrder.SenderUserId = AppDelegate.CurrentUser.UserId;
                toasterOrder.DrinkAmount = drinkAmount;
                toasterOrder.TabsServiceFee = tabFee;
                toasterOrder.TotalOrderAmount = totalAmount;
                toasterOrder.FromBusiness = false;
                toasterOrder.TipAmount = tipAmount;
                toasterOrder.SalesTax = 0;
                toasterOrder.StripeFee = stripeFee;
                toasterOrder.CardChargeAmount = totalAmount - stripeFee;

                if (usePoint)
                {
                    toasterOrder.PointsAmount = discountAmount;
                    toasterOrder.UsedPointType = SendDrinkHelper.SelectedPointToUse == SendDrinkHelper.PointToUse.Quarter ?
                         ToasterOrder.PointType.Quarter : ToasterOrder.PointType.Full;
                }
                else
                {
                    toasterOrder.PointsAmount = 0;
                    toasterOrder.UsedPointType = ToasterOrder.PointType.None;
                }

                await AppDelegate.ToasterOrderFactory.New(toasterOrder);

                Shared.Models.Points.Point point = new Shared.Models.Points.Point();
                point.UserId = AppDelegate.CurrentUser.UserId;
                point.PointStatus = Shared.Models.Points.Point.ToasterPointStatus.Earned;
                point.EarnedDate = DateTime.Now;
                point.RedeemedDate = null;
                point.PointAmount = (int)Shared.Models.Points.Point.PointAmountScale.SendDrink;
                await AppDelegate.ToasterPointsFactory.NewDailyPoint(point);

                BTProgressHUD.ShowSuccessWithStatus(ToastMessage.DrinkSentMsg, Helpers.ToastTime.SuccessTime);

                SendDrinkController.DrinkSent = true;
                var PushNotificationHelper = new PushNotificationHelper(AppDelegate.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.iOS);
                await PushNotificationHelper.SentDrinkPush(AppDelegate.CurrentUser.FirstName, toasterOrder.ReceiverUserId);
                await PushNotificationHelper.NewPointsPush(point.UserId);

                this.NavigationController.PopViewController(true);
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        partial void ChangePaymentBtn_TouchUpInside(UIButton sender)
        {
            var controller = this.Storyboard.InstantiateViewController("PaymentMethodController") as PaymentMethodController;
            controller.FromQuantityController = true;
            this.NavigationController.PushViewController(controller, true);
        }

        partial void TipDecrementBtn_TouchUpInside(UIButton sender)
        {
            tipAmountText.Text = SendDrinkHelper.ModifyTipCounter(false).ToString();
            var prices = SendDrinkHelper.CalculateUpdatedPrice(Drink.Price, usePoint);

            drinkAmount = prices.Item2;
            tabFee = prices.Item1;
            totalAmount = prices.Item3;
            discountAmount = prices.Item4;
            stripeFee = prices.Item5;
            tipAmount = prices.Item6;
            SetUpdatedPrice(totalAmount);
        }

        partial void TipIncrementBtn_TouchUpInside(UIButton sender)
        {
            tipAmountText.Text = SendDrinkHelper.ModifyTipCounter(true).ToString();
            var prices = SendDrinkHelper.CalculateUpdatedPrice(Drink.Price, usePoint);

            drinkAmount = prices.Item2;
            tabFee = prices.Item1;
            totalAmount = prices.Item3;
            discountAmount = prices.Item4;
            stripeFee = prices.Item5;
            tipAmount = prices.Item6;
            SetUpdatedPrice(totalAmount);
        }

        #endregion

    }
}