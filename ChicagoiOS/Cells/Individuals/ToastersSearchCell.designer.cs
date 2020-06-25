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
    [Register ("ToastersSearchCell")]
    partial class ToastersSearchCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton _FollowBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _Name { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _Username { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProfilePic { get; set; }

        [Action ("_FollowBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void _FollowBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (_FollowBtn != null) {
                _FollowBtn.Dispose ();
                _FollowBtn = null;
            }

            if (_Name != null) {
                _Name.Dispose ();
                _Name = null;
            }

            if (_Username != null) {
                _Username.Dispose ();
                _Username = null;
            }

            if (ProfilePic != null) {
                ProfilePic.Dispose ();
                ProfilePic = null;
            }
        }
    }
}