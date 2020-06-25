using Foundation;
using System;
using UIKit;
using Autofac;
using Tabs.Mobile.Shared;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class RootViewController : UIViewController
    {

        #region Constructors

        public RootViewController() : base() { }

        public RootViewController(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides ViewDidLoad
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AppStart.Init();
            using (var scope = AppStart.AutoFacContainer.BeginLifetimeScope())
            {
                AppDelegate.BusinessAccountFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Accounts.BusinessAccountFactory>();
                AppDelegate.BusinessFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Businesses.BusinessFactory>();
                AppDelegate.IndividualFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Individuals.IndividualFactory>();
                AppDelegate.UsersFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Users.UsersFactory>();
                AppDelegate.VerificationCodeFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.VerificationCodeFactory>();
                AppDelegate.ToastersFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Individuals.ToastersFactory>();
                AppDelegate.AddressFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Businesses.AddressFactory>();
                AppDelegate.BusinessTypesFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Businesses.BusinessTypesFactory>();
                AppDelegate.BusinessEventsFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Events.BusinessEventsFactory>();
                AppDelegate.RsvpFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Events.RsvpFactory>();
                AppDelegate.EventLikesFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Events.EventLikesFactory>();
                AppDelegate.CheckInFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.CheckIns.CheckInFactory>();
                AppDelegate.CheckInLikesFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.CheckIns.CheckInLikesFactory>();
                AppDelegate.BusinessDrinkFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Drinks.BusinessDrinkFactory>();
                AppDelegate.StripeCustomerInfoFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Payments.StripeCustomerInfoFactory>();
                AppDelegate.CustomerPaymentInfoFactory = AppStart.AutoFacContainer.Resolve<Shared.Interfaces.Payments.ICustomerPaymentInfoFactory>();
                AppDelegate.ToasterOrderFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Orders.ToasterOrderFactory>();
                AppDelegate.BusinessPhotoFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Businesses.BusinessPhotoFactory>();
                AppDelegate.ToasterPhotoFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Individuals.ToasterPhotoFactory>();
                AppDelegate.ToasterPointsFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Points.ToasterPointsFactory>();
                AppDelegate.SMSMessageFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Individuals.SMSMessageFactory>();
                AppDelegate.NotificationRegisterFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.NotificationRegisterFactory>();
                AppDelegate.ReportedSpamCheckInFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Reports.Spams.ReportedSpamCheckInFactory>();
                AppDelegate.InappropriateReportCheckInFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Reports.InappropriateReports.InappropriateReportCheckInFactory>();
                AppDelegate.ReportedUserFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Reports.Users.ReportedUserFactory>();
            }

            if (AppDelegate.CurrentUser != null)
            {
                if (AppDelegate.CurrentUser.IsIndividual)
                {
                    UIViewController individualController = this.Storyboard.InstantiateViewController("IndividualHomeController") as IndividualHomeController;
                    this.NavigationController.SetViewControllers(new UIViewController[] { individualController }, true);
                } else
                {
                    UIViewController c = this.Storyboard.InstantiateViewController("BusinessHomeController") as BusinessHomeController;
                    this.NavigationController.SetViewControllers(new UIViewController[] { c }, true);
                }
            }
            else
            {
                UIViewController login = this.Storyboard.InstantiateViewController("HomeController") as HomeController;
                this.NavigationController.SetViewControllers(new UIViewController[] { login }, true);
            }

        }

        /// <summary>
        /// Overrides ViewWillAppear
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated) { }

        /// <summary>
        /// Overrides the status bar style
        /// </summary>
        /// <returns>The status bar style.</returns>
        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            if (AppDelegate.NavController == null || AppDelegate.NavController.TopViewController == null)
            {
                return UIStatusBarStyle.LightContent;
            }
            return AppDelegate.NavController.TopViewController.PreferredStatusBarStyle();
        }

        /// <summary>
        /// Overrides whether the status bar should be hidden
        /// </summary>
        /// <returns><c>true</c>, if status bar hidden was prefersed, <c>false</c> otherwise.</returns>
        public override bool PrefersStatusBarHidden()
        {
            return false;
        }

        /// <summary>
        /// Gets the supported interface orientations.
        /// </summary>
        /// <returns>The supported interface orientations.</returns>
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if (AppDelegate.NavController == null || AppDelegate.NavController.TopViewController == null)
            {
                return base.GetSupportedInterfaceOrientations();
            }
            return AppDelegate.NavController.TopViewController.GetSupportedInterfaceOrientations();
        }


        #endregion

    }
}