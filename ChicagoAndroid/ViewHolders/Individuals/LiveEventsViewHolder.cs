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
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals.Events;
using Tabs.Mobile.ChicagoAndroid.Helpers;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals
{
    public class LiveEventsViewHolder : RecyclerView.ViewHolder
    {

        #region Properties 

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int> Listener { get; set; }

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int, LiveEventsViewHolder> LikeBtnListener { get; set; }

        private Action<int, LiveEventsViewHolder> CheckInBtnListener { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public TextView Title { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public TextView EventDate { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public TextView Venue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ImageView EventLogo { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public TextView LikeCount { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public ImageButton LikeBtn { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public ImageButton ShareBtn { get; set; }

        public Button CheckInBtn { get; set; }

        /// <summary>
        /// Gets or sets adapter
        /// </summary>
        private LiveEventsAdapter LiveEventsAdapter { get; set; }

        #endregion

        #region Constructors

        public LiveEventsViewHolder(LiveEventsAdapter liveEventsAdapter, View itemView,
            Action<int> cardClick, Action<int, LiveEventsViewHolder> btnClick,
            Action<int, LiveEventsViewHolder> checkBtnClick) : base(itemView)
        {
            this.LiveEventsAdapter = liveEventsAdapter;
            this.Title = itemView.FindViewById<TextView>(Resource.Id.eventTitle);
            this.Venue = itemView.FindViewById<TextView>(Resource.Id.eventVenue);
            this.EventDate = itemView.FindViewById<TextView>(Resource.Id.eventDate);
            this.EventLogo = itemView.FindViewById<ImageView>(Resource.Id.eventLogo);
            this.ShareBtn = itemView.FindViewById<ImageButton>(Resource.Id.shareBtn);
            this.LikeCount = itemView.FindViewById<TextView>(Resource.Id.likeCount);
            this.LikeBtn = itemView.FindViewById<ImageButton>(Resource.Id.likeBtn);
            this.CheckInBtn = itemView.FindViewById<Button>(Resource.Id.checkIn);

            Listener = cardClick;
            itemView.Click += OnClick;

            LikeBtnListener = btnClick;
            this.LikeBtn.Click += OnLikeBtnClick;

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
        private void OnLikeBtnClick(object sender, EventArgs e)
        {
            int position = base.AdapterPosition;
            if (position != RecyclerView.NoPosition)
            {
                LikeBtnListener(position, this);
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