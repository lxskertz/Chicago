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
    [Register ("EditToasterTxtFieldCell")]
    partial class EditToasterTxtFieldCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Title { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TxtField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Title != null) {
                Title.Dispose ();
                Title = null;
            }

            if (TxtField != null) {
                TxtField.Dispose ();
                TxtField = null;
            }
        }
    }
}