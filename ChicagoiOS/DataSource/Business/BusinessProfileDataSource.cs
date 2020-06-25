using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business
{
    public class BusinessProfileDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString BusinessProfileCell = new NSString("BusinessProfileCell");

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Address AddressInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessTypes BusinessTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Shared.Models.Businesses.Business BusinessInfo { get; set; }

        public BusinessSearch BusinessSearchInfo { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private BusinessProfileController Controller { get; set; }

        #endregion

        #region Constructors

        public BusinessProfileDataSource(BusinessProfileController controller, Shared.Models.Businesses.Business business,
            Address address, BusinessTypes bizTypes)
        {
            this.Controller = controller;
            this.BusinessInfo = business;
            this.AddressInfo = address;
            this.BusinessTypes = bizTypes;
        }

        public BusinessProfileDataSource(BusinessProfileController controller, BusinessSearch businessSearchInfo)
        {
            this.Controller = controller;
            this.BusinessSearchInfo = businessSearchInfo;
        }

        #endregion

        #region Methods

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
            return this.BusinessInfo != null ? 4 : 0;
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
            if (this.BusinessSearchInfo != null)
            {
                switch (indexPath.Row)
                {
                    case 0:
                        var cell1 = (BusinessProfileCell)tableView.DequeueReusableCell(this.BusinessProfileCell);
                        cell1.TextLabel.Text = "Business Name";
                        cell1.DetailTextLabel.Text = string.IsNullOrEmpty(this.BusinessSearchInfo.BusinessName) ? "" : this.BusinessSearchInfo.BusinessName;

                        return cell1;
                    case 1:
                        var cell2 = (BusinessProfileCell)tableView.DequeueReusableCell(this.BusinessProfileCell);
                        cell2.TextLabel.Text = "Address";
                        var addy = string.IsNullOrEmpty(BusinessSearchInfo.StreetAddress) ? "" : BusinessSearchInfo.StreetAddress + ", ";
                        var city = string.IsNullOrEmpty(BusinessSearchInfo.City) ? "" : BusinessSearchInfo.City + ", ";
                        var state = string.IsNullOrEmpty(BusinessSearchInfo.State) ? "" : BusinessSearchInfo.State + " ";
                        var zipcode = string.IsNullOrEmpty(BusinessSearchInfo.ZipCode) ? "" : BusinessSearchInfo.ZipCode;
                        var address = addy + city + state + zipcode;
                        cell2.DetailTextLabel.Text = address;

                        return cell2;
                    case 2:
                        var cell3 = (BusinessProfileCell)tableView.DequeueReusableCell(this.BusinessProfileCell);
                        cell3.TextLabel.Text = "Phone";
                        cell3.DetailTextLabel.Text = this.BusinessSearchInfo.PhoneNumber.ToString();

                        return cell3;
                    case 3:
                        var cell4 = (BusinessProfileCell)tableView.DequeueReusableCell(this.BusinessProfileCell);
                        cell4.TextLabel.Text = "Business Type";
                        var bar = this.BusinessSearchInfo.Bar ? AppText.Bar + ", " : "";
                        var club = this.BusinessSearchInfo.Club ? AppText.Club + ", " : "";
                        var lounge = this.BusinessSearchInfo.Lounge ? AppText.Lounge + ", " : "";
                        var restaurant = this.BusinessSearchInfo.Restaurant ? AppText.Restaurant + ", " : "";
                        var other = this.BusinessSearchInfo.Other ? AppText.Other : "";
                        cell4.DetailTextLabel.Text = bar + club + lounge + restaurant + other;

                        return cell4;
                }
            }
            else
            {
                switch (indexPath.Row)
                {
                    case 0:
                        var cell1 = (BusinessProfileCell)tableView.DequeueReusableCell(this.BusinessProfileCell);
                        cell1.TextLabel.Text = "Business Name";
                        cell1.DetailTextLabel.Text = string.IsNullOrEmpty(this.BusinessInfo.BusinessName) ? "" : this.BusinessInfo.BusinessName;

                        return cell1;
                    case 1:
                        var cell2 = (BusinessProfileCell)tableView.DequeueReusableCell(this.BusinessProfileCell);
                        cell2.TextLabel.Text = "Address";
                        var addy = string.IsNullOrEmpty(AddressInfo.StreetAddress) ? "" : AddressInfo.StreetAddress + ", ";
                        var city = string.IsNullOrEmpty(AddressInfo.City) ? "" : AddressInfo.City + ", ";
                        var state = string.IsNullOrEmpty(AddressInfo.State) ? "" : AddressInfo.State + " ";
                        var zipcode = string.IsNullOrEmpty(AddressInfo.ZipCode) ? "" : AddressInfo.ZipCode;
                        var address = addy + city + state + zipcode;
                        cell2.DetailTextLabel.Text = address;

                        return cell2;
                    case 2:
                        var cell3 = (BusinessProfileCell)tableView.DequeueReusableCell(this.BusinessProfileCell);
                        cell3.TextLabel.Text = "Phone";
                        cell3.DetailTextLabel.Text = this.BusinessInfo.PhoneNumber.ToString();

                        return cell3;
                    case 3:
                        var cell4 = (BusinessProfileCell)tableView.DequeueReusableCell(this.BusinessProfileCell);
                        cell4.TextLabel.Text = "Business Type";
                        var bar = this.BusinessTypes.Bar ? AppText.Bar + ", " : "";
                        var club = this.BusinessTypes.Club ? AppText.Club + ", " : "";
                        var lounge = this.BusinessTypes.Lounge ? AppText.Lounge + ", " : "";
                        var restaurant = this.BusinessTypes.Restaurant ? AppText.Restaurant + ", " : "";
                        var other = this.BusinessTypes.Other ? AppText.Other : "";
                        cell4.DetailTextLabel.Text = bar + club + lounge + restaurant + other;

                        return cell4;
                }
            }

            return new UITableViewCell();
        }

        #endregion

    }
} 