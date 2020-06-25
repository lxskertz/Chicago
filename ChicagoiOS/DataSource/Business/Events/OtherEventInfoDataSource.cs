using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business
{
    public class OtherEventInfoDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        private NSString OtherEventInfoCell = new NSString("OtherEventInfoCell");

        private NSString OtherEventInfoCellTwo = new NSString("OtherEventInfoCellTwo"); 


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the data source owner
        /// </summary>
        private OtherEventInfoController Owner { get; set; }

        #endregion

        #region Constructors

        public OtherEventInfoDataSource(OtherEventInfoController owner)
        {
            this.Owner = owner;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called by the TableView to determine how many sections(groups) there are.
        /// </summary>
        public override nint NumberOfSections(UITableView tableView)
        {
            return 7;
        }

        /// <summary>
        /// Called by the TableView to determine how many rows to create for that particular section.
        /// </summary>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 1;
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return 15f;
        }

        /// <summary>
        /// Called by the TableView to get the section title
        /// </summary>
        public override string TitleForHeader(UITableView tableView, nint section)
        {
            string title = "";
            switch (section)
            {
                case 0:
                    title = "Venue Name";
                    break;
                case 1:
                    title = "Street Address";
                    break;
                case 2:
                    title = "City";
                    break;
                case 3:
                    title = "State";
                    break;
                case 4:
                    title = "Zipcode";
                    break;
                case 5:
                    title = "Event Type";
                    break;
                case 6:
                    title = "Event Category";
                    break;
            }
            return title;
        }

        /// <summary>
        /// Called by the TableView to get the actual UITableViewCell to render for the particular row
        /// </summary>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            switch (indexPath.Section)
            {
                case 0:
                    var cell1 = (OtherEventInfoCell)tableView.DequeueReusableCell(OtherEventInfoCell);
                    cell1.TextLabel.Text = string.IsNullOrEmpty(this.Owner.Venue) ? "Enter venue name" : this.Owner.Venue;
                    return cell1;
                case 1:
                    var cell2 = (OtherEventInfoCell)tableView.DequeueReusableCell(OtherEventInfoCell);
                    cell2.TextLabel.Text = string.IsNullOrEmpty(this.Owner.StreetAddress) ? "Enter street address" : this.Owner.StreetAddress;
                    return cell2;
                case 2:
                    var cell3 = (OtherEventInfoCell)tableView.DequeueReusableCell(OtherEventInfoCell);
                    cell3.TextLabel.Text = string.IsNullOrEmpty(this.Owner.City) ? "Enter city" : this.Owner.City;
                    return cell3;
                case 3:
                    var cell4 = (OtherEventInfoCell)tableView.DequeueReusableCell(OtherEventInfoCell);
                    cell4.TextLabel.Text = string.IsNullOrEmpty(this.Owner.State) ? "Enter state" : this.Owner.State;
                    return cell4;
                case 4:
                    var cell5 = (OtherEventInfoCell)tableView.DequeueReusableCell(OtherEventInfoCell);
                    cell5.TextLabel.Text = string.IsNullOrEmpty(this.Owner.Zipcode) ? "Enter zipcode" : this.Owner.Zipcode;
                    return cell5;
                case 5:
                    var cell6 = (OtherEventInfoCell)tableView.DequeueReusableCell(OtherEventInfoCell);
                    cell6.TextLabel.Text = OtherEventInfoController.SelectedEventType == null ? "Select event type" : OtherEventInfoController.SelectedEventType.TypeName;
                    return cell6;
                case 6:
                    var cell7 = (OtherEventInfoCell)tableView.DequeueReusableCell(OtherEventInfoCell);
                    cell7.TextLabel.Text = OtherEventInfoController.SelectedEventCategory == null ? "Select event category" : OtherEventInfoController.SelectedEventCategory.CategoryName;
                    return cell7;

            }

            return new UITableViewCell();

        }

        /// <summary>
        /// Reload screen if other version sec row is selected
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="indexPath"></param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            try
            {
                tableView.DeselectRow(indexPath, true);
                MyPickerController pickerController;
                TextFieldInputController controller;
                this.Owner.SelectedSection = indexPath.Section;
                switch (indexPath.Section)
                {
                    case 0:
                        controller = this.Owner.Storyboard.InstantiateViewController("TextFieldInputController") as TextFieldInputController;
                        controller.ControllerCaller = TextFieldInputController.Caller.OtherEventInfo;
                        controller._TextviewValue = string.IsNullOrEmpty(this.Owner.Venue) ? string.Empty : this.Owner.Venue;
                        controller.CharLimit = 50;
                        this.Owner.NavigationController.PushViewController(controller, true);
                        break;
                    case 1:
                        controller = this.Owner.Storyboard.InstantiateViewController("TextFieldInputController") as TextFieldInputController;
                        controller.ControllerCaller = TextFieldInputController.Caller.OtherEventInfo;
                        controller._TextviewValue = string.IsNullOrEmpty(this.Owner.StreetAddress) ? string.Empty : this.Owner.StreetAddress;
                        controller.CharLimit = 150;
                        this.Owner.NavigationController.PushViewController(controller, true);
                        break;
                    case 2:
                        controller = this.Owner.Storyboard.InstantiateViewController("TextFieldInputController") as TextFieldInputController;
                        controller.ControllerCaller = TextFieldInputController.Caller.OtherEventInfo;
                        controller._TextviewValue = string.IsNullOrEmpty(this.Owner.City) ? string.Empty : this.Owner.City;
                        controller.CharLimit = 40;
                        this.Owner.NavigationController.PushViewController(controller, true);
                        break;
                    case 3:
                        controller = this.Owner.Storyboard.InstantiateViewController("TextFieldInputController") as TextFieldInputController;
                        controller.ControllerCaller = TextFieldInputController.Caller.OtherEventInfo;
                        controller._TextviewValue = string.IsNullOrEmpty(this.Owner.State) ? string.Empty : this.Owner.State;
                        controller.CharLimit = 40;
                        this.Owner.NavigationController.PushViewController(controller, true);
                        break;
                    case 4:
                        controller = this.Owner.Storyboard.InstantiateViewController("TextFieldInputController") as TextFieldInputController;
                        controller.ControllerCaller = TextFieldInputController.Caller.OtherEventInfo;
                        controller._TextviewValue = string.IsNullOrEmpty(this.Owner.Zipcode) ? string.Empty : this.Owner.Zipcode;
                        controller.CharLimit = 8;
                        this.Owner.NavigationController.PushViewController(controller, true);
                        break;
                    case 5:
                        pickerController = this.Owner.Storyboard.InstantiateViewController("MyPickerController") as MyPickerController;
                        pickerController.ControllerCaller = MyPickerController.Caller.OtherEventInfo;
                        pickerController.PickerComponentType = MyPickerController.ComponentType.EventTypes;
                        this.Owner.NavigationController.PushViewController(pickerController, true);
                        break;
                    case 6:
                        pickerController = this.Owner.Storyboard.InstantiateViewController("MyPickerController") as MyPickerController;
                        pickerController.ControllerCaller = MyPickerController.Caller.OtherEventInfo;
                        pickerController.PickerComponentType = MyPickerController.ComponentType.EventCategories;
                        this.Owner.NavigationController.PushViewController(pickerController, true);
                        break;
                }
            }
            catch (Exception) { }
        }

        #endregion

    }
}