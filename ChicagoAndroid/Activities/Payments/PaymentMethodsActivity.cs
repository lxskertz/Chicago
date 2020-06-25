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
using Stripe;
using Tabs.Mobile.ChicagoAndroid.Adapters.Payments;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Payment;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Payments
{
    [Activity(Label = "Payment Methods", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class PaymentMethodsActivity : BaseActivity
    {

        #region Properties

        private ListView paymentMethodsList;
        private PaymentMethodsAdapter PaymentMethodsAdapter;
        public static bool RequiresRefresh = false;
        private IMenuItem myMenu;
        public StripeCustomerInfo StripeCustomerInfo { get; set; }

        public bool FromQuantityActivity{ get; set; }

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
                SetContentView(Resource.Layout.PaymentMethods);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                paymentMethodsList = FindViewById<ListView>(Resource.Id.paymentMethodsList);

                this.FromQuantityActivity = Intent.GetBooleanExtra("FromQuantityActivity", false);

                if (FromQuantityActivity)
                {
                    this.Title = AppText.SelectPaymentMethod;
                }

                await LoadData();
            }
            catch (Exception)
            {
            }
        }

        private void RemoveAddCard(int count)
        {
            if (count == 4 && myMenu != null)
            {
                myMenu.SetTitle("");
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
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {

                    this.ShowProgressbar(true, "", ToastMessage.Loading);

                    this.StripeCustomerInfo = await App.StripeCustomerInfoFactory.Get(this.CurrentUser.UserId);

                    if (this.StripeCustomerInfo != null)
                    {
                        var paymentMethods = await App.CustomerPaymentInfoFactory.GetAllCards(this.StripeCustomerInfo.StripeCustomerId);

                        if (paymentMethods != null)
                        {
                            RemoveAddCard(paymentMethods.Count);
                            PaymentMethodsAdapter = new PaymentMethodsAdapter(this, paymentMethods);
                            paymentMethodsList.Adapter = PaymentMethodsAdapter;
                            paymentMethodsList.ItemClick += PaymentMethodsAdapter.OnListItemClick;
                            paymentMethodsList.DividerHeight = 2;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu_with_text, menu);
            myMenu = menu.FindItem(Resource.Id.menuAction);
            myMenu.SetTitle(AppText.AddCard);

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
                    if (myMenu != null && !string.IsNullOrEmpty(myMenu.TitleFormatted.ToString())) {
                        Intent activity = new Intent(this, typeof(AddPaymentMethodActivity));
                        this.StartActivity(activity);
                    }                   
                    return true;
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        public async void DeleteCard(int position)
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    if (this.StripeCustomerInfo != null)
                    {
                        var card = this.PaymentMethodsAdapter.Cards.ElementAtOrDefault(position);
                        this.ShowProgressbar(true, "", ToastMessage.Deleting);
                        await App.CustomerPaymentInfoFactory.DeleteCard(card.Id, this.StripeCustomerInfo.StripeCustomerId);

                        this.PaymentMethodsAdapter.Cards.Remove(this.PaymentMethodsAdapter.Cards.ElementAtOrDefault(position));
                        this.RunOnUiThread(() =>
                        {
                            this.PaymentMethodsAdapter.NotifyDataSetChanged();
                        });

                        this.ShowProgressbar(false, "", ToastMessage.Deleting);
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        /// <summary>
        /// Called when the fragment is visible to the user and actively running.
        /// </summary>
        protected async override void OnResume()
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
                    }

                    if(StripeCustomerInfo == null)
                    {
                        this.StripeCustomerInfo = await App.StripeCustomerInfoFactory.Get(this.CurrentUser.UserId);
                    }

                    if (this.StripeCustomerInfo != null)
                    {
                        var paymentMethods = await App.CustomerPaymentInfoFactory.GetAllCards(this.StripeCustomerInfo.StripeCustomerId);

                        if (paymentMethods != null)
                        {
                            RemoveAddCard(paymentMethods.Count);
                            if (PaymentMethodsAdapter == null)
                            {
                                PaymentMethodsAdapter = new PaymentMethodsAdapter(this, paymentMethods);
                                paymentMethodsList.Adapter = PaymentMethodsAdapter;
                                paymentMethodsList.ItemClick += PaymentMethodsAdapter.OnListItemClick;
                                paymentMethodsList.DividerHeight = 2;
                            }
                            else
                            {
                                this.PaymentMethodsAdapter.Cards = paymentMethods;
                                //this.PaymentMethodsAdapter.NotifyDataSetChanged();
                                this.RunOnUiThread(() =>
                                {
                                    this.PaymentMethodsAdapter.NotifyDataSetChanged();
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