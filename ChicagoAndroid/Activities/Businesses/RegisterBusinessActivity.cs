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
using Tabs.Mobile.Shared.Resources;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Users;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Businesses
{
    [Activity(Label = "Business Information", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class RegisterBusinessActivity : BaseActivity, TextView.IOnEditorActionListener
    {

        #region Constants, Enums, and Variables

        private AppCompatEditText businessName;
        private AppCompatEditText phoneNumber;
        private AppCompatEditText address;
        private AppCompatEditText city;
        private AppCompatEditText state;
        private AppCompatEditText zipCode;
        private TextInputLayout businessNameLayout;
        private TextInputLayout addressLayout;
        private TextInputLayout cityLayout;
        private TextInputLayout stateLayout;
        private TextInputLayout zipCodeLayout;
        private TextInputLayout phoneNumberLayout;
        private FrameLayout registerLayout;
        private CheckBox isClub;
        private CheckBox isBar;
        private CheckBox isLounge;
        private CheckBox isRestaurant;
        private CheckBox isOther;
        private Button continueBtn;

        private bool EditProfile = false;
        public Business BusinessInfo { get; set; }
        public Address AddressInfo { get; set; }
        public BusinessTypes BusinessTypes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// handle when user tap done on the keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == ImeAction.Done)
            {
                await CreateBusinessAccount();
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
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.RegisterBusiness);

                //add the back arrow
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                businessNameLayout = FindViewById<TextInputLayout>(Resource.Id.businesName_layout);
                addressLayout = FindViewById<TextInputLayout>(Resource.Id.businesAddress_layout);
                cityLayout = FindViewById<TextInputLayout>(Resource.Id.city_layout);
                stateLayout = FindViewById<TextInputLayout>(Resource.Id.state_layout);
                zipCodeLayout = FindViewById<TextInputLayout>(Resource.Id.zipcode_layout);
                phoneNumberLayout = FindViewById<TextInputLayout>(Resource.Id.phoneNumber_layout);
                //saleTaxRateLayout = FindViewById<TextInputLayout>(Resource.Id.saleTaxRate_layout);

                businessName = FindViewById<AppCompatEditText>(Resource.Id.businessName);
                //saleTaxRate = FindViewById<AppCompatEditText>(Resource.Id.saleTaxRate);
                address = FindViewById<AppCompatEditText>(Resource.Id.businessAddress);
                city = FindViewById<AppCompatEditText>(Resource.Id.city);
                state = FindViewById<AppCompatEditText>(Resource.Id.state);
                zipCode = FindViewById<AppCompatEditText>(Resource.Id.zipcode);
                phoneNumber = FindViewById<AppCompatEditText>(Resource.Id.phoneNumber);
                registerLayout = FindViewById<FrameLayout>(Resource.Id.registerBusinessLayout);
                isBar = FindViewById<CheckBox>(Resource.Id.bar);
                isClub = FindViewById<CheckBox>(Resource.Id.club);
                isLounge = FindViewById<CheckBox>(Resource.Id.lounge);
                isRestaurant = FindViewById<CheckBox>(Resource.Id.restaurant);
                isOther = FindViewById<CheckBox>(Resource.Id.Other);
                continueBtn = FindViewById<Button>(Resource.Id.continueBtn);

                continueBtn.Click += async delegate
                {
                    await CreateBusinessAccount();
                };

                EditProfile = Intent.GetBooleanExtra("EditProfile", false);

                if (EditProfile)
                {
                    continueBtn.Text = AppText.Save;
                    this.BusinessInfo = JsonConvert.DeserializeObject<Business>(Intent.GetStringExtra("BusinessInfo"));
                    this.AddressInfo = JsonConvert.DeserializeObject<Address>(Intent.GetStringExtra("AddressInfo"));
                    this.BusinessTypes = JsonConvert.DeserializeObject<BusinessTypes>(Intent.GetStringExtra("BusinessTypes"));

                    if (BusinessInfo != null & this.AddressInfo != null && this.BusinessTypes != null)
                    {
                        BindData();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void BindData()
        {
            businessName.Text = string.IsNullOrEmpty(BusinessInfo.BusinessName) ? "" : BusinessInfo.BusinessName;
            phoneNumber.Text = BusinessInfo.PhoneNumber.ToString();
            //phoneNumber.Text = BusinessInfo.SalesTaxRate.ToString();

            address.Text = string.IsNullOrEmpty(this.AddressInfo.StreetAddress) ? "" : this.AddressInfo.StreetAddress + ", ";
            city.Text = string.IsNullOrEmpty(this.AddressInfo.City) ? "" : this.AddressInfo.City + ", ";
            state.Text = string.IsNullOrEmpty(this.AddressInfo.State) ? "" : this.AddressInfo.State + " ";
            zipCode.Text = string.IsNullOrEmpty(this.AddressInfo.ZipCode) ? "" : this.AddressInfo.ZipCode;

            isBar.Checked = this.BusinessTypes.Bar;
            isClub.Checked = this.BusinessTypes.Club;
            isLounge.Checked = this.BusinessTypes.Lounge;
            isRestaurant.Checked = this.BusinessTypes.Restaurant;
            isOther.Checked = this.BusinessTypes.Other;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task CreateBusinessAccount()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(registerLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                if (!this.ValidateInput(businessNameLayout, businessName, ToastMessage.RequiredField))
                {
                    return;
                }
                if (!this.ValidateInput(phoneNumberLayout, phoneNumber, ToastMessage.RequiredField))
                {
                    return;
                }
                if (!this.ValidateInput(addressLayout, address, ToastMessage.RequiredField))
                {
                    return;
                }
                if (!this.ValidateInput(cityLayout, city, ToastMessage.RequiredField))
                {
                    return;
                }
                if (!this.ValidateInput(stateLayout, state, ToastMessage.RequiredField))
                {
                    return;
                }
                if (!this.ValidateInput(zipCodeLayout, zipCode, ToastMessage.RequiredField))
                {
                    return;
                }
                //if (!this.ValidateInput(saleTaxRateLayout, saleTaxRate, ToastMessage.RequiredField))
                //{
                //    return;
                //}
                else if (!isBar.Checked && !isLounge.Checked && !isClub.Checked && !isRestaurant.Checked && !isOther.Checked)
                {
                    ShowSnack(registerLayout, ToastMessage.SelectBusinessType, "OK");
                    return;
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.PleaseWait);
                    try
                    {
                        Business business = new Business();
                        Address addressModel = new Address();
                        BusinessTypes bizType = new BusinessTypes();

                        business.BusinessName = businessName.Text.Trim();
                        business.PhoneNumber = Convert.ToInt64(phoneNumber.Text.Trim());
                        //business.SalesTaxRate = Convert.ToDouble(saleTaxRate.Text.Trim());

                        bizType.Bar = isBar.Checked;
                        bizType.Club = isClub.Checked;
                        bizType.Lounge = isLounge.Checked;
                        bizType.Other = isOther.Checked;
                        bizType.Restaurant = isRestaurant.Checked;

                        addressModel.StreetAddress = address.Text.Trim();
                        addressModel.State = state.Text.Trim();
                        addressModel.City = city.Text.Trim();
                        addressModel.Country = "USA";
                        addressModel.ZipCode = zipCode.Text.Trim();

                        if (EditProfile)
                        {
                            business.BusinessId = this.BusinessInfo.BusinessId;
                            await App.BusinessFactory.Update(business);

                            bizType.BusinessId = this.BusinessInfo.BusinessId;
                            bizType.BusinessTypeId = this.BusinessTypes.BusinessTypeId;
                            await App.BusinessTypesFactory.Update(bizType);

                            addressModel.AddressId = this.AddressInfo.AddressId;
                            addressModel.BusinessId = this.BusinessInfo.BusinessId;
                            await App.AddressFactory.UpdateAddress(addressModel);

                            this.ShowProgressbar(false, "", ToastMessage.PleaseWait);
                            Fragments.Business.BusinessProfileFragment.RequiresRefresh = true;
                            this.Finish();
                        }
                        else
                        {
                            business.UserId = this.CurrentUser.UserId;
                            await App.BusinessFactory.CreateBusiness(business);

                            var businessCheck = await App.BusinessFactory.GetByUserId(this.CurrentUser.UserId);

                            if (businessCheck != null)
                            {
                                bizType.BusinessId = businessCheck.BusinessId;

                                await App.BusinessTypesFactory.AddBusinessType(bizType);

                                addressModel.BusinessId = businessCheck.BusinessId;

                                await App.AddressFactory.AddAddress(addressModel);

                                this.ShowProgressbar(false, "", ToastMessage.PleaseWait);

                                StartActivity(typeof(BusinessHomeActivity));
                                this.Finish();
                            }
                            else
                            {
                                ShowSnack(registerLayout, ToastMessage.ServerError, "OK");
                            }
                        }

                    }

                    catch (Exception)
                    {
                        this.ShowProgressbar(false, "", ToastMessage.PleaseWait);
                        ShowSnack(registerLayout, ToastMessage.ServerError, "OK");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

    }
}