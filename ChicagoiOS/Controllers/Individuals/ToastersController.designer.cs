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
    [Register ("ToastersController")]
    partial class ToastersController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton InviteContact { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ToastersTable { get; set; }

        [Action ("InviteContact_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void InviteContact_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (InviteContact != null) {
                InviteContact.Dispose ();
                InviteContact = null;
            }

            if (ToastersTable != null) {
                ToastersTable.Dispose ();
                ToastersTable = null;
            }
        }
    }
}