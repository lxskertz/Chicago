using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Http;
using Android.OS;
using Android.App;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Net;
using Android.Content.PM;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Gms.Common;


namespace Tabs.Mobile.ChicagoAndroid.Activities
{
    [Activity(Theme = "@style/AppTheme")]
    public class HomeActivity : BaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                RequestedOrientation = ScreenOrientation.Portrait;
                this.SupportActionBar.Hide();

                SetContentView(Resource.Layout.Home);

                var loginBtn = FindViewById<TextView>(Resource.Id.loginBtn);
                var signUpBtn = FindViewById<TextView>(Resource.Id.signupBtn);


                signUpBtn.Click += delegate
                {
                    StartActivity(typeof(SignUpActivity));
                };

                loginBtn.Click += delegate
                {
                    StartActivity(typeof(LoginActivity));
                };

            }
            catch (Exception) { }

        }
    }
}