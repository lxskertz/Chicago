// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    [Register ("LoginViewController")]
    partial class LoginViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Email { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ForgotPasswordBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LoginBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Password { get; set; }

        [Action ("ForgotPasswordBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ForgotPasswordBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("LoginBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LoginBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Email != null) {
                Email.Dispose ();
                Email = null;
            }

            if (ForgotPasswordBtn != null) {
                ForgotPasswordBtn.Dispose ();
                ForgotPasswordBtn = null;
            }

            if (LoginBtn != null) {
                LoginBtn.Dispose ();
                LoginBtn = null;
            }

            if (Password != null) {
                Password.Dispose ();
                Password = null;
            }
        }
    }
}