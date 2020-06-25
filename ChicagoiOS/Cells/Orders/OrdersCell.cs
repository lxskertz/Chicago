using Foundation;
using System;
using UIKit;
using System.Threading.Tasks;
using Tabs.Mobile.ChicagoiOS.DataSource.Orders;
using Tabs.Mobile.Shared.Models.Orders;
using Tabs.Mobile.Shared.Resources;
using BigTed;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class OrdersCell : UITableViewCell
    {

        public UILabel _OrderNumber
        {
            get
            {
                return OrderNumber;
            }
        }

        public UILabel _ReceiverName
        {
            get
            {
                return ReceiverName;
            }
        }

        public UILabel _DrinkName
        {
            get
            {
                return DrinkName;
            }
        }

        public UIButton _ChargeBtn
        {
            get
            {
                return ChargeBtn;
            }
        }

        public OrdersCell (IntPtr handle) : base (handle)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public OrdersDataSource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ToasterOrder Item { get; set; }

        partial void ChargeBtn_TouchUpInside(UIButton sender)
        {
            switch (this.DataSource.ToasterOrderEnum)
            {
                case ToasterOrder.ToasterOrderEnum.Sender:
                    if (!Item.Charged)
                    {
                        this.DataSource.Controller.CancelDrink(Item);
                    }
                    break;
                case ToasterOrder.ToasterOrderEnum.Business:
                    if (!Item.Charged && !Item.FromBusiness)
                    {
                        this.DataSource.Controller.ChargeDrink(Item);
                    } 
                    else if (Item.FromBusiness)
                    {
                        this.DataSource.Controller.MarkPickedUp(Item);
                    }
                    break;
            }
        }
    }
}