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
    public class BusinessPhotoViewHolder : RecyclerView.ViewHolder
    {

        #region Properties 

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int> Listener { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public ImageView Photo { get; set; }

        /// <summary>
        /// Gets or sets adapter
        /// </summary>
        private BusinessPhotoAdaper BusinessPhotoAdaper { get; set; }

        #endregion

        #region Constructors

        public BusinessPhotoViewHolder(BusinessPhotoAdaper businessPhotoAdaper, View itemView, Action<int> cardClick) : base(itemView)
        {
            this.BusinessPhotoAdaper = businessPhotoAdaper;
            this.Photo = itemView.FindViewById<ImageView>(Resource.Id.photo);

            Listener = cardClick;
            itemView.Click += OnClick;
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

        #endregion

    }
}