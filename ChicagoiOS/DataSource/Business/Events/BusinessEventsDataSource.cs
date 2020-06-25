using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models.Events;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business
{
    public class BusinessEventsDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString BusinessEventCell = new NSString("BusinessEventCell");

        #endregion

        #region Properties
        
        /// <summary>
        /// 
        /// </summary>
        public List<BusinessEvents> BusinessEvents { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private BusinessEventController Controller { get; set; }

        #endregion

        #region Constructors

        public BusinessEventsDataSource(BusinessEventController controller, List<BusinessEvents> businessEvents)
        {
            this.Controller = controller;
            this.BusinessEvents = businessEvents;
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
            var cell = (BusinessEventCell)tableView.DequeueReusableCell(this.BusinessEventCell);
            var item = this.BusinessEvents.ElementAt(indexPath.Row);

            if (item != null)
            {
                cell.TextLabel.Text = string.IsNullOrEmpty(item.Title) ? "" : item.Title;
                cell.DetailTextLabel.Text = string.IsNullOrEmpty(item.EventDescription) ? "" : item.EventDescription;
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
            var item = this.BusinessEvents.ElementAt(indexPath.Row);
            //var itemLogo = this.ImageViewImage.Where(x => x.Id == item.EventId).FirstOrDefault();
            var controller = this.Controller.Storyboard.InstantiateViewController("EventInfoController") as EventInfoController;
            controller.BusinessEvents = item;
            //controller.EventImage = itemLogo.Image;
            controller.ShowToolbar = false;
            controller.IsBusiness = true;
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
            return this.BusinessEvents.Count;
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

                    if (AppDelegate.IsOfflineMode())
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    }
                    else
                    {
                        var item = this.BusinessEvents.ElementAt(indexPath.Row);

                        await DeleteEvent(item);

                        // remove the item from the underlying data source
                        this.BusinessEvents.RemoveAt(indexPath.Row);

                        // delete the row from the table
                        tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Left);
                    }
                    break;
                case UITableViewCellEditingStyle.Insert:
                    Console.WriteLine("CommitEditingStyle:None called");
                    break;
            }
        }

        /// <summary>
        /// Delete event
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task DeleteEvent(BusinessEvents bEvent)
        {
            try
            {
                BTProgressHUD.Show(ToastMessage.Deleting, -1, ProgressHUD.MaskType.Clear);
                await AppDelegate.BusinessEventsFactory.Delete(bEvent.EventId);
                await BlobStorageHelper.DeleteEventLogoBlob(bEvent.EventId);
                BTProgressHUD.Dismiss();
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