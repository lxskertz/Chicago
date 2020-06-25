using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models.Reports.InappropriateReports;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Reports
{
    public class InappropraiteOptionsDatasource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString InappropriateReportCell = new NSString("InappropriateReportCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<InappropriateReport.ReportReason, string> Reasons { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private InappropraiteOptionsController Controller { get; set; }

        #endregion

        #region Constructors

        public InappropraiteOptionsDatasource(InappropraiteOptionsController controller,
            Dictionary<InappropriateReport.ReportReason, string> reasons)
        {
            this.Controller = controller;
            this.Reasons = reasons;
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
            var cell = (InappropriateReportCell)tableView.DequeueReusableCell(this.InappropriateReportCell);
            var item = this.Reasons.ElementAt(indexPath.Row);

            //{
                cell.TextLabel.Text = string.IsNullOrEmpty(item.Value) ? "" : item.Value;
                //cell.DetailTextLabel.Text = "$" + item.Price.ToString();
            //}

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
            this.Controller.ReportInappropriate(this.Reasons.ElementAt(indexPath.Row).Key);
        }

        /// <summary>
        /// return num of rows that will be in the section
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.Reasons.Count;
        }

        #endregion


    }
}