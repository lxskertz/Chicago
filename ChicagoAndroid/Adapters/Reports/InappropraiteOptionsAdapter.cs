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
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.ChicagoAndroid.Activities.Reports;
using Tabs.Mobile.Shared.Models.Reports.InappropriateReports;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Reports
{
    public class InappropraiteOptionsAdapter : BaseAdapter
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<InappropriateReport.ReportReason, string> Reasons { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private InappropraiteOptionsActivity Owner { get; set; }

        #endregion

        #region Constructors

        public InappropraiteOptionsAdapter(InappropraiteOptionsActivity owner,
            Dictionary<InappropriateReport.ReportReason, string> reasons)
        {
            this.Owner = owner;
            this.Reasons = reasons;
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
                return this.Reasons.Count;
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
                convertView = LayoutInflater.FromContext(this.Owner).Inflate(Resource.Layout.BasicListviewItem, container, false);
            }

            var title = convertView.FindViewById<TextView>(Resource.Id.title);

            var item = this.Reasons.ElementAt(position);
            title.Text = string.IsNullOrEmpty(item.Value) ? "" : item.Value;            

            return convertView;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = this.Reasons.ElementAt(e.Position);
            this.Owner.ReportInappropriate(item.Key);
        }

        #endregion

    }
}