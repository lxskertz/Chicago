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
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals
{
    public class InviteContactsViewHolder : Java.Lang.Object
    {

        #region Properties 

        private Action<int, InviteContactsViewHolder> ActionBtnListener { get; set; }

        public TextView Title { get; set; }

        public TextView Subtitle { get; set; }

        public Button Invitebutton { get; set; }

        private InviteContactAdapter InviteContactAdapter { get; set; }

        #endregion

        #region Constructors

        public InviteContactsViewHolder(InviteContactAdapter inviteContactAdapter, View itemView,
            Action<int, InviteContactsViewHolder> btnClick)
        {
            this.InviteContactAdapter = InviteContactAdapter;
            this.Title = itemView.FindViewById<TextView>(Resource.Id.title);
            this.Subtitle = itemView.FindViewById<TextView>(Resource.Id.subTitle);
            this.Invitebutton = itemView.FindViewById<Button>(Resource.Id.inviteBtn);

            ActionBtnListener = btnClick;
            this.Invitebutton.Click += OnActionBtnListener;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnActionBtnListener(object sender, EventArgs e)
        {
            try
            {
                var position = Convert.ToInt32(this.Invitebutton.GetTag(Resource.Id.inviteBtn));
                ActionBtnListener(position, this);
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}