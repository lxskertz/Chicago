using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.ChicagoAndroid.Activities.Individuals.Events;
using Xamarin.Essentials;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Individuals.Events
{
    public class EventInfoAdapter : BaseAdapter
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public BusinessEvents BusinessEvents { get; set; }

        /// <summary>
        /// Gets or sets the owner
        /// </summary>
        private EventInfoActivity MyContext { get; set; }

        #endregion

        #region Constructors

        public EventInfoAdapter(EventInfoActivity myContext, BusinessEvents businessEvents)
        {
            this.MyContext = myContext;
            this.BusinessEvents = businessEvents;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get number of item to be displayed
        /// </summary>
        public override int Count
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// Gets item ID
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
		public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Get the type of View that will be created for the specified item.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }

        /// <summary>
        /// Get item at specified position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Java.Lang.Object GetItem(int position)
        {
            return "";
        }

        /// <summary>
        /// Gets view... list cells
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup container)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(this.MyContext).Inflate(Resource.Layout.EventInfoListItem, container, false);
            }

            if (this.BusinessEvents != null)
            {
                var title = convertView.FindViewById<TextView>(Resource.Id.title);
                var subTitle = convertView.FindViewById<TextView>(Resource.Id.subTitle);
                var logo = convertView.FindViewById<ImageView>(Resource.Id.eventLogo);

                switch (position)
                {
                    case 0:
                        title.Text = string.IsNullOrEmpty(this.BusinessEvents.Venue) ? "" : this.BusinessEvents.Venue;
                        var addy = string.IsNullOrEmpty(this.BusinessEvents.StreetAddress) ? "" : this.BusinessEvents.StreetAddress + ", ";
                        var city = string.IsNullOrEmpty(this.BusinessEvents.City) ? "" : this.BusinessEvents.City + ", ";
                        var state = string.IsNullOrEmpty(this.BusinessEvents.State) ? "" : this.BusinessEvents.State + " ";
                        var zipcode = string.IsNullOrEmpty(this.BusinessEvents.ZipCode) ? "" : this.BusinessEvents.ZipCode;
                        var address = addy + city + state + zipcode;
                        subTitle.Text = address;
                        logo.Visibility = ViewStates.Visible;
                        subTitle.Visibility = ViewStates.Visible;
                        logo.SetImageResource(Resource.Drawable.location_on_black_24);
                        break;
                    case 1:
                        var startDate = this.BusinessEvents.StartDateTime == null ? "" : this.BusinessEvents.StartDateTime.Value.ToLongDateString();
                        var startTime = this.BusinessEvents.StartDateTime == null ? "" : this.BusinessEvents.StartDateTime.Value.ToShortTimeString() + " - ";
                        title.Text = startDate + " " + startTime;

                        var endDate = this.BusinessEvents.EndDateTime == null ? "" : this.BusinessEvents.EndDateTime.Value.ToLongDateString();
                        var endTime = this.BusinessEvents.EndDateTime == null ? "" : this.BusinessEvents.EndDateTime.Value.ToShortTimeString();

                        subTitle.Text = endDate + " " + endTime; ;
                        logo.Visibility = ViewStates.Visible;
                        subTitle.Visibility = ViewStates.Visible;
                        logo.SetImageResource(Resource.Drawable.baseline_event_black_24);
                        break;
                    case 2:
                        title.Text = string.IsNullOrEmpty(this.BusinessEvents.EventDescription) ? "" : this.BusinessEvents.EventDescription;
                        logo.Visibility = ViewStates.Gone;
                        subTitle.Visibility = ViewStates.Gone;
                        break;
                }
            }

            return convertView;
        }

        public async void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
           if(e.Position == 1)
            {
                try
                {
                    var addy = string.IsNullOrEmpty(this.BusinessEvents.StreetAddress) ? "" : this.BusinessEvents.StreetAddress + ", ";
                    var city = string.IsNullOrEmpty(this.BusinessEvents.City) ? "" : this.BusinessEvents.City + ", ";
                    var state = string.IsNullOrEmpty(this.BusinessEvents.State) ? "" : this.BusinessEvents.State + " ";
                    var zipcode = string.IsNullOrEmpty(this.BusinessEvents.ZipCode) ? "" : this.BusinessEvents.ZipCode;
                    var bName = this.MyContext.BusinessInfo != null ? this.MyContext.BusinessInfo.BusinessName : "";
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
                }
                catch (Exception ex)
                {
                    var a = ex;
                }
            }
        }

        #endregion

    }
}