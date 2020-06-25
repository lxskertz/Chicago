using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models.Users;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessRegistrationController : BaseViewController
    {

        #region Properties

        /// <summary>
        /// Gets or sets RegisterBusinessDataSource
        /// </summary>
        private DataSource.Business.RegisterBusinessDataSource RegisterBusinessDataSource { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public UITextField BusinessName { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public UITextField PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public UITextField Address { get; set; }

        ///// <summary>
        ///// Gets or sets last name
        ///// </summary>
        //public UITextField SaleTaxRate { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField City { get; set; }
        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public UITextField State { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField Zipcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UISwitch Bar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UISwitch Lounge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UISwitch Club { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UISwitch Restaurant { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UISwitch Other { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Users UserInfo { get; set; }

        public bool EditProfile = false;
        public Business BusinessInfo { get; set; }
        public Address AddressInfo { get; set; }
        public BusinessTypes BusinessTypes { get; set; }

        #endregion

        #region Constructors

        public BusinessRegistrationController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load Data
        /// </summary>
        private void LoadData()
        {
            try
            {
                RegisterBusinessTable.EstimatedRowHeight = 44f;
                RegisterBusinessTable.RowHeight = UITableView.AutomaticDimension;
                RegisterBusinessDataSource = new DataSource.Business.RegisterBusinessDataSource(this);
                RegisterBusinessTable.Source = RegisterBusinessDataSource;
                RegisterBusinessTable.TableFooterView = new UIView();

                UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() =>
                {
                    RegisterBusinessTable.EndEditing(true);
                });

                tapGesture.CancelsTouchesInView = false;
                RegisterBusinessTable.AddGestureRecognizer(tapGesture);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                var itemText = EditProfile ? AppText.Save : AppText.Continue;
                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(itemText, UIBarButtonItemStyle.Plain, async (sender, args) => {
                    await Save();
                }), true);

                LoadData();
            }
            catch (Exception ex)
            {
                var a = ex;
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
                this.Bar.ValueChanged += delegate
                {
                    Bar.On = Bar.On; //SetState(false, true);
            };

                this.Club.ValueChanged += delegate
                {
                    Club.On = Club.On;
                };

                this.Lounge.ValueChanged += delegate
                {
                    Lounge.On = Lounge.On;
                };

                this.Restaurant.ValueChanged += delegate
                {
                    Restaurant.On = Restaurant.On;
                };

                this.Other.ValueChanged += delegate
                {
                    Other.On = Other.On;
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
        private async Task Save()
        {
            try
            {
                if (string.IsNullOrEmpty(BusinessName.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredField, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(PhoneNumber.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredField, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(Address.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredField, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(City.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredField, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(State.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredField, Helpers.ToastTime.ErrorTime);
                    return;
                }
                //else if (string.IsNullOrEmpty(SaleTaxRate.Text))
                //{
                //    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredField, Helpers.ToastTime.ErrorTime);
                //    return;
                //}
                else if (string.IsNullOrEmpty(Zipcode.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredField, Helpers.ToastTime.ErrorTime);
                    return;
                } else if(!Bar.On && !Lounge.On && !Club.On && !Restaurant.On && !Other.On)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.SelectBusinessType, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    //if (this.HideAccountPicture)
                    //{
                    //    await UpdateAccount();
                    //}
                    //else
                    //{
                        await CreateBusinessAccount();
                    //}
                }

                //AppDelegate.Track("Register Business", "Register");

            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task CreateBusinessAccount()
        {
            if (AppDelegate.IsOfflineMode())
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                return;
            }
            BTProgressHUD.Show("Please wait...", -1f, ProgressHUD.MaskType.Black);
            try
            {

                Business business = new Business();
                Address addressModel = new Address();
                BusinessTypes bizType = new BusinessTypes();

                business.BusinessName = BusinessName.Text.Trim();
                business.PhoneNumber = Convert.ToInt64(PhoneNumber.Text);
                //business.SalesTaxRate = Convert.ToDouble(SaleTaxRate.Text.Trim());

                bizType.Bar = Bar.On;
                bizType.Club = Club.On;
                bizType.Lounge = Lounge.On;
                bizType.Other = Other.On;
                bizType.Restaurant = Restaurant.On;

                addressModel.StreetAddress = Address.Text.Trim();
                addressModel.State = State.Text.Trim();
                addressModel.City = City.Text.Trim();
                addressModel.Country = "USA";
                addressModel.ZipCode = Zipcode.Text.Trim();

                if (EditProfile)
                {
                    business.BusinessId = this.BusinessInfo.BusinessId;
                    await AppDelegate.BusinessFactory.Update(business);

                    bizType.BusinessId = this.BusinessInfo.BusinessId;
                    bizType.BusinessTypeId = this.BusinessTypes.BusinessTypeId;
                    await AppDelegate.BusinessTypesFactory.Update(bizType);

                    addressModel.AddressId = this.AddressInfo.AddressId;
                    addressModel.BusinessId = this.BusinessInfo.BusinessId;
                    await AppDelegate.AddressFactory.UpdateAddress(addressModel);

                    BTProgressHUD.Dismiss();
                    BusinessProfileController.RequiresRefresh = true;
                    this.NavigationController.PopViewController(true);
                }
                else
                {
                    business.UserId = UserInfo.UserId;
                    await AppDelegate.BusinessFactory.CreateBusiness(business);
                    var businessCheck = await AppDelegate.BusinessFactory.GetByUserId(UserInfo.UserId);

                    if (businessCheck != null)
                    {
                        bizType.BusinessId = businessCheck.BusinessId;

                        await AppDelegate.BusinessTypesFactory.AddBusinessType(bizType);

                        addressModel.BusinessId = businessCheck.BusinessId;
                        await AppDelegate.AddressFactory.AddAddress(addressModel);

                        BTProgressHUD.Dismiss();
                        UIViewController home = this.Storyboard.InstantiateViewController("BusinessHomeController") as BusinessHomeController;
                        this.NavigationController.SetViewControllers(new UIViewController[] { home }, true);
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        #endregion

    }
}