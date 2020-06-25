using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.Shared.Models.Users;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Resources;
using BigTed;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ResetPasswordController : BaseViewController
    {
        #region Constants, Enums, and Variables

        private VerificationCode myVerificationCode;
        private Users user;

        #endregion

        #region Constructors

        public ResetPasswordController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                VerificationCodeText.Hidden = true;
                VerificationCode.Hidden = true;
                Password.Hidden = true;
                ReEnterPassword.Hidden = true;

                Email.BecomeFirstResponder();
                Email.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    Password.BecomeFirstResponder();

                    return true;
                };

                Password.BecomeFirstResponder();
                Password.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    ReEnterPassword.BecomeFirstResponder();

                    return true;
                };

                ReEnterPassword.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    //Register();

                    return true;
                };

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Continue", UIBarButtonItemStyle.Plain, async (sender, args) =>
                {
                    // stop if no internet connection
                    if (AppDelegate.IsOfflineMode())
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);

                        return;
                    }

                    if (this.NavigationItem.RightBarButtonItem.Title == "Continue")
                    {
                        await VerifyCredentials();
                    }
                    else if (this.NavigationItem.RightBarButtonItem.Title == "Login")
                    {
                        await ResetPassword();
                    }

                }), true);


                //AppDelegate.Track("Reset Password", "View");

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Dismiss the keyboard when one or more fingers touches the screen.
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="evt"></param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            VerificationCode.ResignFirstResponder();
            Email.ResignFirstResponder();
            Password.ResignFirstResponder();
            ReEnterPassword.ResignFirstResponder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task VerifyCredentials()
        {
            if (string.IsNullOrEmpty(Email.Text))
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyEmail, Helpers.ToastTime.ErrorTime);
                return;
            }
            BTProgressHUD.Show("......", -1f, ProgressHUD.MaskType.Black);
            try
            {
                user = await AppDelegate.UsersFactory.GetUser(Email.Text.Trim());

                if (user != null)
                {
                    if (user.AccountLocked)
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.AccountLockedMessage, Helpers.ToastTime.ErrorTime);
                    }
                    else
                    {

                        myVerificationCode = new VerificationCode();
                        myVerificationCode.Email = Email.Text.Trim();
                        myVerificationCode.Code = Shared.Helpers.PasswordHash.GenerateCodeNumber();
                        myVerificationCode.UserId = user.UserId;
                        myVerificationCode.SendCode = true;
                        await AppDelegate.VerificationCodeFactory.AddVerificationCode(myVerificationCode);

                        //SendVerificationCode(user.PhoneNumber.ToString(), myVerificationCode.Code);

                        VerificationCodeText.Hidden = false;
                        Email.Enabled = false;
                        VerificationCode.Hidden = false;
                        Password.Hidden = false;
                        ReEnterPassword.Hidden = false;

                        this.NavigationItem.RightBarButtonItem.Title = "Login";
                    }

                }
                else
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.InvalidEmail, Helpers.ToastTime.ErrorTime);
                }
                BTProgressHUD.Dismiss();
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ResetPassword()
        {
            try
            {
                if (!string.Equals(VerificationCode.Text, myVerificationCode.Code))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.InvalidCode, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (string.IsNullOrEmpty(Password.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyPassword, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (Password.Text.Length <= 8)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.PasswordLength, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (!string.Equals(Password.Text, ReEnterPassword.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.UnequalPassword, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show("......", -1f, ProgressHUD.MaskType.Black);
                    user.PasswordHash = Shared.Helpers.PasswordHash.DoHash(Password.Text);
                    await AppDelegate.UsersFactory.UpdatePassword(user.UserId, user.PasswordHash);
                    //await AppDelegate.UserQueriesManager.EditUser(user); 
                    await AppDelegate.VerificationCodeFactory.DeleteVerificationCode(myVerificationCode.Email, user.UserId);
                    BTProgressHUD.Dismiss();
                    AppDelegate.DeleteCurrentUser();
                    user.PasswordHash = string.Empty;
                    AppDelegate.SaveCurrentUser(user);
                    AppDelegate.CurrentUser = user;

                    if (user.IsIndividual)
                    {
                        UIViewController individualController = this.Storyboard.InstantiateViewController("IndividualHomeController") as IndividualHomeController;
                        this.NavigationController.SetViewControllers(new UIViewController[] { individualController }, true);
                    }
                    else
                    {
                        UIViewController c = this.Storyboard.InstantiateViewController("BusinessHomeController") as BusinessHomeController;
                        this.NavigationController.SetViewControllers(new UIViewController[] { c }, true);
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        #endregion

    }
}