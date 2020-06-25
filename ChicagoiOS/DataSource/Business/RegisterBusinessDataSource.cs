using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business
{
    public class RegisterBusinessDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString BusinessInfoCellOne = new NSString("BusinessInfoCellOne"); 

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString BusinessInfoCellTwo = new NSString("BusinessInfoCellTwo");

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString BusinessInfoCellThree = new NSString("BusinessInfoCellThree"); 

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private BusinessRegistrationController Controller { get; set; }

        #endregion

        #region Constructors

        public RegisterBusinessDataSource(BusinessRegistrationController controller)
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
                    var cell1 = (BusinessInfoCellOne)tableView.DequeueReusableCell(this.BusinessInfoCellOne);
                    cell1.BackgroundColor = UIColor.Clear;
                    cell1.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell1._Title.Text = "Business Name";
                    this.Controller.BusinessName = cell1._TitleField;
                    cell1._TitleField.Text = this.Controller.EditProfile ?
                        string.IsNullOrEmpty(this.Controller.BusinessInfo.BusinessName) ? "" : this.Controller.BusinessInfo.BusinessName : "";

                    return cell1;
                case 11:
                    var cell12 = (BusinessInfoCellOne)tableView.DequeueReusableCell(this.BusinessInfoCellOne);
                    cell12.BackgroundColor = UIColor.Clear;
                    cell12.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell12._Title.Text = "Phone Number";
                    cell12._TitleField.KeyboardType = UIKeyboardType.PhonePad;
                    this.Controller.PhoneNumber = cell12._TitleField;
                    cell12._TitleField.Text = this.Controller.EditProfile ? this.Controller.BusinessInfo.PhoneNumber.ToString() : "";

                    return cell12;
                //case 12:
                //    var cell13 = (BusinessInfoCellOne)tableView.DequeueReusableCell(this.BusinessInfoCellOne);
                //    cell13.BackgroundColor = UIColor.Clear;
                //    cell13.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                //    cell13._Title.Text = "Sales Tax Rate";
                //    cell13._TitleField.KeyboardType = UIKeyboardType.DecimalPad;
                //    this.Controller.PhoneNumber = cell13._TitleField;
                //    cell13._TitleField.Text = this.Controller.EditProfile ? this.Controller.BusinessInfo.SalesTaxRate.ToString() : "";

                //    return cell13;
                case 1:
                    var cell2 = (BusinessInfoCellOne)tableView.DequeueReusableCell(this.BusinessInfoCellOne);
                    cell2.BackgroundColor = UIColor.Clear;
                    cell2.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell2._Title.Text = "Address";
                    this.Controller.Address = cell2._TitleField;
                    cell2._TitleField.Text = this.Controller.EditProfile ?
                       string.IsNullOrEmpty(this.Controller.AddressInfo.StreetAddress) ? "" : this.Controller.AddressInfo.StreetAddress : "";
                    cell2.IndexPath = indexPath;
                    this.Controller.Address.EditingDidBegin += delegate {
                        tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 300));
                        tableView.ScrollToRow(cell2.IndexPath, UITableViewScrollPosition.Middle, true);
                    };
                    this.Controller.Address.EditingDidEnd += delegate {
                        tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 0));
                    };

                    return cell2;             
                case 2:
                    var cell3 = (BusinessInfoCellOne)tableView.DequeueReusableCell(this.BusinessInfoCellOne);
                    cell3.BackgroundColor = UIColor.Clear;
                    cell3.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell3._Title.Text = "City";
                    this.Controller.City = cell3._TitleField;
                    cell3._TitleField.Text = this.Controller.EditProfile ?
                      string.IsNullOrEmpty(this.Controller.AddressInfo.City) ? "" : this.Controller.AddressInfo.City : "";
                    cell3.IndexPath = indexPath;
                    this.Controller.City.EditingDidBegin += delegate {
                        tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 300));
                        tableView.ScrollToRow(cell3.IndexPath, UITableViewScrollPosition.Middle, true);
                    };
                    this.Controller.City.EditingDidEnd += delegate {
                        tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 0));
                    };

                    return cell3;
                case 3:
                    var cell4 = (BusinessInfoCellOne)tableView.DequeueReusableCell(this.BusinessInfoCellOne);
                    cell4.BackgroundColor = UIColor.Clear;
                    cell4.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell4._Title.Text = "State";
                    this.Controller.State = cell4._TitleField;
                    cell4._TitleField.Text = this.Controller.EditProfile ?
                      string.IsNullOrEmpty(this.Controller.AddressInfo.State) ? "" : this.Controller.AddressInfo.State : "";
                    cell4.IndexPath = indexPath;
                    this.Controller.State.EditingDidBegin += delegate {
                        tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 300));
                        tableView.ScrollToRow(cell4.IndexPath, UITableViewScrollPosition.Middle, true);
                    };
                    this.Controller.State.EditingDidEnd += delegate {
                        tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 0));
                    };
                    return cell4;
                case 4:
                    var cell5 = (BusinessInfoCellOne)tableView.DequeueReusableCell(this.BusinessInfoCellOne);
                    cell5.BackgroundColor = UIColor.Clear;
                    cell5.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell5._Title.Text = "ZipCode";
                    this.Controller.Zipcode = cell5._TitleField;
                    cell5._TitleField.Text = this.Controller.EditProfile ?
                      string.IsNullOrEmpty(this.Controller.AddressInfo.ZipCode) ? "" : this.Controller.AddressInfo.ZipCode : "";
                    //cell5._TitleField.Text = this.Controller.HideAccountPicture ? this.Controller.AddressInfo.ZipCode : "";
                    cell5.IndexPath = indexPath;
                    this.Controller.Zipcode.EditingDidBegin += delegate {
                        tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 300));
                        tableView.ScrollToRow(cell5.IndexPath, UITableViewScrollPosition.Middle, true);
                    };
                    this.Controller.Zipcode.EditingDidEnd += delegate {
                        tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 0));
                    };
                    return cell5;
                case 5:
                    var cell6 = (BusinessInfoCellThree)tableView.DequeueReusableCell(this.BusinessInfoCellThree);
                    cell6.TextLabel.Text = "Select Business Type";
                    cell6.UserInteractionEnabled = false;

                    return cell6;

                case 6:
                    var cell7 = (BusinessInfoCellTwo)tableView.DequeueReusableCell(this.BusinessInfoCellTwo);
                    cell7.BackgroundColor = UIColor.Clear;
                    cell7.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell7._Title.Text = "Bar";
                    var state = this.Controller.EditProfile ? this.Controller.BusinessTypes.Bar : false;
                    cell7._TitleSwitch.SetState(state, true);
                    this.Controller.Bar = cell7._TitleSwitch;

                    return cell7;

                case 10:
                    var cell11 = (BusinessInfoCellTwo)tableView.DequeueReusableCell(this.BusinessInfoCellTwo);
                    cell11.BackgroundColor = UIColor.Clear;
                    cell11.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell11._Title.Text = "Lounge";
                    var Lstate = this.Controller.EditProfile ? this.Controller.BusinessTypes.Lounge : false;
                    cell11._TitleSwitch.SetState(Lstate, true);
                    this.Controller.Lounge = cell11._TitleSwitch;

                    return cell11;

                case 7:
                    var cell8 = (BusinessInfoCellTwo)tableView.DequeueReusableCell(this.BusinessInfoCellTwo);
                    cell8.BackgroundColor = UIColor.Clear;
                    cell8.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell8._Title.Text = "Club";
                    var Cstate = this.Controller.EditProfile ? this.Controller.BusinessTypes.Club : false;
                    cell8._TitleSwitch.SetState(Cstate, true);
                    this.Controller.Club = cell8._TitleSwitch;

                    return cell8;
                case 8:
                    var cell9 = (BusinessInfoCellTwo)tableView.DequeueReusableCell(this.BusinessInfoCellTwo);
                    cell9.BackgroundColor = UIColor.Clear;
                    cell9.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell9._Title.Text = "Restaurant";
                    var Rstate = this.Controller.EditProfile ? this.Controller.BusinessTypes.Restaurant : false;
                    cell9._TitleSwitch.SetState(Rstate, true);
                    this.Controller.Restaurant = cell9._TitleSwitch;

                    return cell9;
                case 9:
                    var cell10 = (BusinessInfoCellTwo)tableView.DequeueReusableCell(this.BusinessInfoCellTwo);
                    cell10.BackgroundColor = UIColor.Clear;
                    cell10.SelectionStyle = UITableViewCellSelectionStyle.Blue;
                    cell10._Title.Text = "Other";
                    var Ostate = this.Controller.EditProfile ? this.Controller.BusinessTypes.Other : false;
                    cell10._TitleSwitch.SetState(Ostate, true);
                    this.Controller.Other = cell10._TitleSwitch;

                    return cell10;
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
            return 12;
        }

        #endregion

    }
}