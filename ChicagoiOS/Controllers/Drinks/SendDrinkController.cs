using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Drinks;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.CheckIns;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class SendDrinkController : BaseViewController
    {

        #region Properties

        public static bool DrinkSent { get; set; }

        public bool FromBusiness { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CheckIn CheckInItem { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private SendDrinkDataSource SendDrinkDataSource { get; set; }

        #endregion

        #region Constructors

        public SendDrinkController(IntPtr handle) : base(handle)
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

                await GetDrinks();
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
            base.ViewWillAppear(animated);

            if (DrinkSent)
            {
                DrinkSent = false;
                this.NavigationController.PopViewController(true);
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetDrinks()
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
                    var businessInfo = await AppDelegate.BusinessFactory.Get(CheckInItem.BusinessId);

                    if (businessInfo != null)
                    {
                        BusinessName.Text = string.IsNullOrEmpty(businessInfo.BusinessName) ? "" : businessInfo.BusinessName;

                        var addressInfo = await AppDelegate.AddressFactory.GetAddressByBusinessId(businessInfo.BusinessId);

                        if (addressInfo != null)
                        {
                            var addy = string.IsNullOrEmpty(addressInfo.StreetAddress) ? "" : addressInfo.StreetAddress + ", ";
                            var city = string.IsNullOrEmpty(addressInfo.City) ? "" : addressInfo.City + ", ";
                            var state = string.IsNullOrEmpty(addressInfo.State) ? "" : addressInfo.State + " ";
                            var zipcode = string.IsNullOrEmpty(addressInfo.ZipCode) ? "" : addressInfo.ZipCode;
                            var address = addy + city + state + zipcode;
                            BusinessAddress.Text = address;
                        }

                        var drinks = await AppDelegate.BusinessDrinkFactory.Get(businessInfo.BusinessId);

                        if (drinks != null)
                        {
                            SendDrinkTable.EstimatedRowHeight = 44f;
                            SendDrinkTable.RowHeight = UITableView.AutomaticDimension;
                            SendDrinkDataSource = new SendDrinkDataSource(this, drinks.ToList());
                            SendDrinkTable.Source = SendDrinkDataSource;
                            SendDrinkTable.TableFooterView = new UIView();
                        }
                        else
                        {
                            //BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                        }
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


        #endregion
    }
}