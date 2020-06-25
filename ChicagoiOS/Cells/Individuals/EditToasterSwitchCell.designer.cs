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
    [Register ("EditToasterSwitchCell")]
    partial class EditToasterSwitchCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _Title { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch _TitleSwitch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_Title != null) {
                _Title.Dispose ();
                _Title = null;
            }

            if (_TitleSwitch != null) {
                _TitleSwitch.Dispose ();
                _TitleSwitch = null;
            }
        }
    }
}