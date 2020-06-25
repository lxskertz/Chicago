using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Drinks;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Drinks
{
    public class SendDrinkDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString DrinksCell = new NSString("DrinksCell");

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessDrink> Drinks { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private SendDrinkController Controller { get; set; }

        #endregion

        #region Constructors

        public SendDrinkDataSource(SendDrinkController controller, List<BusinessDrink> drinks)
        {
            this.Controller = controller;
            this.Drinks = drinks;
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
            var cell = (DrinksCell)tableView.DequeueReusableCell(this.DrinksCell);
            var item = this.Drinks.ElementAt(indexPath.Row);

            if (item != null)
            {
                cell.TextLabel.Text = string.IsNullOrEmpty(item.DrinkName) ? "" : item.DrinkName;
                cell.DetailTextLabel.Text = "$" + item.Price.ToString();
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
            var controller = this.Controller.Storyboard.InstantiateViewController("DrinkQuantityController") as DrinkQuantityController;
            var item = this.Drinks.ElementAt(indexPath.Row);
            controller.CheckInItem = this.Controller.CheckInItem;
            controller.Drink = item;
            controller.FromBusiness = this.Controller.FromBusiness;
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
            return this.Drinks.Count;
        }

        #endregion


    }
}