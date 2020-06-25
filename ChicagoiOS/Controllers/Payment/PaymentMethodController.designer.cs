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
    [Register ("PaymentMethodController")]
    partial class PaymentMethodController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView PaymentMethodTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (PaymentMethodTable != null) {
                PaymentMethodTable.Dispose ();
                PaymentMethodTable = null;
            }
        }
    }
}