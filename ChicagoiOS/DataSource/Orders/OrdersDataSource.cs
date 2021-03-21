using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Orders;
using Tabs.Mobile.Shared.Helpers;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Orders
{
    public class OrdersDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString OrdersCell = new NSString("OrdersCell");

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<ToasterOrder> ToasterOrders { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public OrdersController Controller { get; set; }

        public ToasterOrder.ToasterOrderEnum ToasterOrderEnum { get; set; }

        #endregion

        #region Constructors

        public OrdersDataSource(OrdersController controller, List<ToasterOrder> toasterOrders,
            ToasterOrder.ToasterOrderEnum toasterOrderEnum)
        {
            this.Controller = controller;
            this.ToasterOrders = toasterOrders;
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
            var cell = (OrdersCell)tableView.DequeueReusableCell(this.OrdersCell);
            var item = this.ToasterOrders.ElementAt(indexPath.Row);

            if (item != null)
            {
                string freeOrder = item.FromBusiness ? AppText.FreeOrder : "";
                cell._OrderNumber.Text = item.ToasterOrderId.ToString() + freeOrder;
                cell._DrinkName.Text = SendDrinkHelper.GetDrinkQuantityAndName(item);
                var buttonTitle = AppText.PickedUp;

                switch (this.ToasterOrderEnum)
                {
                    case ToasterOrder.ToasterOrderEnum.Sender:
                        buttonTitle = item.Charged ? AppText.PickedUp : AppText.Cancel;
                        cell._ChargeBtn.Enabled = buttonTitle == AppText.Cancel;
                        cell._ChargeBtn.UserInteractionEnabled = buttonTitle == AppText.Cancel;
                        cell._ChargeBtn.SetTitle(buttonTitle, UIControlState.Normal);
                        cell._ReceiverName.Text = string.IsNullOrEmpty(item.ReceiverName) ? "" : item.ReceiverName;
                        break;
                    case ToasterOrder.ToasterOrderEnum.Receiver:
                        cell._ChargeBtn.Hidden = true;
                        cell._ReceiverName.Text = string.IsNullOrEmpty(item.SenderName) ? "" : item.SenderName;
                        break;
                    case ToasterOrder.ToasterOrderEnum.Business:
                        if (item.FromBusiness)
                        {
                            buttonTitle = item.PickedUp ? AppText.PickedUp : AppText.ConfirmPicked;
                            cell._ChargeBtn.Enabled = buttonTitle == AppText.ConfirmPicked;
                            cell._ChargeBtn.UserInteractionEnabled = buttonTitle == AppText.ConfirmPicked;
                        }
                        else
                        {
                            buttonTitle = item.Charged ? AppText.PickedUp : AppText.Charge;
                            cell._ChargeBtn.Enabled = buttonTitle == AppText.Charge;
                            cell._ChargeBtn.UserInteractionEnabled = buttonTitle == AppText.Charge;
                        }
                        cell._ChargeBtn.SetTitle(buttonTitle, UIControlState.Normal);
                        cell._ReceiverName.Text = string.IsNullOrEmpty(item.ReceiverName) ? "" : item.ReceiverName;
                        break;
                }

                cell._ChargeBtn.Layer.CornerRadius = 4;
                cell._ChargeBtn.Layer.BorderWidth = 1;
                cell._ChargeBtn.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;

                cell.Item = item;
                cell.IndexPath = indexPath;
                cell.DataSource = this;
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
            var item = this.ToasterOrders.ElementAt(indexPath.Row);
            var controller = this.Controller.Storyboard.InstantiateViewController("OrderDetailsController") as OrderDetailsController;
            controller.ToasterOrderEnum = this.ToasterOrderEnum;
            controller.ToasterOrder = item;
            this.Controller.NavigationController.PushViewController(controller, true);
        }

        /// <summary>
        /// return num of rows that will be in the section
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.ToasterOrders.Count;
        }

        #endregion

    }
}