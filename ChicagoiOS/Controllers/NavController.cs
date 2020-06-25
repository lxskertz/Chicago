using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class NavController : UINavigationController
    {

        #region Constructors

        public NavController() : base() { }
        public NavController(IntPtr handle) : base(handle) { }
        public NavController(UIViewController rootViewController) : base(rootViewController) { }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides the preferred status bar style
        /// </summary>
        /// <returns>The status bar style.</returns>
        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }

        /// <summary>
        /// Gets the supported interface orientations.
        /// </summary>
        /// <returns>The supported interface orientations.</returns>
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if (this.TopViewController == null)
            {
                return base.GetSupportedInterfaceOrientations();
            }

            return this.TopViewController.GetSupportedInterfaceOrientations();
        }

        #endregion

    }
}