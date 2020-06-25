using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;

namespace Tabs.Mobile.ChicagoAndroid.Activities
{
    [Activity(Label = "Privacy Policy", Theme = "@style/AppTheme", NoHistory = true)]
    public class PrivacyPolicyActivity : BaseActivity
    {

        #region Methods 

        /// <summary>
        /// Called when the activity is starting, create the UI
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MyWebviewLayout);
            this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SupportActionBar.SetDisplayShowHomeEnabled(true);
            var webView = FindViewById<WebView>(Resource.Id.webView);
            webView.SetWebViewClient(new WebViewClient());
            webView.Settings.JavaScriptEnabled = true;
            webView.LoadUrl("file:///android_asset/privacypolicy.txt");
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

        #endregion

    }
}