using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models.Events;
using Xamarin.Essentials;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business.Events
{
    public class EventsInfoDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        private NSString EventInfoCell = new NSString("EventInfoCell"); 

        private NSString EventDescriptionCell = new NSString("EventDescriptionCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public BusinessEvents BusinessEvents { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private EventInfoController Controller { get; set; }

        #endregion

        #region Constructors

        public EventsInfoDataSource(EventInfoController controller, BusinessEvents businessEvents)
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
            if (this.BusinessEvents != null)
            {
                switch (indexPath.Row)
                {
                    case 0:
                        var cell0 = (EventInfoCell)tableView.DequeueReusableCell(this.EventInfoCell);
                        cell0._Title.Text = string.IsNullOrEmpty(this.BusinessEvents.Venue) ? "" : this.BusinessEvents.Venue;
                        var addy = string.IsNullOrEmpty(this.BusinessEvents.StreetAddress) ? "" : this.BusinessEvents.StreetAddress + ", ";
                        var city = string.IsNullOrEmpty(this.BusinessEvents.City) ? "" : this.BusinessEvents.City + ", ";
                        var state = string.IsNullOrEmpty(this.BusinessEvents.State) ? "" : this.BusinessEvents.State + " ";
                        var zipcode = string.IsNullOrEmpty(this.BusinessEvents.ZipCode) ? "" : this.BusinessEvents.ZipCode;
                        var address = addy + city + state + zipcode;
                        cell0._SubTitle.Text = address;
                        cell0._ImageIcon.ClipsToBounds = true;
                        cell0._ImageIcon.Image = UIImage.FromFile("twotone_location24pt");
                        return cell0;
                    case 1:
                        var cell1 = (EventInfoCell)tableView.DequeueReusableCell(this.EventInfoCell);
                        var startDate = this.BusinessEvents.StartDateTime == null ? "" : this.BusinessEvents.StartDateTime.Value.ToLongDateString();
                        var startTime = this.BusinessEvents.StartDateTime == null ? "" : this.BusinessEvents.StartDateTime.Value.ToShortTimeString() + " - ";
                        cell1._Title.Text = startDate + " " + startTime;

                        var endDate = this.BusinessEvents.EndDateTime == null ? "" : this.BusinessEvents.EndDateTime.Value.ToLongDateString();
                        var endTime = this.BusinessEvents.EndDateTime == null ? "" : this.BusinessEvents.EndDateTime.Value.ToShortTimeString();
                      
                        cell1._SubTitle.Text = endDate + " " + endTime; ;
                        cell1._ImageIcon.Image = UIImage.FromFile("Events/event");
                        return cell1;
                    case 2:
                        var cell2 = (EventDescriptionCell)tableView.DequeueReusableCell(this.EventDescriptionCell);
                        cell2._EventDescription.Text = string.IsNullOrEmpty(this.BusinessEvents.EventDescription) ? "" : this.BusinessEvents.EventDescription;
                        return cell2;
                }
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
        public async override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);

            if (indexPath.Row == 0)
            {
                try
                {
                    var addy = string.IsNullOrEmpty(this.BusinessEvents.StreetAddress) ? "" : this.BusinessEvents.StreetAddress + ", ";
                    var city = string.IsNullOrEmpty(this.BusinessEvents.City) ? "" : this.BusinessEvents.City + ", ";
                    var state = string.IsNullOrEmpty(this.BusinessEvents.State) ? "" : this.BusinessEvents.State + " ";
                    var zipcode = string.IsNullOrEmpty(this.BusinessEvents.ZipCode) ? "" : this.BusinessEvents.ZipCode;
                    var bName = this.Controller.BusinessInfo != null ? this.Controller.BusinessInfo.BusinessName : "";
                    var placemark = new Placemark
                    {
                        CountryName = "United States",
                        AdminArea = state,
                        Thoroughfare = addy,
                        Locality = city,
                        PostalCode = zipcode
                    };
                    var options = new MapLaunchOptions { Name = bName, NavigationMode = NavigationMode.Default };

                    await Map.OpenAsync(placemark, options);
                } catch(Exception ex)
                {
                    var a = ex;
                }
            }
        }

        /// <summary>
        /// return num of rows that will be in the section
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 3;
        }

        #endregion

    }
}