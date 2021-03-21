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
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Orders;
using Tabs.Mobile.ChicagoAndroid.Activities.Orders;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Orders
{
     public class OrderDetailsAdapter : BaseAdapter
    {

        #region Properties

        public ToasterOrder ToasterOrder { get; set; }

        private OrderDetailsActivity OrderDetailsActivity { get; set; }

        public ToasterOrder.ToasterOrderEnum ToasterOrderEnum { get; set; }

        #endregion

        #region Constructors

        public OrderDetailsAdapter(OrderDetailsActivity orderDetailsActivity,
            ToasterOrder toasterOrder, ToasterOrder.ToasterOrderEnum toasterOrderEnum)
        {
            this.OrderDetailsActivity = orderDetailsActivity;
            this.ToasterOrder = toasterOrder;
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
                return this.ToasterOrderEnum == ToasterOrder.ToasterOrderEnum.Receiver ? 10 : 14;
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
            return "";
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
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(this.OrderDetailsActivity).Inflate(Resource.Layout.OrderDetailsListItem, container, false);
            }

            var title = convertView.FindViewById<TextView>(Resource.Id.title);
            var subTitle = convertView.FindViewById<TextView>(Resource.Id.subTitle);

            if (this.ToasterOrderEnum == ToasterOrder.ToasterOrderEnum.Receiver)
            {
                switch (position)
                {
                    case 0:
                        title.Text = AppText.OrderNumber;
                        subTitle.Text = this.ToasterOrder.ToasterOrderId.ToString();
                        break;
                    case 1:
                        title.Text = AppText.DrinkName;
                        subTitle.Text = this.ToasterOrder.DrinkName;
                        break;
                    case 2:
                        title.Text = AppText.ReceiverName;
                        subTitle.Text = this.ToasterOrder.ReceiverName;
                        break;
                    case 3:
                        title.Text = AppText.SenderName;
                        subTitle.Text = this.ToasterOrder.SenderName;
                        break;
                    case 4:
                        title.Text = AppText.BusinessName;
                        subTitle.Text = this.ToasterOrder.BusinessName;
                        break;
                    case 5:
                        title.Text = AppText.OrderDate;
                        subTitle.Text = this.ToasterOrder.OrderDate != null && this.ToasterOrder.OrderDate.HasValue ? this.ToasterOrder.OrderDate.Value.ToShortDateString() : "";
                        break;
                    case 6:
                        title.Text = AppText.PickedUpDate;
                        subTitle.Text = this.ToasterOrder.PickedUpDate != null && this.ToasterOrder.PickedUpDate.HasValue ? this.ToasterOrder.PickedUpDate.Value.ToShortDateString() : "";
                        break;
                    case 7:
                        title.Text = AppText.PickedUp;
                        subTitle.Text = this.ToasterOrder.PickedUp ? "Yes" : "No";
                        break;
                    case 8:
                        title.Text = AppText.FromBusiness;
                        subTitle.Text = this.ToasterOrder.FromBusiness ? "Yes" : "No";
                        break;
                    case 9:
                        title.Text = AppText.Quantity;
                        subTitle.Text = this.ToasterOrder.Quantity.ToString();
                        break;
                }
            }
            else
            {
                switch (position)
                {
                    case 0:
                        title.Text = AppText.OrderNumber;
                        subTitle.Text = this.ToasterOrder.ToasterOrderId.ToString();
                        break;
                    case 1:
                        title.Text = AppText.DrinkName;
                        subTitle.Text = this.ToasterOrder.DrinkName;
                        break;
                    case 2:
                        title.Text = AppText.TotalOrderAmount;
                        switch (this.ToasterOrderEnum)
                        {
                            case ToasterOrder.ToasterOrderEnum.Receiver:
                            case ToasterOrder.ToasterOrderEnum.Sender:
                                subTitle.Text = "$" + this.ToasterOrder.TotalOrderAmount.ToString();
                                break;
                            case ToasterOrder.ToasterOrderEnum.Business:
                                subTitle.Text = "$" + this.ToasterOrder.DrinkAmount.ToString();
                                break;
                        }
                        break;
                    case 3:
                        title.Text = AppText.ReceiverName;
                        subTitle.Text = this.ToasterOrder.ReceiverName;
                        break;
                    case 4:
                        title.Text = AppText.SenderName;
                        subTitle.Text = this.ToasterOrder.SenderName;
                        break;
                    case 5:
                        title.Text = AppText.BusinessName;
                        subTitle.Text = this.ToasterOrder.BusinessName;
                        break;
                    case 6:
                        title.Text = AppText.OrderDate;
                        subTitle.Text = this.ToasterOrder.OrderDate != null && this.ToasterOrder.OrderDate.HasValue ? this.ToasterOrder.OrderDate.Value.ToShortDateString() : "";
                        break;
                    case 7:
                        title.Text = AppText.PickedUpDate;
                        subTitle.Text = this.ToasterOrder.PickedUpDate != null && this.ToasterOrder.PickedUpDate.HasValue ? this.ToasterOrder.PickedUpDate.Value.ToShortDateString() : "";
                        break;
                    case 8:
                        title.Text = AppText.Charged;
                        subTitle.Text = this.ToasterOrder.Charged ? "Yes" : "No";
                        break;
                    case 9:
                        title.Text = AppText.PickedUp;
                        subTitle.Text = this.ToasterOrder.PickedUp ? "Yes" : "No";
                        break;
                    case 10:
                        title.Text = AppText.OrderPointAmount;
                        subTitle.Text = "$" + this.ToasterOrder.PointsAmount.ToString();
                        break;
                    case 11:
                        title.Text = AppText.UsedPointType;
                        switch (this.ToasterOrder.UsedPointType)
                        {
                            case ToasterOrder.PointType.None:
                                subTitle.Text = AppText.None;
                                break;
                            case ToasterOrder.PointType.Full:
                                subTitle.Text = AppText.FullOff;
                                break;
                            case ToasterOrder.PointType.Quarter:
                                subTitle.Text = AppText.Quarteroff;
                                break;
                        }
                        break;
                    case 12:
                        title.Text = AppText.FromBusiness;
                        subTitle.Text = this.ToasterOrder.FromBusiness ? "Yes" : "No";
                        break;
                    case 13:
                        title.Text = AppText.Quantity;
                        subTitle.Text = this.ToasterOrder.Quantity.ToString();
                        break;
                }
            }

            return convertView;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
        }

        #endregion

    }
}