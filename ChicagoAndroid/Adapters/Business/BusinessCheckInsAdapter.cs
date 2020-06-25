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
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.ChicagoAndroid.Activities.Businesses;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Business;
using Newtonsoft.Json;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Business
{
    public class BusinessCheckInsAdapter : BaseAdapter
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<CheckIn> CheckIns { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public BusinessCheckInsActivity BusinessCheckInsActivity { get; set; }

        #endregion

        #region Constructors

        public BusinessCheckInsAdapter(BusinessCheckInsActivity context, List<CheckIn> checkIns)
        {
            this.BusinessCheckInsActivity = context;
            this.CheckIns = checkIns;
        }

        #endregion

        #region Methods

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            BusinessCheckInsViewHolder holder = null;

            if (view != null)
            {
                holder = view.Tag as BusinessCheckInsViewHolder;
            }

            if (holder == null)
            {
                view = LayoutInflater.FromContext(this.BusinessCheckInsActivity).Inflate(Resource.Layout.BusinessCheckInsListItem, parent, false);
                holder = new BusinessCheckInsViewHolder(this, view, OnActionBtnListener);
                view.Tag = holder;
            }

            var item = this.CheckIns.ElementAt(position);

            if (item != null)
            {
                holder.SendDrinkButon.SetTag(Resource.Id.sendDrinkBtn, position);
                var fname = string.IsNullOrEmpty(item.FirstName) ? "" : item.FirstName;
                var lname = string.IsNullOrEmpty(item.LastName) ? "" : item.LastName;
                holder.Title.Text = fname + " " + lname;
                holder.Subtitle.Text = item.CheckInDate.HasValue ? item.CheckInDate.Value.ToLongDateString() : "";
            }

            return view;
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

        public override int Count
        {
            get
            {
                return this.CheckIns.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="viewHolder"></param>
        public void OnActionBtnListener(int position, BusinessCheckInsViewHolder viewHolder)
        {
            var item = this.CheckIns.ElementAt(position);
            Intent activity = new Intent(this.BusinessCheckInsActivity, typeof(Activities.Drinks.SendDrinkActivity));
            activity.PutExtra("CheckInItem", JsonConvert.SerializeObject(item));
            activity.PutExtra("FromBusiness", true);
            this.BusinessCheckInsActivity.StartActivity(activity);
        }
    }

    #endregion

}