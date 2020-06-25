using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using BigTed;
using Stripe;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Payment;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Payments
{
    public class PaymentMethodsDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString CardsCell = new NSString("CardsCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<Card> Cards { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private PaymentMethodController Controller { get; set; }

        #endregion

        #region Constructors

        public PaymentMethodsDataSource(PaymentMethodController controller, List<Card> cards)
        {
            this.Controller = controller;
            this.Cards = cards;
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
            var cell = (CardsCell)tableView.DequeueReusableCell(this.CardsCell);
            var item = this.Cards.ElementAt(indexPath.Row);

            if (item != null)
            {
                cell.TextLabel.Text = string.IsNullOrEmpty(item.Last4) ? "" : AppText.Asterisks + item.Last4 + "    " + item.ExpMonth + "/" + item.ExpYear;
                cell.Accessory = indexPath.Row == 0 ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
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
            var item = this.Cards.ElementAt(indexPath.Row);

            if (this.Controller.FromQuantityController)
            {
                DrinkQuantityController.RequiresRefresh = true;
                DrinkQuantityController.defaultPayment = item;
                //DrinkQuantityController.StripeCustomerInfo = this.Controller.StripeCustomerInfo;
                this.Controller.NavigationController.PopViewController(true);
            }
        }

        /// <summary>
        /// return num of rows that will be in the section
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.Cards.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="editingStyle"></param>
        /// <param name="indexPath"></param>
        public async override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    var item = this.Cards.ElementAt(indexPath.Row);
                    await DeleteCard(item);
                    this.Cards.RemoveAt(indexPath.Row);
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Left);
                    break;
            }
        }

        /// <summary>
        /// Delete event
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task DeleteCard(Card card)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    if (this.Controller.StripeCustomerInfo != null)
                    {
                        BTProgressHUD.Show(ToastMessage.Deleting, -1, ProgressHUD.MaskType.Clear);
                        await AppDelegate.CustomerPaymentInfoFactory.DeleteCard(card.Id, this.Controller.StripeCustomerInfo.StripeCustomerId);
                        BTProgressHUD.Dismiss();
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        #endregion

    }
}