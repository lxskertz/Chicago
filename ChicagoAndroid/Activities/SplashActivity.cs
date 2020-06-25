using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Preferences;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Users;

namespace Tabs.Mobile.ChicagoAndroid.Activities
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class SplashActivity : Activity
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                Thread.Sleep(1000);
                LoadScreen();               
            }catch(Exception ex)
            {
                var a = ex;
            }
        }

        /// Take user to homescreen if they were not logged in
        /// </summary>
        private void LoadScreen()
        {
            var user = GetCurrentUser();

            if(user != null)
            {
                if (user.IsIndividual)
                {
                    StartActivity(typeof(Individuals.IndividualHomeActivity));
                }
                else
                {
                    StartActivity(typeof(Businesses.BusinessHomeActivity));
                }
            } else
            {
                var intent = new Intent(this, typeof(HomeActivity));
                StartActivity(intent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Users GetCurrentUser()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            return JsonConvert.DeserializeObject<Users>(prefs.GetString(BaseActivity.currentUserPref, ""));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetEnvironment()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            return prefs.GetString(BaseActivity.environment, "");
        }

        #endregion

    }
}