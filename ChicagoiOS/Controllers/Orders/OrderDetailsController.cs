using Foundation;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Models.Orders;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoiOS.DataSource.Orders;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class OrderDetailsController : BaseViewController
    {

        #region Constants, Enums, and Variables

        #endregion

        #region Properties

        public ToasterOrder.ToasterOrderEnum ToasterOrderEnum { get; set; }

        public ToasterOrder ToasterOrder { get; set; }

        private OrderDetailsDataSource OrderDetailsDataSource { get; set; }

        #endregion

        #region Constructors

        public OrderDetailsController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            GetOrder();
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private void GetOrder()
        {
            try
            {
                string freeOrder = ToasterOrder.FromBusiness ? AppText.FreeOrder : "";
                OrderNumer.Text = ToasterOrder.ToasterOrderId.ToString() + freeOrder;
                OrderDetailsTable.EstimatedRowHeight = 44f;
                OrderDetailsTable.RowHeight = UITableView.AutomaticDimension;
                OrderDetailsDataSource = new OrderDetailsDataSource(this, ToasterOrder, ToasterOrderEnum);
                OrderDetailsTable.Source = OrderDetailsDataSource;
                OrderDetailsTable.TableFooterView = new UIView();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        #endregion

    }
}