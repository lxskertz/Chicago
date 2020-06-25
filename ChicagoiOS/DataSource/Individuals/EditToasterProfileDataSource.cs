using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Individuals
{
    public class EditToasterProfileDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString EditToasterSwitchCell = new NSString("EditToasterSwitchCell"); 

        /// <summary>
        /// 
        /// </summary>
        private NSString EditToasterTxtFieldCell = new NSString("EditToasterTxtFieldCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public EditToasterProfileController Controller { get; set; }

        #endregion

        #region Constructors

        public EditToasterProfileDataSource(EditToasterProfileController controller)
        {
            this.Controller = controller;
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
            switch (indexPath.Row)
            {
                case 0:
                    var cell0 = (EditToasterTxtFieldCell)tableView.DequeueReusableCell(this.EditToasterTxtFieldCell);
                    cell0.BackgroundColor = UIColor.Clear;
                    cell0.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell0._Title.Text = "First name";
                    cell0._TitleField.Text = AppDelegate.CurrentUser.FirstName;
                    this.Controller.Firstname = cell0._TitleField;

                    return cell0;
                case 1:
                    var cell1 = (EditToasterTxtFieldCell)tableView.DequeueReusableCell(this.EditToasterTxtFieldCell);
                    cell1.BackgroundColor = UIColor.Clear;
                    cell1.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell1._Title.Text = "Last name";
                    cell1._TitleField.Text = AppDelegate.CurrentUser.LastName;
                    this.Controller.Lastname = cell1._TitleField;

                    return cell1;
                case 2:
                    var cell2 = (EditToasterTxtFieldCell)tableView.DequeueReusableCell(this.EditToasterTxtFieldCell);
                    cell2.BackgroundColor = UIColor.Clear;
                    cell2.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell2._Title.Text = "Phone number";
                    cell2._TitleField.KeyboardType = UIKeyboardType.PhonePad;
                    cell2._TitleField.Text = AppDelegate.CurrentUser.PhoneNumber.ToString();
                    this.Controller.PhoneNumber = cell2._TitleField;

                    return cell2;
                case 3:
                    var cell8 = (EditToasterTxtFieldCell)tableView.DequeueReusableCell(this.EditToasterTxtFieldCell);
                    cell8.BackgroundColor = UIColor.Clear;
                    cell8.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell8._Title.Text = "Headline";
                    cell8._TitleField.Text = !string.IsNullOrEmpty(this.Controller.Individual.Headline) ? this.Controller.Individual.Headline : string.Empty;
                    this.Controller.Headline = cell8._TitleField;

                    return cell8;
                case 4:
                    var cell3 = (EditToasterTxtFieldCell)tableView.DequeueReusableCell(this.EditToasterTxtFieldCell);
                    cell3.BackgroundColor = UIColor.Clear;
                    cell3.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell3._Title.Text = "Location";
                    cell3._TitleField.Text = !string.IsNullOrEmpty(this.Controller.Individual.HomeTown) ? this.Controller.Individual.HomeTown : string.Empty;
                    this.Controller.Location = cell3._TitleField;

                    return cell3;
                case 5:
                    var cell5 = (EditToasterSwitchCell)tableView.DequeueReusableCell(this.EditToasterSwitchCell);
                    cell5.BackgroundColor = UIColor.Clear;
                    cell5.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell5.Title.Text = "Female";
                    cell5.TitleSwitch.SetState(this.Controller.Individual.Female, true);
                    this.Controller.Female = cell5.TitleSwitch;

                    return cell5;

                case 6:
                    var cell6 = (EditToasterSwitchCell)tableView.DequeueReusableCell(this.EditToasterSwitchCell);
                    cell6.BackgroundColor = UIColor.Clear;
                    cell6.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell6.Title.Text = "Male";
                    cell6.TitleSwitch.SetState(this.Controller.Individual.Male, true);
                    this.Controller.Male = cell6.TitleSwitch;

                    return cell6;

                case 7:
                    var cell7 = (EditToasterSwitchCell)tableView.DequeueReusableCell(this.EditToasterSwitchCell);
                    cell7.BackgroundColor = UIColor.Clear;
                    cell7.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell7.Title.Text = "Other";
                    cell7.TitleSwitch.SetState(this.Controller.Individual.OtherSex, true);
                    this.Controller.OtherSex = cell7.TitleSwitch;

                    return cell7;

                //case 8:
                //    var cell9 = (EditToasterTxtFieldCell)tableView.DequeueReusableCell(this.EditToasterTxtFieldCell);
                //    cell9.BackgroundColor = UIColor.Clear;
                //    cell9.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                //    cell9._Title.Text = "Age";
                //    //cell9._TitleField.Text = !string.IsNullOrEmpty(this.Controller.Individual.Headline) ? this.Controller.Individual. : string.Empty;
                //    this.Controller.Age = cell9._TitleField;

                //    return cell9;
                //case 9:
                //    var cell10 = (EditToasterTxtFieldCell)tableView.DequeueReusableCell(this.EditToasterTxtFieldCell);
                //    cell10.BackgroundColor = UIColor.Clear;
                //    cell10.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                //    cell10._Title.Text = "Profile Description";
                //    cell10._TitleField.Text = !string.IsNullOrEmpty(this.Controller.Individual.ProfileDescription) ? this.Controller.Individual.ProfileDescription : string.Empty;
                //    this.Controller.Location = cell10._TitleField;

                //    return cell10;

                case 8:
                    var cell11 = (EditToasterSwitchCell)tableView.DequeueReusableCell(this.EditToasterSwitchCell);
                    cell11.BackgroundColor = UIColor.Clear;
                    cell11.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell11.Title.Text = "Private Account";
                    cell11.TitleSwitch.SetState(this.Controller.Individual.PrivateAccount, true);
                    this.Controller.PrivateAccount = cell11.TitleSwitch;

                    return cell11;

            }

            return new UITableViewCell();
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
            return 9;
        }

        #endregion

    }
}