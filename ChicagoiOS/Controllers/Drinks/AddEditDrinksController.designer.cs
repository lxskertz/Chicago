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
    [Register ("AddEditDrinksController")]
    partial class AddEditDrinksController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField DrinkName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField DrinkPrice { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SaveBtn { get; set; }

        [Action ("SaveBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SaveBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (DrinkName != null) {
                DrinkName.Dispose ();
                DrinkName = null;
            }

            if (DrinkPrice != null) {
                DrinkPrice.Dispose ();
                DrinkPrice = null;
            }

            if (SaveBtn != null) {
                SaveBtn.Dispose ();
                SaveBtn = null;
            }
        }
    }
}