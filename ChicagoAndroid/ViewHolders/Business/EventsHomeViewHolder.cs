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
using Tabs.Mobile.ChicagoAndroid.Adapters.Business.Events;
using Tabs.Mobile.ChicagoAndroid.Helpers;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.ViewHolders.Business
{
    public class EventsHomeViewHolder : RecyclerView.ViewHolder
    {

        #region Properties 

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int> Listener { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public TextView Title { get; set; }

        /// <summary>
        /// Gets or sets wait time
        /// </summary>
        public TextView Description { get; set; }

        /// <summary>
        /// Gets or sets adapter
        /// </summary>
        private EventsHomeAdapter EventsHomeAdapter { get; set; }

        #endregion

        #region Constructors

        public EventsHomeViewHolder(EventsHomeAdapter eventsHomeAdapter, View itemView, Action<int> cardClick) : base(itemView)
        {
            this.EventsHomeAdapter = eventsHomeAdapter;
            this.Title = itemView.FindViewById<TextView>(Resource.Id.eventTitle);
            this.Description = itemView.FindViewById<TextView>(Resource.Id.eventDescription);

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

        /// <summary>
        /// 
        /// </summary>
        public void OnItemSelected()
        {
            ItemView.SetBackgroundColor(Android.Graphics.Color.LightGray);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnItemCleared()
        {
            ItemView.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }

        #endregion

    }
}