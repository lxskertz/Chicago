using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Orders;
using Tabs.Mobile.ChicagoAndroid.Fragments.Orders;
using Tabs.Mobile.ChicagoAndroid.Activities.Orders;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Orders;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Orders
{
    public class OrdersAdapter : BaseAdapter
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<ToasterOrder> ToasterOrders { get; set; }

        public ToasterOrder.ToasterOrderEnum ToasterOrderEnum { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private OrdersFragment OrdersFragment { get; set; }

        private OrdersActivity OrdersActivity { get; set; }

        #endregion

        #region Constructors

        public OrdersAdapter(OrdersFragment ordersFragment, List<ToasterOrder> toasterOrders,
          ToasterOrder.ToasterOrderEnum toasterOrderEnum)
        {
            this.OrdersFragment = ordersFragment;
            this.ToasterOrders = toasterOrders;
            this.ToasterOrderEnum = toasterOrderEnum;
        }

        public OrdersAdapter(OrdersActivity ordersActivity, List<ToasterOrder> toasterOrders,
         ToasterOrder.ToasterOrderEnum toasterOrderEnum)
        {
            this.OrdersActivity = ordersActivity;
            this.ToasterOrders = toasterOrders;
            this.ToasterOrderEnum = toasterOrderEnum;
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
                return this.ToasterOrders.Count;
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
            return position;
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
            OrdersViewholder holder = null;
            var view = convertView;

            if (view != null)
            {
                holder = view.Tag as OrdersViewholder;
            }

            if (this.OrdersFragment != null)
            {
                if (holder == null)
                {
                    view = LayoutInflater.FromContext(this.OrdersFragment.HomeContext).Inflate(Resource.Layout.OrdersListItem, container, false);
                    holder = new OrdersViewholder(this, view, OnActionBtnListener);
                    view.Tag = holder;
                }
            } else
            {
                if (view == null)
                {
                    view = LayoutInflater.FromContext(this.OrdersActivity).Inflate(Resource.Layout.OrdersListItem, container, false);
                    holder = new OrdersViewholder(this, view, OnActionBtnListener);
                    view.Tag = holder;
                }
            }

            var item = this.ToasterOrders.ElementAt(position);

            if (item != null)
            {
                holder.ActionButon.SetTag(Resource.Id.actionButon, position);
                string freeOrder = item.FromBusiness ? AppText.FreeOrder : "";
                holder.OrderNumber.Text = item.ToasterOrderId.ToString() + freeOrder;
                holder.DrinkName.Text = string.IsNullOrEmpty(item.DrinkName) ? "" : item.DrinkName;
                var buttonTitle = AppText.PickedUp;

                switch (this.ToasterOrderEnum)
                {
                    case ToasterOrder.ToasterOrderEnum.Sender:
                        buttonTitle = item.Charged ? AppText.PickedUp : AppText.Cancel;
                        holder.ActionButon.Text = buttonTitle;
                        holder.ActionButon.Enabled = buttonTitle == AppText.Cancel;
                        holder.Name.Text = string.IsNullOrEmpty(item.ReceiverName) ? "" : item.ReceiverName;
                        break;
                    case ToasterOrder.ToasterOrderEnum.Receiver:
                        holder.ActionButon.Visibility = ViewStates.Gone;
                        holder.Name.Text = string.IsNullOrEmpty(item.SenderName) ? "" : item.SenderName;
                        break;
                    case ToasterOrder.ToasterOrderEnum.Business:
                        if (item.FromBusiness)
                        {
                            buttonTitle = item.PickedUp ? AppText.PickedUp : AppText.ConfirmPicked;
                            holder.ActionButon.Enabled = buttonTitle == AppText.ConfirmPicked;
                        }
                        else
                        {
                            buttonTitle = item.Charged ? AppText.PickedUp : AppText.Charge;
                            holder.ActionButon.Enabled = buttonTitle == AppText.Charge;
                        }
                        holder.ActionButon.Text = buttonTitle;
                        holder.Name.Text = string.IsNullOrEmpty(item.ReceiverName) ? "" : item.ReceiverName;
                        break;
                }

            }

            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="viewHolder"></param>
        public void OnActionBtnListener(int position, OrdersViewholder viewHolder)
        {
            var item = this.ToasterOrders.ElementAt(position);
            switch (this.ToasterOrderEnum)
            {
                case ToasterOrder.ToasterOrderEnum.Sender:
                    if (!item.Charged)
                    {
                        this.OrdersActivity.CancelDrink(item);
                    }
                    break;
                case ToasterOrder.ToasterOrderEnum.Business:
                    if (!item.Charged && !item.FromBusiness)
                    {
                        this.OrdersFragment.ChargeDrink(item);
                    }
                    else if (item.FromBusiness)
                    {
                        this.OrdersFragment.MarkPickedUp(item);
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (this.OrdersFragment != null)
            {
                var item = this.ToasterOrders.ElementAt(e.Position);
                Intent activity = new Intent(this.OrdersFragment.HomeContext, typeof(OrderDetailsActivity));
                activity.PutExtra("ToasterOrderEnum", (int)ToasterOrderEnum);
                activity.PutExtra("ToasterOrder", JsonConvert.SerializeObject(item));
                this.OrdersFragment.HomeContext.StartActivity(activity);
            } else
            {
                var item = this.ToasterOrders.ElementAt(e.Position);
                Intent activity = new Intent(this.OrdersActivity, typeof(OrderDetailsActivity));
                activity.PutExtra("ToasterOrderEnum", (int)ToasterOrderEnum);
                activity.PutExtra("ToasterOrder", JsonConvert.SerializeObject(item));
                this.OrdersActivity.StartActivity(activity);
            }
        }

        #endregion

    }
}