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
    [Register ("ToastersEventsCell")]
    partial class ToastersEventsCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EventDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EventLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView EventLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EventTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LIkeBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LikeCount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ShareBtn { get; set; }

        [Action ("LIkeBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LIkeBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (EventDate != null) {
                EventDate.Dispose ();
                EventDate = null;
            }

            if (EventLocation != null) {
                EventLocation.Dispose ();
                EventLocation = null;
            }

            if (EventLogo != null) {
                EventLogo.Dispose ();
                EventLogo = null;
            }

            if (EventTitle != null) {
                EventTitle.Dispose ();
                EventTitle = null;
            }

            if (LIkeBtn != null) {
                LIkeBtn.Dispose ();
                LIkeBtn = null;
            }

            if (LikeCount != null) {
                LikeCount.Dispose ();
                LikeCount = null;
            }

            if (ShareBtn != null) {
                ShareBtn.Dispose ();
                ShareBtn = null;
            }
        }
    }
}