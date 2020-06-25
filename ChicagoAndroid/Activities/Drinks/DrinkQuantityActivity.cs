using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Stripe;
using Tabs.Mobile.Shared.Models.Drinks;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.Shared.Models.Payment;
using Tabs.Mobile.Shared.Models.Orders;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Drinks
{
    [Activity(Label = "Select Quantity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class DrinkQuantityActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        TextView drinkName;
        TextView price;
        TextView updatedPrice;
        Button increment;
        Button decrement;
        TextView quantity;
        RelativeLayout PaymentOptionView;
        TextView PaymentOptionText;
        TextView PaymentDisclaimer;
        TextView SelectPaymentOption;
        Button startTab;
        Button changePayment;
        TextView pointDiscountLbl;

        Button tipIncrementBtn;
        Button tipDecrementBtn;
        TextView tipHeader;
        RelativeLayout tipLayout;
        TextView tipAmountText;

        private double totalAmount = 0;
        private double drinkAmount = 0;
        private double tabFee = 0;
        private double discountAmount = 0;
        private double stripeFee = 0;
        private double tipAmount = 0;
        private IMenuItem myMenu;
        private int totalEarnedPoints = 0;
        private bool usePoint = false;

        #endregion

        #region Properties

        public static bool RequiresRefresh { get; set; }

        public static Card defaultPayment;
        private bool FromBusiness { get; set; }

        public StripeCustomerInfo StripeCustomerInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessDrink Drink { get; set; }


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
                this.FromBusiness = Intent.GetBooleanExtra("FromBusiness", false);
                SetContentView(Resource.Layout.DrinkQuantity);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                SendDrinkHelper.counter = 1;
                SendDrinkHelper.tipCounter = 0;
                SendDrinkHelper.SelectedPointToUse = SendDrinkHelper.PointToUse.None;

                drinkName = FindViewById<TextView>(Resource.Id.title);
                price = FindViewById<TextView>(Resource.Id.subTitle);
                updatedPrice = FindViewById<TextView>(Resource.Id.updatedPrice);
                quantity = FindViewById<TextView>(Resource.Id.quantity);
                increment = FindViewById<Button>(Resource.Id.addBtn);
                decrement = FindViewById<Button>(Resource.Id.minusBtn);
                startTab = FindViewById<Button>(Resource.Id.startTab);
                PaymentOptionText = FindViewById<TextView>(Resource.Id.paymentOptionText);
                PaymentDisclaimer = FindViewById<TextView>(Resource.Id.paymentDisclaimer);
                SelectPaymentOption = FindViewById<TextView>(Resource.Id.selectPaymentOption);
                PaymentOptionView = FindViewById<RelativeLayout>(Resource.Id.paymentOptionView);
                changePayment = FindViewById<Button>(Resource.Id.changePaymentBtn);
                pointDiscountLbl = FindViewById<TextView>(Resource.Id.pointDiscountText);

                tipHeader = FindViewById<TextView>(Resource.Id.serverTipHeader);
                tipIncrementBtn = FindViewById<Button>(Resource.Id.tipIncrementBtn);
                tipDecrementBtn = FindViewById<Button>(Resource.Id.tipDecrementBtn);
                tipLayout = FindViewById<RelativeLayout>(Resource.Id.tipLayout);
                tipAmountText = FindViewById<TextView>(Resource.Id.tipAmountText);

                this.Drink = JsonConvert.DeserializeObject<BusinessDrink>(Intent.GetStringExtra("BusinessDrink"));
                this.CheckInItem = JsonConvert.DeserializeObject<CheckIn>(Intent.GetStringExtra("CheckInItem"));

                tipHeader.Visibility = ViewStates.Gone;
                tipLayout.Visibility = ViewStates.Gone;

                if (Drink != null)
                {
                    drinkName.Text = string.IsNullOrEmpty(Drink.DrinkName) ? "" : Drink.DrinkName;
                    price.Text = SendDrinkHelper.CurrencySymbol + Drink.Price.ToString() + SendDrinkHelper.TabsServiceFeeText;

                    SetPrices();
                    SetUpdatedPrice(totalAmount);
                }

                increment.Click += delegate
                {
                    quantity.Text = SendDrinkHelper.ModifyCounter(true).ToString();
                    var prices = SendDrinkHelper.CalculateUpdatedPrice(Drink.Price, usePoint);

                    drinkAmount = prices.Item2;
                    tabFee = prices.Item1;
                    totalAmount = prices.Item3;
                    discountAmount = prices.Item4;
                    stripeFee = prices.Item5;
                    tipAmount = prices.Item6;
                    SetUpdatedPrice(totalAmount);
                };

                decrement.Click += delegate
                {
                    quantity.Text = SendDrinkHelper.ModifyCounter(false).ToString();
                    var prices = SendDrinkHelper.CalculateUpdatedPrice(Drink.Price, usePoint);

                    drinkAmount = prices.Item2;
                    tabFee = prices.Item1;
                    totalAmount = prices.Item3;
                    discountAmount = prices.Item4;
                    stripeFee = prices.Item5;
                    tipAmount = prices.Item6;
                    SetUpdatedPrice(totalAmount);
                };

                tipIncrementBtn.Click += delegate
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
                };

                tipDecrementBtn.Click += delegate
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
                };

                if (FromBusiness)
                {
                    startTab.Text = AppText.Send;
                    PaymentOptionText.Visibility = ViewStates.Gone;
                    PaymentOptionView.Visibility = ViewStates.Gone;
                    PaymentDisclaimer.Visibility = ViewStates.Gone;

                    tipHeader.Visibility = ViewStates.Gone;
                    tipLayout.Visibility = ViewStates.Gone;
                }

                startTab.Click += delegate
                {
                    if (FromBusiness)
                    {
                        CreateBusinessOrder();
                    }
                    else
                    {
                        if (defaultPayment == null)
                        {
                            Toast.MakeText(this, ToastMessage.NoPaymentMethod, ToastLength.Short).Show();
                            //Intent activity = new Intent(this, typeof(Payments.PaymentMethodsActivity));
                            //activity.PutExtra("FromQuantityActivity", true);
                            //this.StartActivity(activity);
                        }
                        else
                        {
                            CreateToasterOrder();
                        }
                    }
                };

                changePayment.Click += delegate
                {
                //if(changePayment.Text == AppText.AddPaymentText)
                //{

                //}
                Intent activity = new Intent(this, typeof(Payments.PaymentMethodsActivity));
                    activity.PutExtra("FromQuantityActivity", true);
                    this.StartActivity(activity);
                };

                await GetDefaultCard();
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
            myMenu = menu.FindItem(Resource.Id.menuAction);
            this.FromBusiness = Intent.GetBooleanExtra("FromBusiness", false);
            var mText = FromBusiness ? "" : AppText.UsePoints;
            myMenu.SetTitle(mText);

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
                    GetEarnedPoints();
                    return true;
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void SetUpdatedPrice(double amt)
        {
            updatedPrice.Text = SendDrinkHelper.CurrencySymbol + amt.ToString();
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
                if (myMenu.TitleFormatted.ToString() == AppText.DontUsePoints)
                {
                    usePoint = false;
                    myMenu.SetTitle(AppText.UsePoints);
                    pointDiscountLbl.Visibility = ViewStates.Gone;
                    ApplyRemovePoints();
                }
                else
                {
                    if (this.CheckNetworkConnectivity() == null)
                    {
                        Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                    }
                    else
                    {
                        this.ShowProgressbar(true, "", ToastMessage.PleaseWait);
                        totalEarnedPoints = await App.ToasterPointsFactory.GetTotalEarnedPoints(this.CurrentUser.UserId);

                        if (totalEarnedPoints < SendDrinkHelper.QuarterPercentOff)
                        {
                            pointDiscountLbl.Visibility = ViewStates.Gone;
                            Toast.MakeText(this, ToastMessage.NotEnoughPoints, ToastLength.Short).Show();
                        }
                        else
                        {
                            myMenu.SetTitle(AppText.DontUsePoints);
                            usePoint = true;
                            if (totalEarnedPoints >= SendDrinkHelper.FullOff)
                            {
                                SendDrinkHelper.SelectedPointToUse = SendDrinkHelper.PointToUse.Full;
                            }
                            else
                            {
                                SendDrinkHelper.SelectedPointToUse = SendDrinkHelper.PointToUse.Quarter;
                            }
                            pointDiscountLbl.Visibility = ViewStates.Visible;
                            pointDiscountLbl.Text = SendDrinkHelper.GetPointToUseText();
                            ApplyRemovePoints();

                        }
                        this.ShowProgressbar(false, "", ToastMessage.PleaseWait);
                    }
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        private async Task GetDefaultCard()
        {
            try
            {
                this.StripeCustomerInfo = await App.StripeCustomerInfoFactory.Get(this.CurrentUser.UserId);

                if (this.StripeCustomerInfo != null)
                {
                    var customer = await App.CustomerPaymentInfoFactory.GetCustomer(StripeCustomerInfo.StripeCustomerId);

                    if (customer != null)
                    {
                        defaultPayment = await App.CustomerPaymentInfoFactory.GetDefaultCard(this.StripeCustomerInfo.StripeCustomerId, customer.DefaultSourceId);

                        if (defaultPayment != null)
                        {
                            var text = string.IsNullOrEmpty(defaultPayment.Last4) ? "" :
                                AppText.Asterisks + defaultPayment.Last4 + "    " + defaultPayment.ExpMonth + "/" + defaultPayment.ExpYear;
                            SelectPaymentOption.Text = text;
                        }
                    }
                    else
                    {
                        changePayment.Text = AppText.AddPaymentText;
                    }
                }
                else
                {
                    changePayment.Text = AppText.AddPaymentText;
                }
            }
            catch (Exception) { }
        }

        private void SetPrices()
        {
            drinkAmount = this.Drink.Price;
            tabFee = SendDrinkHelper.tabServiceFee * this.Drink.Price;
            stripeFee = (SendDrinkHelper.stripeServiceFee * tabFee) + SendDrinkHelper.additionalStripeServiceFee;
            totalAmount = SendDrinkHelper.InitialUpdatedPrice(Drink.Price, usePoint);
        }

        /// <summary>
        /// 
        /// </summary>
        private void HideShowPaymentOption(bool hide)
        {
            //if (SelectedCard != null)
            //{
            //    var text = string.IsNullOrEmpty(SelectedCard.Last4) ? "" : AppText.Asterisks + SelectedCard.Last4 + "    " + SelectedCard.ExpMonth + "/" + SelectedCard.ExpYear;
            //    SelectPaymentOption.Text = !hide ? text : "";
            //}
            if (defaultPayment != null)
            {
                changePayment.Text = AppText.Change;
                var text = string.IsNullOrEmpty(defaultPayment.Last4) ? "" : AppText.Asterisks + defaultPayment.Last4 + "    " + defaultPayment.ExpMonth + "/" + defaultPayment.ExpYear;
                SelectPaymentOption.Text = !hide ? text : "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnResume()
        {
            try
            {
                base.OnResume();

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;

                    if (defaultPayment != null)
                    {
                        HideShowPaymentOption(false);
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
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                    return;
                }

                this.ShowProgressbar(true, "", ToastMessage.SendingDrink);

                var receiverInfo = await App.UsersFactory.GetUser(this.CheckInItem.UserId);
                var businessInfo = await App.BusinessFactory.Get(this.Drink.BusinessId);

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
                toasterOrder.SenderUserId = this.CurrentUser.UserId;
                toasterOrder.DrinkAmount = drinkAmount;
                toasterOrder.TabsServiceFee = tabFee;
                toasterOrder.TipAmount = 0;
                toasterOrder.TotalOrderAmount = totalAmount;
                toasterOrder.FromBusiness = true;
                toasterOrder.SalesTax = 0;
                toasterOrder.StripeFee = 0;
                toasterOrder.CardChargeAmount = 0;

                await App.ToasterOrderFactory.New(toasterOrder);

                this.ShowProgressbar(false, "", ToastMessage.SendingDrink);
                Toast.MakeText(this, ToastMessage.DrinkSentMsg, ToastLength.Short).Show();

                SendDrinkActivity.DrinkSent = true;

                var PushNotificationHelper = new PushNotificationHelper(App.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.Android);
                await PushNotificationHelper.SentDrinkPush(this.CurrentUser.FirstName, toasterOrder.ReceiverUserId);

                this.Finish();
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.SendingDrink);
                Toast.MakeText(this, ToastMessage.ServerError, ToastLength.Short).Show();
            }
        }

        private async void CreateToasterOrder()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                    return;
                }

                this.ShowProgressbar(true, "", ToastMessage.SendingDrink);

                var receiverInfo = await App.UsersFactory.GetUser(this.CheckInItem.UserId);
                var businessInfo = await App.BusinessFactory.Get(this.Drink.BusinessId);

                if (receiverInfo == null || businessInfo == null || Drink == null
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
                toasterOrder.SenderName = this.CurrentUser.FirstName + " " + this.CurrentUser.LastName;
                toasterOrder.SenderStripeCustomerId = StripeCustomerInfo.StripeCustomerId;
                toasterOrder.SenderUserId = this.CurrentUser.UserId;
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

                await App.ToasterOrderFactory.New(toasterOrder);

                Shared.Models.Points.Point point = new Shared.Models.Points.Point();
                point.UserId = this.CurrentUser.UserId;
                point.PointStatus = Shared.Models.Points.Point.ToasterPointStatus.Earned;
                point.EarnedDate = DateTime.Now;
                point.RedeemedDate = null;
                point.PointAmount = (int)Shared.Models.Points.Point.PointAmountScale.SendDrink;
                await App.ToasterPointsFactory.NewDailyPoint(point);

                this.ShowProgressbar(false, "", ToastMessage.SendingDrink);
                Toast.MakeText(this, ToastMessage.DrinkSentMsg, ToastLength.Short).Show();

                SendDrinkActivity.DrinkSent = true;

                var PushNotificationHelper = new PushNotificationHelper(App.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.Android);
                await PushNotificationHelper.SentDrinkPush(this.CurrentUser.FirstName, toasterOrder.ReceiverUserId);
                await PushNotificationHelper.NewPointsPush(point.UserId);

                this.Finish();
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.SendingDrink);
                Toast.MakeText(this, ToastMessage.ServerError, ToastLength.Short).Show();
            }
        }


        #endregion

    }
}