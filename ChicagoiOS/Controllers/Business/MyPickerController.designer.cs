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
    [Register ("MyPickerController")]
    partial class MyPickerController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIDatePicker _DatePicker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView _PickerView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_DatePicker != null) {
                _DatePicker.Dispose ();
                _DatePicker = null;
            }

            if (_PickerView != null) {
                _PickerView.Dispose ();
                _PickerView = null;
            }
        }
    }
}