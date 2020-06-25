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
    [Register ("SignUpController")]
    partial class SignUpController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView CreateAcctTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PrivacyPolicyLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TermsOfUseLbl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CreateAcctTable != null) {
                CreateAcctTable.Dispose ();
                CreateAcctTable = null;
            }

            if (PrivacyPolicyLbl != null) {
                PrivacyPolicyLbl.Dispose ();
                PrivacyPolicyLbl = null;
            }

            if (TermsOfUseLbl != null) {
                TermsOfUseLbl.Dispose ();
                TermsOfUseLbl = null;
            }
        }
    }
}