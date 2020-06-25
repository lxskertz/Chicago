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
    [Register ("EditToasterProfileController")]
    partial class EditToasterProfileController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton EditPicBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView EditToasterProfileTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProfilePicture { get; set; }

        [Action ("EditPicBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void EditPicBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (EditPicBtn != null) {
                EditPicBtn.Dispose ();
                EditPicBtn = null;
            }

            if (EditToasterProfileTable != null) {
                EditToasterProfileTable.Dispose ();
                EditToasterProfileTable = null;
            }

            if (ProfilePicture != null) {
                ProfilePicture.Dispose ();
                ProfilePicture = null;
            }
        }
    }
}