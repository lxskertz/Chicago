using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class PrivacyPolicyController : BaseViewController
    {

        #region Constructors

        public PrivacyPolicyController(IntPtr handle) : base(handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            string fileName = "privacypolicy.txt";
            var a = NSBundle.MainBundle.BundlePath;
            string localDocUrl = System.IO.Path.Combine(a, fileName);
            Webview.LoadRequest(new NSUrlRequest(new NSUrl(localDocUrl, false)));
            Webview.ScalesPageToFit = true;
        }

        public override void ViewWillAppear(bool animated)
        {
            this.NavigationController.SetNavigationBarHidden(false, false);
        }

        #endregion

    }
}