using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Individuals
{
    public class InviteContactDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString InviteContactCell = new NSString("InviteContactCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<PhoneContact> PhoneContacts { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private InviteContactController Controller { get; set; }

        #endregion

        #region Constructors

        public InviteContactDataSource(InviteContactController controller, List<PhoneContact> phoneContacts)
        {
            this.Controller = controller;
            this.PhoneContacts = phoneContacts;
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
            var cell = (InviteContactCell)tableView.DequeueReusableCell(this.InviteContactCell);
            var item = this.PhoneContacts.ElementAt(indexPath.Row);

            if (item != null)
            {
                cell._Title.Text = string.IsNullOrEmpty(item.Name) ? "" : item.Name;

                cell._InviteBtn.Layer.CornerRadius = 4;
                cell._InviteBtn.Layer.BorderWidth = 1;
                cell._InviteBtn.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;
            }

            cell.Item = item;
            cell.IndexPath = indexPath;

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
            return this.PhoneContacts.Count;
        }

        #endregion

    }
}