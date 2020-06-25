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
    [Register ("ToastersEventController")]
    partial class ToastersEventController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoResultMessage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ToastersEventsTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (NoResultMessage != null) {
                NoResultMessage.Dispose ();
                NoResultMessage = null;
            }

            if (ToastersEventsTable != null) {
                ToastersEventsTable.Dispose ();
                ToastersEventsTable = null;
            }
        }
    }
}