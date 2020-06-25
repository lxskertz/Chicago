using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.Widget;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.ChicagoAndroid.Adapters.Business;
using Tabs.Mobile.ChicagoAndroid.Helpers;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.ViewHolders.Business
{
    public class BusinessesViewHolder : RecyclerView.ViewHolder
    {

        #region Properties 

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int> Listener { get; set; }

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int, BusinessesViewHolder> CheckInBtnListener { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public TextView BusinessName { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public TextView BusinessType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ImageView BusinessLogo { get; set; }

        public Button CheckInBtn { get; set; }

        /// <summary>
        /// Gets or sets adapter
        /// </summary>
        private BusinessesAdapter BusinessesAdapter { get; set; }

        #endregion

        #region Constructors

        public BusinessesViewHolder(BusinessesAdapter businessesAdapter, View itemView,
            Action<int> cardClick, Action<int, BusinessesViewHolder> checkBtnClick) : base(itemView)
        {
            this.BusinessesAdapter = businessesAdapter;
            this.BusinessName = itemView.FindViewById<TextView>(Resource.Id.businessName);
            this.BusinessType = itemView.FindViewById<TextView>(Resource.Id.businessType);
            this.BusinessLogo = itemView.FindViewById<ImageView>(Resource.Id.businessLogo);
            this.CheckInBtn = itemView.FindViewById<Button>(Resource.Id.checkIn);

            Listener = cardClick;
            itemView.Click += OnClick;

            CheckInBtnListener = checkBtnClick;
            this.CheckInBtn.Click += OnCheckInBtnClick;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called on card click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClick(object sender, EventArgs e)
        {
            int position = base.AdapterPosition;
            if (position != RecyclerView.NoPosition)
            {
                Listener(position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCheckInBtnClick(object sender, EventArgs e)
        {
            int position = base.AdapterPosition;
            if (position != RecyclerView.NoPosition)
            {
                CheckInBtnListener(position, this);
            }
        }

        #endregion

    }
}