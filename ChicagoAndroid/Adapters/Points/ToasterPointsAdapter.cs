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
using Tabs.Mobile.Shared.Models.Points;
using Tabs.Mobile.ChicagoAndroid.Activities.Points;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Points
{
    public class ToasterPointsAdapter : BaseAdapter
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<Point> Points { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private ToasterPointsActivity ToasterPointsActivity { get; set; }

        #endregion

        #region Constructors

        public ToasterPointsAdapter(ToasterPointsActivity context, List<Point> points)
        {
            this.ToasterPointsActivity = context;
            this.Points = points;
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
                return this.Points.Count;
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
                convertView = LayoutInflater.FromContext(this.ToasterPointsActivity).Inflate(Resource.Layout.PointsListItem, container, false);
            }

            var title = convertView.FindViewById<TextView>(Resource.Id.title);
            var subTitle = convertView.FindViewById<TextView>(Resource.Id.subTitle);

            var item = this.Points.ElementAt(position);

            if (item != null)
            {
                title.Text = item.EarnedDate.HasValue ? item.EarnedDate.Value.ToLongDateString() : "";
                subTitle.Text = item.PointAmount.ToString() + " Points";
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
        }

        #endregion

    }
}