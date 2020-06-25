using Foundation;
using System;
using UIKit;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class IndividualHomeController : UITabBarController
    {
        public IndividualHomeController (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
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
                //this.EdgesForExtendedLayout = UIRectEdge.None;

                DetermineIfAccountIsLocked();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        private async void Logout()
        {
            try
            {

                if (!AppDelegate.IsOfflineMode())
                {
                    await AppDelegate.UsersFactory.Logout(AppDelegate.CurrentUser.Email);
                    //if (!string.IsNullOrEmpty(AppDelegate.DeviceRegistrationId()))
                    //{
                    //    await AppDelegate.NotificationRegisterManager.Delete(AppDelegate.DeviceRegistrationId());
                    //}
                }
                AppDelegate.DeleteSettings();
                UIViewController login = this.Storyboard.InstantiateViewController("HomeController") as HomeController;
                this.NavigationController.SetViewControllers(new UIViewController[] { login }, true);

            }
            catch (Exception) {
                UIViewController login = this.Storyboard.InstantiateViewController("HomeController") as HomeController;
                this.NavigationController.SetViewControllers(new UIViewController[] { login }, true);
            }
        }

        private async void DetermineIfAccountIsLocked()
        {
            var user = await AppDelegate.UsersFactory.GetUser(AppDelegate.CurrentUser.UserId);
            if(user != null && user.AccountLocked)
            {

                UIAlertController uIAlertController = new UIAlertController();
                uIAlertController = UIAlertController.Create("", ToastMessage.AccountLockedMessage, UIAlertControllerStyle.Alert);
                uIAlertController.AddAction(UIAlertAction.Create(AppText.Ok, UIAlertActionStyle.Cancel, (action) => Logout()));
                this.PresentViewController(uIAlertController, true, null);
            }

        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //try
            //{
            //    //AppDelegate.CurrentAddy = new Plugin.Geolocator.Abstractions.Address();
            //    //AppDelegate.CurrentAddy.Locality = AppDelegate.City;
            //    //AppDelegate.CurrentAddy.PostalCode = AppDelegate.ZipCode;

            //    if (AppDelegate.CurrentAddy == null)
            //    {
            //        AppDelegate.CurrentAddy = await new LocationHelper().GetAddress();
            //    }
            //    if (AppDelegate.CurrentAddy != null)
            //    {
            //        AppDelegate.City = string.IsNullOrEmpty(AppDelegate.CurrentAddy.Locality) ? "" : AppDelegate.CurrentAddy.Locality;
            //        AppDelegate.ZipCode = string.IsNullOrEmpty(AppDelegate.CurrentAddy.PostalCode) ? "" : AppDelegate.CurrentAddy.PostalCode;
            //        //state = string.IsNullOrEmpty(currentAddy.SubLocality) ? "" : currentAddy.AdminArea;
            //    }
            //}
            //catch (Exception)
            //{

            //}
        }

    }
}