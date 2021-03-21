using System;
using System.Collections.Generic;
using System.Text;
using Tabs.Mobile.Shared.Models.Orders;

namespace Tabs.Mobile.Shared.Helpers
{
    public static class SendDrinkHelper
    {

        #region Constants, Enums, and Variables

        public enum PointToUse
        {
            None = 0,
            Quarter = 25,
            Full = 100
        }

        public static int counter = 1;
        public static int tipCounter = 0;
        public const double stripeServiceFee = 0.029;
        public const double additionalStripeServiceFee = 0.3;
        public const double tabServiceFee = 0.03;
        public const double quarterOffAmt = 0.25;
        public const int QuarterPercentOff = 500;
        public const int FullOff = 1000;
        public const string FullPointsText = "100% off with points";
        public const string QuarterPointsText = "25% off with points";

        #endregion

        #region Properties

        public static string TabsServiceFeeText { get; private set; } = " + TABS service fee.";

        public static string CurrencySymbol { get; private set; } = "$";

        public static PointToUse SelectedPointToUse { get; set; } = PointToUse.None;

        #endregion

        #region Methods
    
        public static string GetPointToUseText()
        {
            switch (SelectedPointToUse)
            {
                case PointToUse.Full:
                    return FullPointsText;
                case PointToUse.Quarter:
                    return QuarterPointsText;
                default:
                    return "";
            }
        }

        public static double InitialUpdatedPrice(double price, bool usePoints)
        {
            if (!usePoints)
            {
                var value = (tabServiceFee * price) + price;
                var stripeFee = (stripeServiceFee * value) + additionalStripeServiceFee;
                var totalValue = value + stripeFee;

                return Math.Round(totalValue, 2);
            }
            else
            {
                if (SelectedPointToUse == PointToUse.Quarter)
                {
                    var value = (tabServiceFee * price) + price;
                    value = value - (value * quarterOffAmt);
                    var stripeFee = (stripeServiceFee * value) + additionalStripeServiceFee;
                    var totalValue = value + stripeFee;

                    return Math.Round(totalValue, 2);
                } else
                {
                    return 0;
                }
            }
        }

        private static double CalculateTip(double drinkAmount)
        {
            if(tipCounter <= 0)
            {
                return 0.0;
            }

            var tipAmt = (double)tipCounter / 100;

            return tipAmt * drinkAmount;
        }

        public static double RemoveStripeFee(double value)
        {
            var stripeFee = (stripeServiceFee * value) + additionalStripeServiceFee;

            return value - stripeFee;
        }

        public static Tuple<double, double, double, double, double, double> CalculateUpdatedPrice(double price, bool usePoints)
        {
            if (!usePoints)
            {
                 var drinkAmount = price * counter;
                var tabFee = tabServiceFee * drinkAmount;
                var tip = CalculateTip(drinkAmount);
                var value = tabFee + tip + drinkAmount;
                var stripeFee = (stripeServiceFee * value) + additionalStripeServiceFee;
                var totalAmount = value + stripeFee;

                totalAmount = Math.Round(totalAmount, 2);
                tabFee = Math.Round(tabFee, 2);
                drinkAmount = Math.Round(drinkAmount, 2);
                stripeFee = Math.Round(stripeFee, 2);
                tip = Math.Round(tip, 2);

                var prices = Tuple.Create(tabFee, drinkAmount, totalAmount, 0.0, stripeFee, tip);
                return prices;
            }
            else
            {
                var drinkAmount = price * counter;
                var tabFee = tabServiceFee * drinkAmount;
                var tip = CalculateTip(drinkAmount);
                var value = tabFee + tip + drinkAmount;
                //var stripeFee = (stripeServiceFee * value) + additionalStripeServiceFee;
                var totalAmount = value; //+ stripeFee;
                double discountAmt = 0.0;

                tabFee = Math.Round(tabFee, 2);
                drinkAmount = Math.Round(drinkAmount, 2);
                //stripeFee = Math.Round(stripeFee, 2);
                tip = Math.Round(tip, 2);

                double stripeFee = 0.0;
                if (SelectedPointToUse == PointToUse.Quarter)
                {
                    discountAmt = (totalAmount * quarterOffAmt);
                    discountAmt = Math.Round(discountAmt, 2);
                    totalAmount = totalAmount - discountAmt;

                    stripeFee = (stripeServiceFee * totalAmount) + additionalStripeServiceFee;
                    totalAmount = totalAmount + stripeFee;

                    stripeFee = Math.Round(stripeFee, 2);
                    totalAmount = Math.Round(totalAmount, 2);

                } else
                {
                    discountAmt = Math.Round(totalAmount, 2);
                    totalAmount = 0.00;
                }

                var prices = Tuple.Create(tabFee, drinkAmount, totalAmount, discountAmt, stripeFee, tip);
                return prices;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="increment"></param>
        public static int ModifyCounter(bool increment)
        {
            if (increment)
            {
                return counter = counter + 1;                
            }
            else
            {
                if (counter <= 1)
                {
                    return counter = 1;
                }

                return counter = counter - 1;
            }
        }

        public static int ModifyTipCounter(bool increment)
        {
            if (increment)
            {
                return tipCounter = tipCounter + 1;
            }
            else
            {
                if (tipCounter <= 0)
                {
                    return tipCounter = 0;
                }

                return tipCounter = tipCounter - 1;
            }
        }

        public static int GetDrinkQuantity(string quantity)
        {
            int drinkQuantity = 0;

            if (string.IsNullOrEmpty(quantity))
            {
                return 0;
            }

            int.TryParse(quantity, out drinkQuantity);

            return drinkQuantity;
        }


        public static string GetDrinkQuantityAndName(ToasterOrder item)
        {
            if(string.IsNullOrEmpty(item.DrinkName))
            {
                return item.Quantity + " ";
            }

            //var quantity = item.Quantity > 0 ? item.Quantity : 0;

            return item.Quantity > 0 ? item.Quantity + " - " + item.DrinkName : item.DrinkName;

        }

        #endregion

    }
}
  