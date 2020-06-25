using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Events;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business
{
    public class BusinessRsvpDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString BusinessRsvpCell = new NSString("BusinessRsvpCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<Rsvp> Rsvps { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private BusinessRsvpController Controller { get; set; }

        #endregion

        #region Constructors

        public BusinessRsvpDataSource(BusinessRsvpController controller, List<Rsvp> rsvps)
        {
            this.Controller = controller;
            this.Rsvps = rsvps;
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
            var cell = (BusinessRsvpCell)tableView.DequeueReusableCell(this.BusinessRsvpCell);
            var item = this.Rsvps.ElementAt(indexPath.Row);

            if (item != null)
            {
                var fname = string.IsNullOrEmpty(item.FirstName) ? "" : item.FirstName;
                var lname = string.IsNullOrEmpty(item.LastName) ? "" : item.LastName;
                cell.TextLabel.Text = fname + " " + lname;
                var going = item.Going ? "Yes" : "No";
                cell.DetailTextLabel.Text = "Going: " + going;
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
            //var controller = this.Controller.Storyboard.InstantiateViewController("AddEditDrinksController") as AddEditDrinksController;
            //var item = this.Drinks.ElementAt(indexPath.Row);
            //controller.ScreenActionType = AddEditDrinksController.ActionType.Edit;
            //controller.Drink = item;
            //this.Controller.NavigationController.PushViewController(controller, true);
        }

        /// <summary>
        /// return num of rows that will be in the section
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.Rsvps.Count;
        }

        #endregion


    }
}