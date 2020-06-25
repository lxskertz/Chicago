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
using Android.Widget;
using Android.Views.InputMethods;
using Android.Text;
using Android.Text.Method;
using Android.Support.Design.Widget;
using Android.Text.Style;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using Stripe;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Payment;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Payments
{
    [Activity(Label = "Add Card", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class AddPaymentMethodActivity : BaseActivity, TextView.IOnEditorActionListener, ITextWatcher, View.IOnKeyListener
    {

        #region Constants, Enums, and Variables

        private AppCompatEditText cardNumber;
        private AppCompatEditText expDate;
        private AppCompatEditText cvv;
        private AppCompatEditText zipCode;
        private TextInputLayout cardNumberLayout;
        private TextInputLayout expDateLayout;
        private TextInputLayout cvvLayout;
        private TextInputLayout zipCodeLayout;
        private FrameLayout paymentViewLayout;
        private Button addBtn;

        private bool delTapped = false;

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.AddPayment);

                //add the back arrow
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                addBtn = FindViewById<Button>(Resource.Id.addCard);
                cardNumberLayout = FindViewById<TextInputLayout>(Resource.Id.cardNumber_layout);
                expDateLayout = FindViewById<TextInputLayout>(Resource.Id.expirationDate_layout);
                cvvLayout = FindViewById<TextInputLayout>(Resource.Id.cvv_layout);
                zipCodeLayout = FindViewById<TextInputLayout>(Resource.Id.zipCode_layout);
                paymentViewLayout = FindViewById<FrameLayout>(Resource.Id.paymentLayout);

                cardNumber = FindViewById<AppCompatEditText>(Resource.Id.cardNumber);
                expDate = FindViewById<AppCompatEditText>(Resource.Id.expirationDate);
                cvv = FindViewById<AppCompatEditText>(Resource.Id.cvv);
                zipCode = FindViewById<AppCompatEditText>(Resource.Id.zipCode);

                cvv.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(3) });
                expDate.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(5) });
                zipCode.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(6) });
                cardNumber.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(19) });

                expDate.AddTextChangedListener(this);
                expDate.SetOnKeyListener(this);

                addBtn.Click += delegate
                {
                    AddCard();
                };
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async void AddCard()
        {
            if (this.CheckNetworkConnectivity() == null)
            {
                this.ShowSnack(paymentViewLayout, ToastMessage.NoInternet, "OK");
            }
            else
            {
                try
                {
                    if (!ValidateInput(cardNumberLayout, cardNumber, ToastMessage.EmptyCardNumber))
                    {
                        return;
                    }
                    else if (!ValidateInput(expDateLayout, expDate, ToastMessage.EmptyExpDate))
                    {
                        return;
                    }
                    else if (!ValidateInput(cvvLayout, cvv, ToastMessage.EmptyCVV))
                    {
                        return;
                    }
                    else if (!ValidateInput(zipCodeLayout, zipCode, ToastMessage.EmptyZipCode))
                    {
                        return;
                    }
                    else
                    {
                        this.ShowProgressbar(true, "", ToastMessage.Processing);

                        CustomerPaymentInfo paymentInfo = new CustomerPaymentInfo();
                        paymentInfo.Number = cardNumber.Text; 
                        paymentInfo.ExpirationMonth = string.IsNullOrEmpty(expDate.Text) ? 0 : Convert.ToInt32(expDate.Text.Substring(0, 2));
                        var yearSub = string.IsNullOrEmpty(expDate.Text) ? 0 : Convert.ToInt32(expDate.Text.Substring(3, 2));
                        paymentInfo.ExpirationYear = Convert.ToInt32("20" + yearSub);
                        paymentInfo.UserId = this.CurrentUser.UserId;
                        paymentInfo.Email = this.CurrentUser.Email;
                        paymentInfo.Cvc = cvv.Text;

                        var stripeCustomerInfo = await App.StripeCustomerInfoFactory.Get(this.CurrentUser.UserId);

                        if (stripeCustomerInfo == null)
                        {
                            await App.CustomerPaymentInfoFactory.CreateCustomerPaymentInfo(paymentInfo);
                        }
                        else
                        {
                            await App.CustomerPaymentInfoFactory.AddCard(paymentInfo, stripeCustomerInfo.StripeCustomerId);
                        }
                    }

                    this.ShowProgressbar(false, "", ToastMessage.Processing);
                    PaymentMethodsActivity.RequiresRefresh = true;
                    this.Finish();
                }
                catch (StripeException ex)
                {
                    var a = ex;
                    this.ShowProgressbar(false, "", ToastMessage.Charging);
                    ShowSnack(paymentViewLayout, ex.Message, "OK");
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                }
                catch (Exception ex)
                {
                    var a = ex;
                    ShowSnack(paymentViewLayout, ToastMessage.ServerError, "OK");
                    this.ShowProgressbar(false, "", ToastMessage.Processing);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="keyCode"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnKey(View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Del)
            {
                delTapped = true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void AfterTextChanged(IEditable s)
        {
            if (s.Length() == 2 && !expDate.Text.Contains("/") && !delTapped)
            {
                var newText = expDate.Text + "/";
                expDate.Text = newText;
                expDate.SetSelection(expDate.Text.Length);
            }
            delTapped = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="after"></param>
        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start"></param>
        /// <param name="before"></param>
        /// <param name="count"></param>
        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == ImeAction.Done)
            {
                //await ProcessPayment();
            }
        }

        /// <summary>
        /// Observes the TextView's ImeAction so an action can be taken on keypress
        /// Called when an action is being performed.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="actionId"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            return true;
        }

        /// <summary>
        /// This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <Param name="item"></Param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                this.Finish();
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion

    }
}