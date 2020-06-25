using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Helpers;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Individuals
{
    public class ToastersMoreDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString MoreCell = new NSString("MoreCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// GEts or set the controller
        /// </summary>
        private ToasterMoreController Controller { get; set; }

        /// <summary>
        /// Gets or sets rows
        /// </summary>
        private List<string> Rows { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public ToastersMoreDataSource(ToasterMoreController controller, List<string> rows)
        {
            this.Controller = controller;
            this.Rows = rows;
        }

        #endregion

        #region Methods

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (MoreCell)tableView.DequeueReusableCell(this.MoreCell); 
            cell.TextLabel.Text = this.Rows[indexPath.Row];

            return cell;
        }

        private void OpenPayments()
        {
            var controller = this.Controller.Storyboard.InstantiateViewController("PaymentMethodController") as PaymentMethodController;
            this.Controller.NavigationController.PushViewController(controller, true);
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
        public async override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            var item = this.Rows[indexPath.Row];

            if (AppDelegate.CurrentUser != null && AppDelegate.CurrentUser.IsBusiness)
            {
                switch (item)
                {
                    case MoreScreenHelper.Logout:
                        await Logout();
                        break;
                    case MoreScreenHelper.Payment:
                        OpenPayments();
                        break;
                    case MoreScreenHelper.LiveCheckIns:
                        OpenCheckIns();
                        break;
                }
            }
            else
            {
                switch (item)
                {
                    case MoreScreenHelper.Points:
                        var toasterPointsController = this.Controller.Storyboard.InstantiateViewController("ToasterPointsController") as ToasterPointsController;
                        this.Controller.NavigationController.PushViewController(toasterPointsController, true);
                        break;
                    case MoreScreenHelper.Settings:
                        break;
                    case MoreScreenHelper.Orders:
                        break;
                    case MoreScreenHelper.Payment:
                        OpenPayments();
                        break;
                    case MoreScreenHelper.Logout:
                        await Logout();
                        break;
                }
            }
        }

        /// <summary>
        /// Called by the TableView to determine how many cells to create for that particular section.
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.Rows.Count;
        }

        private void OpenCheckIns()
        {
            var controller = this.Controller.Storyboard.InstantiateViewController("BusinessCheckInsController") as BusinessCheckInsController;
            controller.ScreenCheckInType = Shared.Models.CheckIns.CheckIn.CheckInTypes.Business;
            this.Controller.NavigationController.PushViewController(controller, true);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        private async Task Logout()
        {
            try
            {
                AppDelegate.DeleteSettings();
                UIViewController login = this.Controller.Storyboard.InstantiateViewController("HomeController") as HomeController;
                this.Controller.NavigationController.SetViewControllers(new UIViewController[] { login }, true);

                if (!AppDelegate.IsOfflineMode())
                {
                    await AppDelegate.UsersFactory.Logout(AppDelegate.CurrentUser.Email);
                    //if (!string.IsNullOrEmpty(AppDelegate.DeviceRegistrationId()))
                    //{
                    //    await AppDelegate.NotificationRegisterManager.Delete(AppDelegate.DeviceRegistrationId());
                    //}
                }
            }
            catch (Exception) { }
        }

        #endregion

    }
}