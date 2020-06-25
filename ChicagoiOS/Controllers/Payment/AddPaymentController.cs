using Foundation;
using System;
using System.Threading.Tasks;
using System.Linq;
using UIKit;
using Stripe;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Payment;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class AddPaymentController : BaseViewController
    {

        #region Constructors

        public AddPaymentController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dismiss the keyboard when one or more fingers touches the screen.
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="evt"></param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            CardNumber.ResignFirstResponder();
            ZipCode.ResignFirstResponder();
            CvcCode.ResignFirstResponder();
            ExpDate.ResignFirstResponder();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(AppText.Add, UIBarButtonItemStyle.Plain, (sender, args) =>
                {
                    AddCard();
                }), true);


                CardNumber.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    ExpDate.BecomeFirstResponder();

                    return true;
                };
                ExpDate.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    CvcCode.BecomeFirstResponder();

                    return true;
                };
                CvcCode.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    ZipCode.BecomeFirstResponder();

                    return true;
                };

                ZipCode.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    AddCard();

                    return true;
                };

                bool delTapped = false;

                ExpDate.ShouldChangeCharacters = (textField, range, replacementString) => {
                    var newLength = textField.Text.Length + replacementString.Length - range.Length;

                    if (ExpDate.Text.Length > 0 && replacementString.Length == 0)
                    {
                        delTapped = true;
                    }
                    else
                    {
                        delTapped = false;
                    }

                    if (ExpDate.Text.Length == 2 && !ExpDate.Text.Contains("/") && !delTapped)
                    {
                        var newText = ExpDate.Text + "/";
                        ExpDate.Text = newText;
                    }


                    return newLength <= 5;
                };

                CvcCode.ShouldChangeCharacters = (textField, range, replacementString) => {
                    var newLength = textField.Text.Length + replacementString.Length - range.Length;
                    return newLength <= 3;
                };

                ZipCode.ShouldChangeCharacters = (textField, range, replacementString) => {
                    var newLength = textField.Text.Length + replacementString.Length - range.Length;
                    return newLength <= 6;
                };

                CardNumber.ShouldChangeCharacters = (textField, range, replacementString) => {
                    var newLength = textField.Text.Length + replacementString.Length - range.Length;
                    return newLength <= 19;
                };
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async void AddCard()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);

                    return;
                }

                BTProgressHUD.Show(ToastMessage.Processing, -1f, ProgressHUD.MaskType.Black);

                if (string.IsNullOrEmpty(CardNumber.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyCardNumber, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(ExpDate.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyExpDate, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(CvcCode.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyCVV, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(ZipCode.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyZipCode, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {            
                    CustomerPaymentInfo paymentInfo = new CustomerPaymentInfo();
                    paymentInfo.Number = CardNumber.Text;
                    paymentInfo.ExpirationMonth = string.IsNullOrEmpty(ExpDate.Text) ? 0 : Convert.ToInt32(ExpDate.Text.Substring(0, 2));
                    var yearSub = string.IsNullOrEmpty(ExpDate.Text) ? 0 : Convert.ToInt32(ExpDate.Text.Substring(3, 2));
                    paymentInfo.ExpirationYear = Convert.ToInt32("20" + yearSub);
                    paymentInfo.UserId = AppDelegate.CurrentUser.UserId;
                    paymentInfo.Email = AppDelegate.CurrentUser.Email;
                    paymentInfo.Cvc = CvcCode.Text;

                    var stripeCustomerInfo = await AppDelegate.StripeCustomerInfoFactory.Get(AppDelegate.CurrentUser.UserId);

                    if(stripeCustomerInfo == null)
                    {
                        await AppDelegate.CustomerPaymentInfoFactory.CreateCustomerPaymentInfo(paymentInfo);
                    }
                    else
                    {
                        await AppDelegate.CustomerPaymentInfoFactory.AddCard(paymentInfo, stripeCustomerInfo.StripeCustomerId);
                    }

                    BTProgressHUD.Dismiss();
                    PaymentMethodController.RequiresRefresh = true;
                    this.NavigationController.PopViewController(true);

                }
            }
            catch (StripeException ex)
            {
                var a = ex;
                BTProgressHUD.ShowErrorWithStatus(ex.Message, Helpers.ToastTime.SuccessTime);
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, 3000);
                //BTProgressHUD.Dismiss();
            }
        }

        #endregion

    }
}