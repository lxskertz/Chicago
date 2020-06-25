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
    [Register ("ResetPasswordController")]
    partial class ResetPasswordController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Email { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Password { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ReEnterPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField VerificationCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel VerificationCodeText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Email != null) {
                Email.Dispose ();
                Email = null;
            }

            if (Password != null) {
                Password.Dispose ();
                Password = null;
            }

            if (ReEnterPassword != null) {
                ReEnterPassword.Dispose ();
                ReEnterPassword = null;
            }

            if (VerificationCode != null) {
                VerificationCode.Dispose ();
                VerificationCode = null;
            }

            if (VerificationCodeText != null) {
                VerificationCodeText.Dispose ();
                VerificationCodeText = null;
            }
        }
    }
}