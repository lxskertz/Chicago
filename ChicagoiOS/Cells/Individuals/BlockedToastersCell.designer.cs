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
    [Register ("BlockedToastersCell")]
    partial class BlockedToastersCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _ToasterName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UnBlockBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_ToasterName != null) {
                _ToasterName.Dispose ();
                _ToasterName = null;
            }

            if (UnBlockBtn != null) {
                UnBlockBtn.Dispose ();
                UnBlockBtn = null;
            }
        }
    }
}