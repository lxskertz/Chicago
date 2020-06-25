using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business
{
    public class EventNameDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        private NSString EventTextFieldCell = new NSString("EventTextFieldCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the data source owner
        /// </summary>
        private EventNameDescController Owner { get; set; }

        #endregion

        #region Constructors
        
        public EventNameDataSource(EventNameDescController owner)
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
            return 4;
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

        //public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        //{
        //    return 3f;
        //}

        /// <summary>
        /// Called by the TableView to get the section title
        /// </summary>
        public override string TitleForHeader(UITableView tableView, nint section)
        {
            string title = "";
            switch (section)
            {
                case 0:
                    title = "Event Title";
                    break;
                case 1:
                    title = "Event Description";
                    break;
                case 2:
                    title = "Event Start Date and Time";
                    break;
                case 3:
                    title = "Event End Date and Time";
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
                    var cell1 = (EventTextFieldCell)tableView.DequeueReusableCell(EventTextFieldCell);
                    cell1.TextLabel.Text = string.IsNullOrEmpty(this.Owner.EventTitle) ? "Enter title" : this.Owner.EventTitle;
                    return cell1;
                case 1:
                    var cell2 = (EventTextFieldCell)tableView.DequeueReusableCell(EventTextFieldCell);
                    cell2.TextLabel.Text = string.IsNullOrEmpty(this.Owner.EventDescription) ? "Enter description" : this.Owner.EventDescription;
                    return cell2;
                case 2:
                    var cell3 = (EventTextFieldCell)tableView.DequeueReusableCell(EventTextFieldCell);
                    cell3.TextLabel.Text = EventNameDescController.EventStartDateTime == null ? "Select start date and time" : EventNameDescController.EventStartDateTime.Value.ToString();
                    return cell3;
                case 3:
                    var cell4 = (EventTextFieldCell)tableView.DequeueReusableCell(EventTextFieldCell);
                    cell4.TextLabel.Text = EventNameDescController.EventEndDateTime == null ? "Select end date and time" : EventNameDescController.EventEndDateTime.Value.ToString();
                    return cell4;

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
                        controller.ControllerCaller = TextFieldInputController.Caller.EventNameController;
                        controller._TextviewValue = string.IsNullOrEmpty(this.Owner.EventTitle) ? string.Empty : this.Owner.EventTitle;
                        controller.CharLimit = 80;
                        this.Owner.NavigationController.PushViewController(controller, true);
                        break;
                    case 1:
                        controller = this.Owner.Storyboard.InstantiateViewController("TextFieldInputController") as TextFieldInputController;
                        controller.ControllerCaller = TextFieldInputController.Caller.EventNameController;
                        controller._TextviewValue = string.IsNullOrEmpty(this.Owner.EventDescription) ? string.Empty : this.Owner.EventDescription;
                        controller.CharLimit = 250;
                        this.Owner.NavigationController.PushViewController(controller, true);
                        break;
                    case 2:
                        pickerController = this.Owner.Storyboard.InstantiateViewController("MyPickerController") as MyPickerController;
                        pickerController.ControllerCaller = MyPickerController.Caller.EventNameController;
                        pickerController.PickerComponentType = MyPickerController.ComponentType.StartDateTime;
                        pickerController.ControllerPickerViewType = MyPickerController.PickerViewType.DatePicker;
                        this.Owner.NavigationController.PushViewController(pickerController, true);
                        break;
                    case 3:
                        pickerController = this.Owner.Storyboard.InstantiateViewController("MyPickerController") as MyPickerController;
                        pickerController.ControllerCaller = MyPickerController.Caller.EventNameController;
                        pickerController.PickerComponentType = MyPickerController.ComponentType.EndDateTime;
                        pickerController.ControllerPickerViewType = MyPickerController.PickerViewType.DatePicker;
                        this.Owner.NavigationController.PushViewController(pickerController, true);
                        break;
                }
            }
            catch (Exception) { }
        }

        #endregion

    }
}