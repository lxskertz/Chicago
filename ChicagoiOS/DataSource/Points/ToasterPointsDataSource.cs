using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Points;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Points
{
    public class ToasterPointsDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString ToasterPointCell = new NSString("ToasterPointCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<Point> Points { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private ToasterPointsController Controller { get; set; }

        #endregion

        #region Constructors

        public ToasterPointsDataSource(ToasterPointsController controller, List<Point> points)
        {
            this.Controller = controller;
            this.Points = points;
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
            var cell = (ToasterPointCell)tableView.DequeueReusableCell(this.ToasterPointCell);
            var item = this.Points.ElementAt(indexPath.Row);

            if (item != null)
            {
                cell.TextLabel.Text = item.EarnedDate.HasValue ? item.EarnedDate.Value.ToLongDateString() : "";
                cell.DetailTextLabel.Text = item.PointAmount.ToString() + " Points";
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
            return this.Points.Count;
        }       

        #endregion

    }
}