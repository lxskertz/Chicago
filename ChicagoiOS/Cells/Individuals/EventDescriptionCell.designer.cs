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
    [Register ("EventDescriptionCell")]
    partial class EventDescriptionCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EventDescription { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (EventDescription != null) {
                EventDescription.Dispose ();
                EventDescription = null;
            }
        }
    }
}