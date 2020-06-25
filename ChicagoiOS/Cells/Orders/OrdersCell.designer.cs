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
    [Register ("OrdersCell")]
    partial class OrdersCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ChargeBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DrinkName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel OrderNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ReceiverName { get; set; }

        [Action ("ChargeBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ChargeBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ChargeBtn != null) {
                ChargeBtn.Dispose ();
                ChargeBtn = null;
            }

            if (DrinkName != null) {
                DrinkName.Dispose ();
                DrinkName = null;
            }

            if (OrderNumber != null) {
                OrderNumber.Dispose ();
                OrderNumber = null;
            }

            if (ReceiverName != null) {
                ReceiverName.Dispose ();
                ReceiverName = null;
            }
        }
    }
}