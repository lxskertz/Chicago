using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Widget;
using Newtonsoft.Json;
using Tabs.Mobile.ChicagoAndroid.Adapters.Orders;
using Tabs.Mobile.Shared.Models.Orders;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Orders
{
    [Activity(Label = "Order Details", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class OrderDetailsActivity : BaseActivity
    {

        #region Constants, Enums, Variables

        private ListView orderDetailsList;
        private View headerView;
        private OrderDetailsAdapter OrderDetailsAdapter;

        #endregion

        #region Properties

        public ToasterOrder.ToasterOrderEnum ToasterOrderEnum { get; set; }

        public ToasterOrder ToasterOrder { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.OrderDetails);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                orderDetailsList = FindViewById<ListView>(Resource.Id.orderDetailsList);
                GetOrder();
            }
            catch (Exception)
            {
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private void GetOrder()
        {
            try
            {
                this.ToasterOrder = JsonConvert.DeserializeObject<ToasterOrder>(Intent.GetStringExtra("ToasterOrder"));
                this.ToasterOrderEnum = (ToasterOrder.ToasterOrderEnum)Intent.GetIntExtra("ToasterOrderEnum", (int)ToasterOrder.ToasterOrderEnum.Receiver);

                if (headerView != null)
                {
                    orderDetailsList.RemoveHeaderView(headerView);
                }
                headerView = LayoutInflater.FromContext(this).Inflate(Resource.Layout.OrderDetailsHeader, null);

                var OrderNumer = headerView.FindViewById<TextView>(Resource.Id.subTitle);
                string freeOrder = ToasterOrder.FromBusiness ? AppText.FreeOrder : "";
                OrderNumer.Text = ToasterOrder.ToasterOrderId.ToString() + freeOrder;

                orderDetailsList.AddHeaderView(headerView);
                OrderDetailsAdapter = new OrderDetailsAdapter(this, ToasterOrder, ToasterOrderEnum);
                orderDetailsList.Adapter = OrderDetailsAdapter;
                orderDetailsList.ItemClick += OrderDetailsAdapter.OnListItemClick;
                orderDetailsList.DividerHeight = 2;
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion

    }
}