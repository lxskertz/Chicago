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
    [Register ("BusinessListController")]
    partial class BusinessListController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView BusinessesTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BusinessesTable != null) {
                BusinessesTable.Dispose ();
                BusinessesTable = null;
            }
        }
    }
}