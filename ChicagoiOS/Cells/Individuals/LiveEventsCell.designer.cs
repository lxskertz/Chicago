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
    [Register ("LiveEventsCell")]
    partial class LiveEventsCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton _ShareBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CheckInBtn { get; set; }

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
        UIKit.UIButton LikeBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LikeCount { get; set; }

        [Action ("CheckInBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CheckInBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("LikeBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LikeBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (_ShareBtn != null) {
                _ShareBtn.Dispose ();
                _ShareBtn = null;
            }

            if (CheckInBtn != null) {
                CheckInBtn.Dispose ();
                CheckInBtn = null;
            }

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

            if (LikeBtn != null) {
                LikeBtn.Dispose ();
                LikeBtn = null;
            }

            if (LikeCount != null) {
                LikeCount.Dispose ();
                LikeCount = null;
            }
        }
    }
}