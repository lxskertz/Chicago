using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class HomeController : UIViewController
    {
        public HomeController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SignupBtn.Layer.CornerRadius = 4;
            SignupBtn.Layer.BorderWidth = 1;
            SignupBtn.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;

            LoginBtn.Layer.CornerRadius = 4;
            LoginBtn.Layer.BorderWidth = 1;
            LoginBtn.Layer.BorderColor = UIColor.White.CGColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationItem.HidesBackButton = true;
            this.NavigationController.NavigationBarHidden = true;
        }

        partial void LoginBtn_TouchUpInside(UIButton sender)
        {
            UIViewController login = this.Storyboard.InstantiateViewController("LoginViewController") as LoginViewController;
            this.NavigationController.PushViewController(login, true);
        }

        partial void SignupBtn_TouchUpInside(UIButton sender)
        {
            UIViewController signUp = this.Storyboard.InstantiateViewController("SignUpController") as SignUpController;
            this.NavigationController.PushViewController(signUp, true);
        }
    }
}