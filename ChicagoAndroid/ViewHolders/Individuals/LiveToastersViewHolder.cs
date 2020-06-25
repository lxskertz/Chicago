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
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals.CheckIns;
using Tabs.Mobile.ChicagoAndroid.Helpers;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals
{
    public class LiveToastersViewHolder : RecyclerView.ViewHolder
    {

        #region Properties 

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int> Listener { get; set; }

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int, LiveToastersViewHolder> LikeBtnListener { get; set; }

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int, LiveToastersViewHolder> MoreBtnListener { get; set; }

        /// <summary>
        /// Gets or sets listener
        /// </summary>
        private Action<int, LiveToastersViewHolder> SendDrinkBtnListener { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public TextView Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ImageView CheckInPic { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public TextView LikeCount { get; set; }

        public Button SendDrinkBtn { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public ImageButton LikeBtn { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public ImageButton MoreBtn { get; set; }

        /// <summary>
        /// Gets or sets adapter
        /// </summary>
        private LiveToastersAdapter LiveToastersAdapter { get; set; }

        #endregion

        #region Constructors

        public LiveToastersViewHolder(LiveToastersAdapter liveToastersAdapter, View itemView,
            Action<int> cardClick, Action<int, LiveToastersViewHolder> btnClick,
            Action<int, LiveToastersViewHolder> sendDrinkClick,
            Action<int, LiveToastersViewHolder> morebtnClick) : base(itemView)
        {
            this.LiveToastersAdapter = liveToastersAdapter;
            this.Username = itemView.FindViewById<TextView>(Resource.Id.username);          
            this.CheckInPic = itemView.FindViewById<ImageView>(Resource.Id.checkInPic);
            this.LikeCount = itemView.FindViewById<TextView>(Resource.Id.likeCount);
            this.LikeBtn = itemView.FindViewById<ImageButton>(Resource.Id.likeBtn);
            this.SendDrinkBtn = itemView.FindViewById<Button>(Resource.Id.sendDrink);
            this.MoreBtn = itemView.FindViewById<ImageButton>(Resource.Id.moreBtn);

            Listener = cardClick;
            itemView.Click += OnClick;

            LikeBtnListener = btnClick;
            this.LikeBtn.Click += OnLikeBtnClick;

            SendDrinkBtnListener = sendDrinkClick;
            this.SendDrinkBtn.Click += OnSendDrinkBtnClick;

            MoreBtnListener = morebtnClick;
            this.MoreBtn.Click += OnMoreBtnClick;
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
        private void OnMoreBtnClick(object sender, EventArgs e)
        {
            int position = base.AdapterPosition;
            if (position != RecyclerView.NoPosition)
            {
                MoreBtnListener(position, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSendDrinkBtnClick(object sender, EventArgs e)
        {
            int position = base.AdapterPosition;
            if (position != RecyclerView.NoPosition)
            {
                SendDrinkBtnListener(position, this);
            }
        }

        #endregion

    }
}