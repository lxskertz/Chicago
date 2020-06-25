using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessHomeController : UITabBarController
    {
        public BusinessHomeController (IntPtr handle) : base (handle)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                this.NavigationController.NavigationBarHidden = false;
                this.NavigationItem.HidesBackButton = false;

                this.NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(145, 200, 244);
                this.NavigationController.NavigationBar.TintColor = UIColor.White;
                this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
                {
                    ForegroundColor = UIColor.White
                };

                this.EdgesForExtendedLayout = UIRectEdge.None;

                var stripeKey = await AppDelegate.UsersFactory.GetStripeKey();
                AppDelegate.CustomerPaymentInfoFactory.Initialize(stripeKey);
                var storageKey = await AppDelegate.UsersFactory.GetStorageConnectionKey();
                Shared.Helpers.BlobStorageHelper.ConnectionString = storageKey;

            }
            catch (Exception)
            {
            }

        }
    }
}