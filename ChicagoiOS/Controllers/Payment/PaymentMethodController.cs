using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Stripe;
using Tabs.Mobile.ChicagoiOS.DataSource.Payments;
using Tabs.Mobile.Shared.Models.Payment;
using BigTed;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class PaymentMethodController : BaseViewController
    {

        #region Properties

        public static bool RequiresRefresh { get; set; }

        public StripeCustomerInfo StripeCustomerInfo { get; set; }

        public bool FromQuantityController { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private PaymentMethodsDataSource PaymentMethodsDataSource { get; set; }

        #endregion

        #region Constructors

        public PaymentMethodController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(AppText.AddCard, UIBarButtonItemStyle.Plain, (sender, args) =>
                {
                    var controller = this.Storyboard.InstantiateViewController("AddPaymentController") as AddPaymentController;
                    this.NavigationController.PushViewController(controller, true);
                }), true);

                if (FromQuantityController)
                {
                    this.Title = AppText.SelectPaymentMethod;
                }

                await GetPaymentMethods();
            }
            catch (Exception)
            {
            }
        }

        private void RemoveAddCard(int count)
        {
            if(count == 4)
            {
                this.NavigationItem.RightBarButtonItem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public async override void ViewWillAppear(bool animated)
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

                    if (StripeCustomerInfo == null)
                    {
                        this.StripeCustomerInfo = await AppDelegate.StripeCustomerInfoFactory.Get(AppDelegate.CurrentUser.UserId);
                    }

                    if (this.StripeCustomerInfo != null)
                    {
                        var paymentMethods = await AppDelegate.CustomerPaymentInfoFactory.GetAllCards(this.StripeCustomerInfo.StripeCustomerId);

                        if (paymentMethods != null)
                        {
                            RemoveAddCard(paymentMethods.Count);
                            if (PaymentMethodsDataSource == null)
                            {
                                PaymentMethodTable.EstimatedRowHeight = 44f;
                                PaymentMethodTable.RowHeight = UITableView.AutomaticDimension;
                                PaymentMethodsDataSource = new PaymentMethodsDataSource(this, paymentMethods);
                                PaymentMethodTable.Source = PaymentMethodsDataSource;
                                PaymentMethodTable.TableFooterView = new UIView();
                            }
                            else
                            {
                                this.PaymentMethodsDataSource.Cards = paymentMethods;
                                PaymentMethodTable.ReloadData();
                            }
                        }
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
        /// <returns></returns>
        private async Task GetPaymentMethods()
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

                    this.StripeCustomerInfo = await AppDelegate.StripeCustomerInfoFactory.Get(AppDelegate.CurrentUser.UserId);

                    if (this.StripeCustomerInfo != null)
                    {
                        var paymentMethods = await AppDelegate.CustomerPaymentInfoFactory.GetAllCards(this.StripeCustomerInfo.StripeCustomerId);

                        if (paymentMethods != null)
                        {
                            RemoveAddCard(paymentMethods.Count);
                            PaymentMethodTable.EstimatedRowHeight = 44f;
                            PaymentMethodTable.RowHeight = UITableView.AutomaticDimension;
                            PaymentMethodsDataSource = new PaymentMethodsDataSource(this, paymentMethods);
                            PaymentMethodTable.Source = PaymentMethodsDataSource;
                            PaymentMethodTable.TableFooterView = new UIView();

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