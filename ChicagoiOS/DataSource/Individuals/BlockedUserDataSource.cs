using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Individuals
{
    public class BlockedUserDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString BlockedUserCell = new NSString("BlockedUserCell");

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public BlockedUsersController Controller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Toasters> Rows { get; set; } = new List<Toasters>();

        #endregion

        #region Constructors

        public BlockedUserDataSource(BlockedUsersController controller, List<Toasters> rows)
        {
            this.Controller = controller;
            this.Rows = rows;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        public void AddRowItems(List<Toasters> rows)
        {
            foreach (var row in rows)
            {
                Rows.Add(row);
            }
        }
     
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
            var row = this.Rows[indexPath.Row];
            var cell = (BlockedUserCell)tableView.DequeueReusableCell(this.BlockedUserCell);
            var firstName = !string.IsNullOrEmpty(row.FirstName) ? row.FirstName : string.Empty;
            var lastName = !string.IsNullOrEmpty(row.LastName) ? row.LastName : string.Empty;
            cell._Name.Text = firstName + " " + lastName;
            cell.Item = row;
            cell.DataSource = this;
            cell.IndexPath = indexPath;
            cell.Tag = indexPath.Row;
            this.Controller.AddButtonBorder(cell._UnblockBtn);

            return cell;

        }

        /// <summary>
        /// Load more
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="willDecelerate"></param>
        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            //if (this.Rows.Count == 0 || !this.Controller.loadMore)
            //{
            //    return;
            //}

            //nfloat currentOffset = scrollView.ContentOffset.Y;
            //nfloat maximumOffset = scrollView.ContentSize.Height - scrollView.Frame.Size.Height;

            //if (maximumOffset - currentOffset <= -1)
            //{
            //    await this.Controller.ScrolledToBottom();
            //}
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
            //var row = this.Rows[indexPath.Row];
            //var controller = this.Controller.Storyboard.InstantiateViewController("ToasterProfileController") as ToasterProfileController;
            //controller.FromSearchedUser = true;
            //controller.FromRequestPending = this.Controller.pendingRequestShown;
            //controller.Toaster = row;
            //controller.SearchedUser = new Shared.Models.Users.Users()
            //{
            //    Email = row.Email,
            //    UserId = row.UserId,
            //    FirstName = row.FirstName,
            //    LastName = row.LastName,
            //    Username = row.Username,
            //};

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
            return this.Rows.Count;
        }

        #endregion

    }
}