using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS.DataSource
{
    public class SignUpDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the SignUpPropCell
        /// </summary>
        private NSString SignUpItemCell = new NSString("SignUpItemCell"); 

        /// <summary>
        /// 
        /// </summary>
        private NSString UserTypeCell = new NSString("UserTypeCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public SignUpController Controller { get; set; }

        #endregion

        #region Constructors

        public SignUpDataSource(SignUpController controller)
        {
            this.Controller = controller;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called by the TableView to get the row height for a given cell
        /// </summary>
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 44f;
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
            switch (indexPath.Row)
            {
                case 0:
                    var cell0 = (SignUpItemCell)tableView.DequeueReusableCell(this.SignUpItemCell);
                    cell0.BackgroundColor = UIColor.Clear;
                    cell0.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell0._Title.Text = "First name";

                    cell0._TitleField.ShouldChangeCharacters = (textField, range, replacementString) => {
                        var newLength = textField.Text.Length + replacementString.Length - range.Length;
                        return newLength <= 128;
                    };

                    this.Controller.Firstname = cell0._TitleField;

                    return cell0;
                case 1:
                    var cell1 = (SignUpItemCell)tableView.DequeueReusableCell(this.SignUpItemCell);
                    cell1.BackgroundColor = UIColor.Clear;
                    cell1.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell1._Title.Text = "Last name";

                    cell1._TitleField.ShouldChangeCharacters = (textField, range, replacementString) => {
                        var newLength = textField.Text.Length + replacementString.Length - range.Length;
                        return newLength <= 128;
                    };

                    this.Controller.Lastname = cell1._TitleField;

                    return cell1;
                case 2:
                    var cell2 = (SignUpItemCell)tableView.DequeueReusableCell(this.SignUpItemCell);
                    cell2.BackgroundColor = UIColor.Clear;
                    cell2.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell2._Title.Text = "Email";
                    cell2._TitleField.KeyboardType = UIKeyboardType.EmailAddress;

                    cell2._TitleField.ShouldChangeCharacters = (textField, range, replacementString) => {
                        var newLength = textField.Text.Length + replacementString.Length - range.Length;
                        return newLength <= 256;
                    };

                    this.Controller.Email = cell2._TitleField;

                    return cell2;
                case 3:
                    var cell8 = (SignUpItemCell)tableView.DequeueReusableCell(this.SignUpItemCell);
                    cell8.BackgroundColor = UIColor.Clear;
                    cell8.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell8._Title.Text = "Username";

                    cell8._TitleField.ShouldChangeCharacters = (textField, range, replacementString) => {
                        var newLength = textField.Text.Length + replacementString.Length - range.Length;
                        return newLength <= 256;
                    };

                    this.Controller.Username = cell8._TitleField;

                    return cell8;
                case 4:
                    var cell3 = (SignUpItemCell)tableView.DequeueReusableCell(this.SignUpItemCell);
                    cell3.BackgroundColor = UIColor.Clear;
                    cell3.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell3._Title.Text = "Password";
                    cell3._TitleField.SecureTextEntry = true;

                    cell3._TitleField.ShouldChangeCharacters = (textField, range, replacementString) => {
                        var newLength = textField.Text.Length + replacementString.Length - range.Length;
                        return newLength <= 50;
                    };

                    this.Controller.Password = cell3._TitleField;

                    return cell3;
                case 5:
                    var cell4 = (SignUpItemCell)tableView.DequeueReusableCell(this.SignUpItemCell);
                    cell4.BackgroundColor = UIColor.Clear;
                    cell4.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell4._Title.Text = "Re-Enter Password";
                    cell4._TitleField.SecureTextEntry = true;

                    cell4._TitleField.ShouldChangeCharacters = (textField, range, replacementString) => {
                        var newLength = textField.Text.Length + replacementString.Length - range.Length;
                        return newLength <= 50;
                    };

                    this.Controller.ReEnterPAssword = cell4._TitleField;

                    return cell4;

                case 6:
                    var cell6 = (UserTypeCell)tableView.DequeueReusableCell(this.UserTypeCell);
                    cell6.BackgroundColor = UIColor.Clear;
                    cell6.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell6._Title.Text = "Individual";
                    cell6._TitleSwitch.SetState(false, true);
                    this.Controller.Individual = cell6._TitleSwitch;

                    return cell6;

                case 7:
                    var cell7 = (UserTypeCell)tableView.DequeueReusableCell(this.UserTypeCell);
                    cell7.BackgroundColor = UIColor.Clear;
                    cell7.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell7._Title.Text = "Business";
                    cell7._TitleSwitch.SetState(false, true);
                    this.Controller.Business = cell7._TitleSwitch;
                    cell7.UserInteractionEnabled = false;

                    return cell7;
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
            return 8;
        }

        #endregion

    }
}