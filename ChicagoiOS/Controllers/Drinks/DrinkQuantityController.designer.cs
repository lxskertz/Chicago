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
    [Register ("DrinkQuantityController")]
    partial class DrinkQuantityController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ChangePaymentBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DrinInfoView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DrinkName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MinusBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PaymentDisclaimer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PaymentOptionText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PaymentOptionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PointDiscountLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PointsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Price { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Quantity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel QuantityHeader { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView QuantityView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SelectPaymentOption { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton StartTabBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel tipAmountText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton tipDecrementBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel tipHeader { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton tipIncrementBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TipView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UpdatedPriceText { get; set; }

        [Action ("AddBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("ChangePaymentBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ChangePaymentBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("MinusBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void MinusBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("StartTabBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void StartTabBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("TipDecrementBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TipDecrementBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("TipIncrementBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TipIncrementBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddBtn != null) {
                AddBtn.Dispose ();
                AddBtn = null;
            }

            if (ChangePaymentBtn != null) {
                ChangePaymentBtn.Dispose ();
                ChangePaymentBtn = null;
            }

            if (DrinInfoView != null) {
                DrinInfoView.Dispose ();
                DrinInfoView = null;
            }

            if (DrinkName != null) {
                DrinkName.Dispose ();
                DrinkName = null;
            }

            if (MinusBtn != null) {
                MinusBtn.Dispose ();
                MinusBtn = null;
            }

            if (PaymentDisclaimer != null) {
                PaymentDisclaimer.Dispose ();
                PaymentDisclaimer = null;
            }

            if (PaymentOptionText != null) {
                PaymentOptionText.Dispose ();
                PaymentOptionText = null;
            }

            if (PaymentOptionView != null) {
                PaymentOptionView.Dispose ();
                PaymentOptionView = null;
            }

            if (PointDiscountLbl != null) {
                PointDiscountLbl.Dispose ();
                PointDiscountLbl = null;
            }

            if (PointsView != null) {
                PointsView.Dispose ();
                PointsView = null;
            }

            if (Price != null) {
                Price.Dispose ();
                Price = null;
            }

            if (Quantity != null) {
                Quantity.Dispose ();
                Quantity = null;
            }

            if (QuantityHeader != null) {
                QuantityHeader.Dispose ();
                QuantityHeader = null;
            }

            if (QuantityView != null) {
                QuantityView.Dispose ();
                QuantityView = null;
            }

            if (SelectPaymentOption != null) {
                SelectPaymentOption.Dispose ();
                SelectPaymentOption = null;
            }

            if (StartTabBtn != null) {
                StartTabBtn.Dispose ();
                StartTabBtn = null;
            }

            if (tipAmountText != null) {
                tipAmountText.Dispose ();
                tipAmountText = null;
            }

            if (tipDecrementBtn != null) {
                tipDecrementBtn.Dispose ();
                tipDecrementBtn = null;
            }

            if (tipHeader != null) {
                tipHeader.Dispose ();
                tipHeader = null;
            }

            if (tipIncrementBtn != null) {
                tipIncrementBtn.Dispose ();
                tipIncrementBtn = null;
            }

            if (TipView != null) {
                TipView.Dispose ();
                TipView = null;
            }

            if (UpdatedPriceText != null) {
                UpdatedPriceText.Dispose ();
                UpdatedPriceText = null;
            }
        }
    }
}