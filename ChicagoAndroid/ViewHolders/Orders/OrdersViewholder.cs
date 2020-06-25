using System;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.ChicagoAndroid.Adapters.Orders;

namespace Tabs.Mobile.ChicagoAndroid.ViewHolders.Orders
{
    public class OrdersViewholder : Java.Lang.Object
    {

        #region Properties 

        private Action<int, OrdersViewholder> ActionBtnListener { get; set; }

        public TextView OrderNumber { get; set; }

        public TextView Name { get; set; }

        public TextView DrinkName { get; set; }

        public Button ActionButon { get; set; }

        private OrdersAdapter OrdersAdapter { get; set; }

        #endregion

        #region Constructors

        public OrdersViewholder(OrdersAdapter ordersAdapter, View itemView,
            Action<int, OrdersViewholder> btnClick) 
        {
            this.OrdersAdapter = ordersAdapter;
            this.OrderNumber = itemView.FindViewById<TextView>(Resource.Id.orderNumber);
            this.Name = itemView.FindViewById<TextView>(Resource.Id.senderReceiverName);
            this.DrinkName = itemView.FindViewById<TextView>(Resource.Id.drinkName);
            this.ActionButon = itemView.FindViewById<Button>(Resource.Id.actionButon);

            ActionBtnListener = btnClick;
            this.ActionButon.Click += OnActionBtnListener;
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
                var position = Convert.ToInt32(this.ActionButon.GetTag(Resource.Id.actionButon));
                ActionBtnListener(position, this);
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}