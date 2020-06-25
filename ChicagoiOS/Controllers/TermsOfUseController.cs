using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class TermsOfUseController : BaseViewController
    {

        #region Constructors

        public TermsOfUseController(IntPtr handle) : base(handle)
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

            string fileName = "termsofuse.txt";
            var a = NSBundle.MainBundle.BundlePath;
            string localDocUrl = System.IO.Path.Combine(a, fileName);
            webview.LoadRequest(new NSUrlRequest(new NSUrl(localDocUrl, false)));
            webview.ScalesPageToFit = true;
        }

        public override void ViewWillAppear(bool animated)
        {
            this.NavigationController.SetNavigationBarHidden(false, false);
        }

        #endregion

    }
}