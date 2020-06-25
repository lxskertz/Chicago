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
    [Register ("ToastersSearchViewController")]
    partial class ToastersSearchViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ToastersSearchTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ToastersSearchTable != null) {
                ToastersSearchTable.Dispose ();
                ToastersSearchTable = null;
            }
        }
    }
}