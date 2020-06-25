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
    [Register ("CheckInController")]
    partial class CheckInController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CheckInBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView CheckInImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CheckInNoteBtn { get; set; }

        [Action ("CheckInBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CheckInBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("CheckInNoteBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CheckInNoteBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CheckInBtn != null) {
                CheckInBtn.Dispose ();
                CheckInBtn = null;
            }

            if (CheckInImage != null) {
                CheckInImage.Dispose ();
                CheckInImage = null;
            }

            if (CheckInNoteBtn != null) {
                CheckInNoteBtn.Dispose ();
                CheckInNoteBtn = null;
            }
        }
    }
}