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
    [Register ("ToastersCell")]
    partial class ToastersCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _Name { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView _ProfilePic { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _Username { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_Name != null) {
                _Name.Dispose ();
                _Name = null;
            }

            if (_ProfilePic != null) {
                _ProfilePic.Dispose ();
                _ProfilePic = null;
            }

            if (_Username != null) {
                _Username.Dispose ();
                _Username = null;
            }
        }
    }
}