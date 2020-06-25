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
    [Register ("BusinessProfileController")]
    partial class BusinessProfileController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView BusinessProfileTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProfilePicture { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ProfilePicView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BusinessProfileTable != null) {
                BusinessProfileTable.Dispose ();
                BusinessProfileTable = null;
            }

            if (ProfilePicture != null) {
                ProfilePicture.Dispose ();
                ProfilePicture = null;
            }

            if (ProfilePicView != null) {
                ProfilePicView.Dispose ();
                ProfilePicView = null;
            }
        }
    }
}