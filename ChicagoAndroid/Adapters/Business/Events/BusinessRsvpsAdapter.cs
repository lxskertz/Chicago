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
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.ChicagoAndroid.Activities.Businesses;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Business.Events
{
    public class BusinessRsvpsAdapter : BaseAdapter
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<Rsvp> Rsvps { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private BusinessRsvpsActivity BusinessRsvpsActivity { get; set; }

        #endregion

        #region Constructors

        public BusinessRsvpsAdapter(BusinessRsvpsActivity businessRsvpsActivity,
            List<Rsvp> rsvps)
        {
            this.BusinessRsvpsActivity = businessRsvpsActivity;
            this.Rsvps = rsvps;
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
                return this.Rsvps.Count;
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
                convertView = LayoutInflater.FromContext(this.BusinessRsvpsActivity).Inflate(Resource.Layout.BusinessRsvpListItem, container, false);
            }

            var title = convertView.FindViewById<TextView>(Resource.Id.title);
            var subTitle = convertView.FindViewById<TextView>(Resource.Id.subTitle);

            var item = this.Rsvps.ElementAt(position);

            if (item != null)
            {
                var fname = string.IsNullOrEmpty(item.FirstName) ? "" : item.FirstName;
                var lname = string.IsNullOrEmpty(item.LastName) ? "" : item.LastName;
                title.Text = fname + " " + lname;
                var going = item.Going ? AppText.Yes : AppText.No;
                subTitle.Text = "Going: " + going;
            }

            return convertView;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //var item = this.Drinks.ElementAt(e.Position);
            //Intent activity = new Intent(this.BusinessDrinksFragment.HomeContext, typeof(Activities.Drinks.AddEditDrinkActivity));
            //activity.PutExtra("ScreenActionType", (int)Activities.Drinks.AddEditDrinkActivity.ActionType.Edit);
            //activity.PutExtra("BusinessDrink", JsonConvert.SerializeObject(item));
            //this.BusinessDrinksFragment.HomeContext.StartActivity(activity);
        }

        #endregion

    }
}