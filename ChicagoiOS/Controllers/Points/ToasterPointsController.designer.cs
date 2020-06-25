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
    [Register ("ToasterPointsController")]
    partial class ToasterPointsController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PointAmt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView PointsTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UsageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (PointAmt != null) {
                PointAmt.Dispose ();
                PointAmt = null;
            }

            if (PointsTable != null) {
                PointsTable.Dispose ();
                PointsTable = null;
            }

            if (UsageView != null) {
                UsageView.Dispose ();
                UsageView = null;
            }
        }
    }
}