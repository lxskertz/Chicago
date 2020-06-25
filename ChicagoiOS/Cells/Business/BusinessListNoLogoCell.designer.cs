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
    [Register ("BusinessListNoLogoCell")]
    partial class BusinessListNoLogoCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BusinessName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BusinessType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CheckInBtn { get; set; }

        [Action ("CheckInBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CheckInBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BusinessName != null) {
                BusinessName.Dispose ();
                BusinessName = null;
            }

            if (BusinessType != null) {
                BusinessType.Dispose ();
                BusinessType = null;
            }

            if (CheckInBtn != null) {
                CheckInBtn.Dispose ();
                CheckInBtn = null;
            }
        }
    }
}