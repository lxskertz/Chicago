using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Orders;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Orders
{
    public class OrderDetailsDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString OrderDetailsCell = new NSString("OrderDetailsCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ToasterOrder ToasterOrder { get; set; }

        private OrderDetailsController Controller { get; set; }

        public ToasterOrder.ToasterOrderEnum ToasterOrderEnum { get; set; }

        #endregion

        #region Constructors

        public OrderDetailsDataSource(OrderDetailsController controller, ToasterOrder toasterOrder,
            ToasterOrder.ToasterOrderEnum toasterOrderEnum)
        {
            this.Controller = controller;
            this.ToasterOrder = toasterOrder;
            this.ToasterOrderEnum = toasterOrderEnum;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a table cell for the row indicated by row property of the NSIndexPath
        /// This method is called multiple times to populate each row of the table.
        /// The method automatically uses cells that have scrolled off the screen or creates new ones as necessary
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (OrderDetailsCell)tableView.DequeueReusableCell(this.OrderDetailsCell);

            if (this.ToasterOrderEnum == ToasterOrder.ToasterOrderEnum.Receiver)
            {
                switch (indexPath.Row)
                {
                    case 0:
                        cell.TextLabel.Text = AppText.OrderNumber;
                        cell.DetailTextLabel.Text = this.ToasterOrder.ToasterOrderId.ToString();
                        break;
                    case 1:
                        cell.TextLabel.Text = AppText.DrinkName;
                        cell.DetailTextLabel.Text = this.ToasterOrder.DrinkName;
                        break;
                    case 2:
                        cell.TextLabel.Text = AppText.ReceiverName;
                        cell.DetailTextLabel.Text = this.ToasterOrder.ReceiverName;
                        break;
                    case 3:
                        cell.TextLabel.Text = AppText.SenderName;
                        cell.DetailTextLabel.Text = this.ToasterOrder.SenderName;
                        break;
                    case 4:
                        cell.TextLabel.Text = AppText.BusinessName;
                        cell.DetailTextLabel.Text = this.ToasterOrder.BusinessName;
                        break;
                    case 5:
                        cell.TextLabel.Text = AppText.OrderDate;
                        cell.DetailTextLabel.Text = this.ToasterOrder.OrderDate != null && this.ToasterOrder.OrderDate.HasValue ? this.ToasterOrder.OrderDate.Value.ToShortDateString() : "";
                        break;
                    case 6:
                        cell.TextLabel.Text = AppText.PickedUpDate;
                        cell.DetailTextLabel.Text = this.ToasterOrder.PickedUpDate != null && this.ToasterOrder.PickedUpDate.HasValue ? this.ToasterOrder.PickedUpDate.Value.ToShortDateString() : "";
                        break;
                    case 7:
                        cell.TextLabel.Text = AppText.PickedUp;
                        cell.DetailTextLabel.Text = this.ToasterOrder.PickedUp ? "Yes" : "No";
                        break;
                    case 8:
                        cell.TextLabel.Text = AppText.FromBusiness;
                        cell.DetailTextLabel.Text = this.ToasterOrder.FromBusiness ? "Yes" : "No";
                        break;
                }
            }
            else
            {
                switch (indexPath.Row)
                {
                    case 0:
                        cell.TextLabel.Text = AppText.OrderNumber;
                        cell.DetailTextLabel.Text = this.ToasterOrder.ToasterOrderId.ToString();
                        break;
                    case 1:
                        cell.TextLabel.Text = AppText.DrinkName;
                        cell.DetailTextLabel.Text = this.ToasterOrder.DrinkName;
                        break;
                    case 2:
                        cell.TextLabel.Text = AppText.TotalOrderAmount;
                        switch (this.ToasterOrderEnum)
                        {
                            case ToasterOrder.ToasterOrderEnum.Receiver:
                            case ToasterOrder.ToasterOrderEnum.Sender:
                                cell.DetailTextLabel.Text = "$" + this.ToasterOrder.TotalOrderAmount.ToString();
                                break;
                            case ToasterOrder.ToasterOrderEnum.Business:
                                cell.DetailTextLabel.Text = "$" + this.ToasterOrder.DrinkAmount.ToString();
                                break;
                        }
                        break;
                    case 3:
                        cell.TextLabel.Text = AppText.ReceiverName;
                        cell.DetailTextLabel.Text = this.ToasterOrder.ReceiverName;
                        break;
                    case 4:
                        cell.TextLabel.Text = AppText.SenderName;
                        cell.DetailTextLabel.Text = this.ToasterOrder.SenderName;
                        break;
                    case 5:
                        cell.TextLabel.Text = AppText.BusinessName;
                        cell.DetailTextLabel.Text = this.ToasterOrder.BusinessName;
                        break;
                    case 6:
                        cell.TextLabel.Text = AppText.OrderDate;
                        cell.DetailTextLabel.Text = this.ToasterOrder.OrderDate != null && this.ToasterOrder.OrderDate.HasValue ? this.ToasterOrder.OrderDate.Value.ToShortDateString() : "";
                        break;
                    case 7:
                        cell.TextLabel.Text = AppText.PickedUpDate;
                        cell.DetailTextLabel.Text = this.ToasterOrder.PickedUpDate != null && this.ToasterOrder.PickedUpDate.HasValue ? this.ToasterOrder.PickedUpDate.Value.ToShortDateString() : "";
                        break;
                    case 8:
                        cell.TextLabel.Text = AppText.Charged;
                        cell.DetailTextLabel.Text = this.ToasterOrder.Charged ? "Yes" : "No";
                        break;
                    case 9:
                        cell.TextLabel.Text = AppText.PickedUp;
                        cell.DetailTextLabel.Text = this.ToasterOrder.PickedUp ? "Yes" : "No";
                        break;
                    case 10:
                        cell.TextLabel.Text = AppText.OrderPointAmount;
                        cell.DetailTextLabel.Text = "$" + this.ToasterOrder.PointsAmount.ToString();
                        break;
                    case 11:
                        cell.TextLabel.Text = AppText.UsedPointType;
                        switch (this.ToasterOrder.UsedPointType)
                        {
                            case ToasterOrder.PointType.None:
                                cell.DetailTextLabel.Text = AppText.None;
                                break;
                            case ToasterOrder.PointType.Full:
                                cell.DetailTextLabel.Text = AppText.FullOff;
                                break;
                            case ToasterOrder.PointType.Quarter:
                                cell.DetailTextLabel.Text = AppText.Quarteroff;
                                break;
                        }
                        break;
                    case 12:
                        cell.TextLabel.Text = AppText.FromBusiness;
                        cell.DetailTextLabel.Text = this.ToasterOrder.FromBusiness ? "Yes" : "No";
                        break;
                }
            }

            return cell;

        }

        /// <summary>
        /// Gets number of section.... which is 1 in this case
        /// </summary>
        /// <param name="tableView"></param>
        /// <returns></returns>
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        /// <summary>
        /// Called when a row is touched
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
        }

        /// <summary>
        /// return num of rows that will be in the section
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.ToasterOrderEnum == ToasterOrder.ToasterOrderEnum.Receiver ? 9 : 13;
        }

        #endregion


    }
}