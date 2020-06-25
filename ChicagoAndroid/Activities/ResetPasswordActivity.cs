using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Views.InputMethods;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.Shared.Models.Users;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.Activities
{
    [Activity(Label = "Reset Password", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class ResetPasswordActivity : BaseActivity, TextView.IOnEditorActionListener
    {

        #region Constants, Enums, and Variables

        private EditText email;
        private EditText password;
        private EditText reEnterPassword;
        private TextView continueBtn;
        private EditText verificationCode;
        private TextView verifyAccountText;
        private VerificationCode myVerificationCode;
        private Users user;

        #endregion

        #region Methods

        /// <summary>
        /// handle when user tap done on the keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            try
            {
                e.Handled = false;
                if (e.ActionId == ImeAction.Done)
                {
                    //this hides the keyboard
                    var imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(password.WindowToken, HideSoftInputFlags.NotAlways);

                    // stop if no internet connection
                    if (CheckNetworkConnectivity() == null)
                    {
                        Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Long).Show();
                        return;
                    }

                    if (continueBtn.Text == "Continue")
                    {
                        await VerifyCredentials();
                    }
                    else if (continueBtn.Text == "Login")
                    {
                        await ResetPassword();
                    }
                }
            }
            catch (Exception)
            {
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
            this.Finish();
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

                SetContentView(Resource.Layout.ResetPassword);

                //add the back arrow
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                email = FindViewById<EditText>(Resource.Id.email);
                password = FindViewById<EditText>(Resource.Id.password);
                reEnterPassword = FindViewById<EditText>(Resource.Id.reEnterpassword);
                verificationCode = FindViewById<EditText>(Resource.Id.verificationCode);
                verifyAccountText = FindViewById<TextView>(Resource.Id.verifyAcctText);
                verifyAccountText.Visibility = ViewStates.Gone;
                verificationCode.Visibility = ViewStates.Gone;
                password.Visibility = ViewStates.Gone;
                reEnterPassword.Visibility = ViewStates.Gone;

                // load the toolbar
                continueBtn = FindViewById<TextView>(Resource.Id.resetPassword);
                continueBtn.Text = "Continue";

                //Runtime permissions are an Android Marshmallow feature. This means that if the user is on an 
                //older version of Android, there’s no need to call any of the new runtime permission workflows. 
                //It’s simple to check the SDK version before making the call to an API by using the Build.Version.SdkInt enum.                  
                if ((int)Build.VERSION.SdkInt >= (int)BuildVersionCodes.M)
                {
                    CheckPermission();
                }
                if (permissionsToRequest.Any())
                {
                    this.waitingForPermission = Helpers.PlatformChecks.RequestPermissions(this, permissionsToRequest.ToArray(), 101);
                }

                continueBtn.Click += async delegate
                {
                //this hides the keyboard
                var imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(password.WindowToken, HideSoftInputFlags.NotAlways);

                // stop if no internet connection
                if (CheckNetworkConnectivity() == null)
                    {
                        Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Long).Show();
                        return;
                    }

                    if (continueBtn.Text == "Continue")
                    {
                        await VerifyCredentials();
                    }
                    else if (continueBtn.Text == "Login")
                    {
                        await ResetPassword();
                    }

                };

                //App.Track("Forgot Password", "View");
            }
            catch (Exception)
            {
            }

        }

        private async Task VerifyCredentials()
        {
            if (string.IsNullOrEmpty(email.Text))
            {
                Toast.MakeText(this, ToastMessage.EmptyEmail, ToastLength.Long).Show();
                return;
            }
            this.ShowProgressbar(true, "", "");
            try
            {
                user = await App.UsersFactory.GetUser(email.Text.Trim());

                if (user != null)
                {
                    if (user.AccountLocked)
                    {
                        Toast.MakeText(this, ToastMessage.AccountLockedMessage, ToastLength.Long).Show();
                    }
                    else
                    {

                        myVerificationCode = new VerificationCode();
                        myVerificationCode.Email = email.Text.Trim();
                        myVerificationCode.Code = Shared.Helpers.PasswordHash.GenerateCodeNumber();
                        myVerificationCode.UserId = user.UserId;
                        myVerificationCode.SendCode = true;
                        await App.VerificationCodeFactory.AddVerificationCode(myVerificationCode);

                        //SendVerificationCode(user.PhoneNumber.ToString(), myVerificationCode.Code);

                        verifyAccountText.Visibility = ViewStates.Visible;
                        password.Visibility = ViewStates.Visible;
                        email.Visibility = ViewStates.Gone;
                        reEnterPassword.Visibility = ViewStates.Visible;
                        verificationCode.Visibility = ViewStates.Visible;
                        continueBtn.Text = "Login";
                    }

                }
                else
                {
                    Toast.MakeText(this, ToastMessage.InvalidEmail, ToastLength.Short).Show();
                }
                this.ShowProgressbar(false, "", ToastMessage.LoggingIn);
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.LoggingIn);
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
                if (!string.Equals(verificationCode.Text, myVerificationCode.Code))
                {
                    Toast.MakeText(this, ToastMessage.InvalidCode, ToastLength.Short).Show();
                    return;
                }
                if (string.IsNullOrEmpty(password.Text))
                {
                    Toast.MakeText(this, ToastMessage.EmptyPassword, ToastLength.Short).Show();
                    return;
                }
                else if (password.Text.Length <= 8)
                {
                    Toast.MakeText(this, ToastMessage.PasswordLength, ToastLength.Short).Show();
                    return;
                }
                if (!string.Equals(password.Text, reEnterPassword.Text))
                {
                    Toast.MakeText(this, ToastMessage.UnequalPassword, ToastLength.Short).Show();
                    return;
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.LoggingIn);
                    user.PasswordHash = Shared.Helpers.PasswordHash.DoHash(password.Text);
                    await App.UsersFactory.UpdatePassword(user.UserId, user.PasswordHash);
                    //userService.UpdateUser(user);
                    await App.VerificationCodeFactory.DeleteVerificationCode(myVerificationCode.Email, user.UserId);
                    this.ShowProgressbar(false, "", ToastMessage.LoggingIn);

                    DeleteCredentials();
                    user.PasswordHash = string.Empty;
                    SaveCrendentials(user);
                    //SaveEnvironment(Shared.MyEnvironment.Environment);

                    if (user.IsIndividual)
                    {
                        StartActivity(typeof(Individuals.IndividualHomeActivity));
                    }
                    else
                    {
                        StartActivity(typeof(Businesses.BusinessHomeActivity));
                    }

                    //App.Track("Forgot Password", "Reset");

                    this.Finish();
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.LoggingIn);
            }
        }

        #endregion

    }
}