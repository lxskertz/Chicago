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
    [Register ("ToasterProfileHeaderCell")]
    partial class ToasterProfileHeaderCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Headline { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HomeTown { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Name { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProfilePic { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RequestBtn { get; set; }

        [Action ("UIButton1499487_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton1499487_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Headline != null) {
                Headline.Dispose ();
                Headline = null;
            }

            if (HomeTown != null) {
                HomeTown.Dispose ();
                HomeTown = null;
            }

            if (Name != null) {
                Name.Dispose ();
                Name = null;
            }

            if (ProfilePic != null) {
                ProfilePic.Dispose ();
                ProfilePic = null;
            }

            if (RequestBtn != null) {
                RequestBtn.Dispose ();
                RequestBtn = null;
            }
        }
    }
}