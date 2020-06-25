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
    [Register ("EventInfoController")]
    partial class EventInfoController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem AreYouGoing { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView EventInfoTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIToolbar EventInfoToolbar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView EVentLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EventOwnerName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EventTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem No { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem Yes { get; set; }

        [Action ("No_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void No_Activated (UIKit.UIBarButtonItem sender);

        [Action ("Yes_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Yes_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (AreYouGoing != null) {
                AreYouGoing.Dispose ();
                AreYouGoing = null;
            }

            if (EventInfoTable != null) {
                EventInfoTable.Dispose ();
                EventInfoTable = null;
            }

            if (EventInfoToolbar != null) {
                EventInfoToolbar.Dispose ();
                EventInfoToolbar = null;
            }

            if (EVentLogo != null) {
                EVentLogo.Dispose ();
                EVentLogo = null;
            }

            if (EventOwnerName != null) {
                EventOwnerName.Dispose ();
                EventOwnerName = null;
            }

            if (EventTitle != null) {
                EventTitle.Dispose ();
                EventTitle = null;
            }

            if (No != null) {
                No.Dispose ();
                No = null;
            }

            if (Yes != null) {
                Yes.Dispose ();
                Yes = null;
            }
        }
    }
}