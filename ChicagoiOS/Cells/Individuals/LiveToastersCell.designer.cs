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
    [Register ("LiveToastersCell")]
    partial class LiveToastersCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LikeBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LikeCount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MoreBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SendDrinkBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UserImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Username { get; set; }

        [Action ("LikeBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LikeBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("MoreBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void MoreBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("SendDrinkBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SendDrinkBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (LikeBtn != null) {
                LikeBtn.Dispose ();
                LikeBtn = null;
            }

            if (LikeCount != null) {
                LikeCount.Dispose ();
                LikeCount = null;
            }

            if (MoreBtn != null) {
                MoreBtn.Dispose ();
                MoreBtn = null;
            }

            if (SendDrinkBtn != null) {
                SendDrinkBtn.Dispose ();
                SendDrinkBtn = null;
            }

            if (UserImage != null) {
                UserImage.Dispose ();
                UserImage = null;
            }

            if (Username != null) {
                Username.Dispose ();
                Username = null;
            }
        }
    }
}