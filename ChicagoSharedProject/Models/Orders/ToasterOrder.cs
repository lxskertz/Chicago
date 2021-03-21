using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models.Orders
{
    public class ToasterOrder
    {

        public enum PointType
        {
            None = 0,
            Quarter = 1,
            Full = 2
        }

        public enum ToasterOrderEnum
        {
            None = 0,
            Sender = 1,
            Receiver = 2,
            Business = 3
        }

        public int ToasterOrderId { get; set; }

        public int SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }

        public int BusinessDrinkId { get; set; }

        public int BusinessId { get; set; }

        public string SenderStripeCustomerId { get; set; }

        public string ReceiverName { get; set; }

        public string DrinkName { get; set; }

        public double TotalOrderAmount { get; set; }

        public double DrinkAmount { get; set; }

        public double TabsServiceFee { get; set; }

        public double TipAmount { get; set; }

        public double PointsAmount { get; set; }

        public PointType UsedPointType { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? PickedUpDate { get; set; }

        public bool Charged { get; set; }

        public bool PickedUp { get; set; }

        public string BusinessName { get; set; }

        public string SenderName { get; set; }

        public bool Cancelled { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateCreated { get; set; }

        public bool FromBusiness { get; set; }

        public double SalesTax { get; set; }

        public double StripeFee { get; set; }

        public double CardChargeAmount { get; set; }

        public int Quantity { get; set; }

    }
}
