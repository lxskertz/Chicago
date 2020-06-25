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
    [Register ("SendDrinkController")]
    partial class SendDrinkController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BusinessAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BusinessInfoView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BusinessName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView SendDrinkTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BusinessAddress != null) {
                BusinessAddress.Dispose ();
                BusinessAddress = null;
            }

            if (BusinessInfoView != null) {
                BusinessInfoView.Dispose ();
                BusinessInfoView = null;
            }

            if (BusinessName != null) {
                BusinessName.Dispose ();
                BusinessName = null;
            }

            if (SendDrinkTable != null) {
                SendDrinkTable.Dispose ();
                SendDrinkTable = null;
            }
        }
    }
}