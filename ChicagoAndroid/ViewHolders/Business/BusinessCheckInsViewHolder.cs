using System;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.ChicagoAndroid.Adapters.Business;

namespace Tabs.Mobile.ChicagoAndroid.ViewHolders.Business
{
    public class BusinessCheckInsViewHolder : Java.Lang.Object
    {
        #region Properties 

        private Action<int, BusinessCheckInsViewHolder> ActionBtnListener { get; set; }

        public TextView Title { get; set; }

        public TextView Subtitle { get; set; }

        public Button SendDrinkButon { get; set; }

        private BusinessCheckInsAdapter BusinessCheckInsAdapter { get; set; }

        #endregion

        #region Constructors

        public BusinessCheckInsViewHolder(BusinessCheckInsAdapter adapter, View itemView,
            Action<int, BusinessCheckInsViewHolder> btnClick)
        {
            this.BusinessCheckInsAdapter = adapter;
            this.Title = itemView.FindViewById<TextView>(Resource.Id.title);
            this.Subtitle = itemView.FindViewById<TextView>(Resource.Id.subTitle);
            this.SendDrinkButon = itemView.FindViewById<Button>(Resource.Id.sendDrinkBtn);

            ActionBtnListener = btnClick;
            this.SendDrinkButon.Click += OnActionBtnListener;
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
                var position = Convert.ToInt32(this.SendDrinkButon.GetTag(Resource.Id.sendDrinkBtn));
                ActionBtnListener(position, this);
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}