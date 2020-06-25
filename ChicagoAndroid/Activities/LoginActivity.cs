using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Support.Design.Widget;
using Android.Widget;
using Tabs.Mobile.Shared.Resources;
using Android.Text;

namespace Tabs.Mobile.ChicagoAndroid.Activities
{
    [Activity(Label = "Login", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class LoginActivity : BaseActivity, TextView.IOnEditorActionListener
    {

        #region Constants, Enums, and variables

        FrameLayout loginLayout;
        TextInputLayout emailLayout;
        TextInputEditText email;
        TextInputLayout passwordLayout;
        TextInputEditText password;

        #endregion

        #region Methods

        /// <summary>
        /// handle when user tap done on the keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == ImeAction.Done)
            {
                //await Login();
            }
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
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                //Window.RequestFeature(WindowFeatures.ActionBar);
                //SupportActionBar.Hide();

                base.OnCreate(savedInstanceState);
                RequestedOrientation = ScreenOrientation.Portrait;
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                SetContentView(Resource.Layout.Login);
                loginLayout = FindViewById<FrameLayout>(Resource.Id.loginLayout);
                emailLayout = FindViewById<TextInputLayout>(Resource.Id.email_layout);
                email = FindViewById<TextInputEditText>(Resource.Id.email);
                passwordLayout = FindViewById<TextInputLayout>(Resource.Id.password_layout);
                password = FindViewById<TextInputEditText>(Resource.Id.password_edittext);

                var loginBtn = FindViewById<TextView>(Resource.Id.login);
                var forgotPassword = FindViewById<TextView>(Resource.Id.forgotPassword);

                forgotPassword.Click += delegate
                {
                    StartActivity(typeof(ResetPasswordActivity));
                };

                loginBtn.Click += async delegate
                {
                    await Login();
                };

                email.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(256) });
                password.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(50) });

            }
            catch (Exception) { }
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            try
            {
                // stop if no internet connection
                if (CheckNetworkConnectivity() == null)
                {
                    ShowSnack(loginLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                if (!ValidateInput(emailLayout, email, ToastMessage.EmptyEmail))
                {
                    return;
                }
                if (!ValidateInput(passwordLayout, password, ToastMessage.EmptyPassword))
                {
                    return;
                }
                this.ShowProgressbar(true, "", ToastMessage.LoggingIn);

                //this hides the keyboard
                var imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(password.WindowToken, HideSoftInputFlags.NotAlways);

                var user = await App.UsersFactory.Login(email.Text.Trim(), password.Text.Trim());

                if (user != null)
                {
                    if (user.AccountLocked)
                    {
                        ShowSnack(loginLayout, ToastMessage.AccountLockedMessage, "OK");
                    }
                    else
                    {
                        DeleteCredentials();
                        user.PasswordHash = string.Empty;
                        SaveCrendentials(user);
                        this.MyPreferences.SaveEnvironment(Shared.MyEnvironment.Environment);

                        if (user.IsIndividual)
                        {
                            StartActivity(typeof(Individuals.IndividualHomeActivity));
                        }
                        else
                        {
                            StartActivity(typeof(Businesses.BusinessHomeActivity));
                        }

                        this.Finish();
                    }
                }
                else
                {
                    ShowSnack(loginLayout, ToastMessage.InValidUsernameOrPassword, "OK");
                }

                //App.Track("Login", "Login");
                this.ShowProgressbar(false, "", ToastMessage.LoggingIn);
            }
            catch (Exception ex)
            {
                var a = ex;
                Toast.MakeText(this, ToastMessage.ServerError, ToastLength.Short).Show();
                this.ShowProgressbar(false, "", ToastMessage.LoggingIn);
            }
        }

        #endregion

    }
}