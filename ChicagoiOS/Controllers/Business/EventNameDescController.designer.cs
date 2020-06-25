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
    [Register ("EventNameDescController")]
    partial class EventNameDescController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView _EventLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton _LogoButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView EventNameDecTable { get; set; }

        [Action ("_LogoButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void _LogoButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (_EventLogo != null) {
                _EventLogo.Dispose ();
                _EventLogo = null;
            }

            if (_LogoButton != null) {
                _LogoButton.Dispose ();
                _LogoButton = null;
            }

            if (EventNameDecTable != null) {
                EventNameDecTable.Dispose ();
                EventNameDecTable = null;
            }
        }
    }
}