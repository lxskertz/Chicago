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
    [Register ("TextFieldInputController")]
    partial class TextFieldInputController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView _Textview { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_Textview != null) {
                _Textview.Dispose ();
                _Textview = null;
            }
        }
    }
}