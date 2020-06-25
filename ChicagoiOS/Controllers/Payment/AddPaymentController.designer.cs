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
    [Register ("AddPaymentController")]
    partial class AddPaymentController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField CardNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField CvcCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ExpDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ZipCode { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CardNumber != null) {
                CardNumber.Dispose ();
                CardNumber = null;
            }

            if (CvcCode != null) {
                CvcCode.Dispose ();
                CvcCode = null;
            }

            if (ExpDate != null) {
                ExpDate.Dispose ();
                ExpDate = null;
            }

            if (ZipCode != null) {
                ZipCode.Dispose ();
                ZipCode = null;
            }
        }
    }
}