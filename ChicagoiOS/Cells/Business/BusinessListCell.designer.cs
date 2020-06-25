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
    [Register ("BusinessListCell")]
    partial class BusinessListCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView BusinessLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BusinessName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BusinessType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ChkInBtn { get; set; }

        [Action ("ChkInBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ChkInBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BusinessLogo != null) {
                BusinessLogo.Dispose ();
                BusinessLogo = null;
            }

            if (BusinessName != null) {
                BusinessName.Dispose ();
                BusinessName = null;
            }

            if (BusinessType != null) {
                BusinessType.Dispose ();
                BusinessType = null;
            }

            if (ChkInBtn != null) {
                ChkInBtn.Dispose ();
                ChkInBtn = null;
            }
        }
    }
}