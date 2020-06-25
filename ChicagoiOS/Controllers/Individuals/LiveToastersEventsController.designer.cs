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
    [Register ("LiveToastersEventsController")]
    partial class LiveToastersEventsController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoResultMessage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl ToasterEventSegmentCtrl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ToastersLiveTable { get; set; }

        [Action ("ToasterEventSegmentCtrl_ValueChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ToasterEventSegmentCtrl_ValueChanged (UIKit.UISegmentedControl sender);

        void ReleaseDesignerOutlets ()
        {
            if (NoResultMessage != null) {
                NoResultMessage.Dispose ();
                NoResultMessage = null;
            }

            if (ToasterEventSegmentCtrl != null) {
                ToasterEventSegmentCtrl.Dispose ();
                ToasterEventSegmentCtrl = null;
            }

            if (ToastersLiveTable != null) {
                ToastersLiveTable.Dispose ();
                ToastersLiveTable = null;
            }
        }
    }
}