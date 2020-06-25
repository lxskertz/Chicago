using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models.CheckIns;
using BigTed;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business
{
    public class BusinessCheckInsDataSource : UITableViewSource
    {


        #region Constants, Enums, and Variables

        private NSString BusinessCheckInsCell = new NSString("BusinessCheckInsCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<CheckIn> CheckIns { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public BusinessCheckInsController Controller { get; set; }

        #endregion

        #region Constructors

        public BusinessCheckInsDataSource(BusinessCheckInsController controller, List<CheckIn> checkIns)
        {
            this.Controller = controller;
            this.CheckIns = checkIns;
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
            var cell = (BusinessCheckInsCell)tableView.DequeueReusableCell(this.BusinessCheckInsCell);
            var item = this.CheckIns.ElementAt(indexPath.Row);

            if (item != null)
            {
                var fname = string.IsNullOrEmpty(item.FirstName) ? "" : item.FirstName;
                var lname = string.IsNullOrEmpty(item.LastName) ? "" : item.LastName;
                cell._Title.Text = fname + " " + lname;
                cell._SubTitle.Text = item.CheckInDate.HasValue ? item.CheckInDate.Value.ToLongDateString() : "";
                cell._SendDrinkBtn.Layer.CornerRadius = 4;
                cell._SendDrinkBtn.Layer.BorderWidth = 1;
                cell._SendDrinkBtn.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;
                cell.Item = item;
                cell.DataSource = this;
                cell.IndexPath = indexPath;
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
            return this.CheckIns.Count;
        }

        #endregion

    }
}