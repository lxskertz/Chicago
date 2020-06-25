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
    [Register ("BlockedUserCell")]
    partial class BlockedUserCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Name { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UnblockBtn { get; set; }

        [Action ("UnblockBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UnblockBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Name != null) {
                Name.Dispose ();
                Name = null;
            }

            if (UnblockBtn != null) {
                UnblockBtn.Dispose ();
                UnblockBtn = null;
            }
        }
    }
}