using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Orders;
using Tabs.Mobile.Shared.Interfaces.Orders;

namespace Tabs.Mobile.Shared.WebServices
{
    public class ToasterOrderService : BaseService, IToasterOrderFactory
    {

        #region Methods

        public async Task New(ToasterOrder toasterOrder)
        {
            string methodPath = "toaster_order/";
            HttpResponseMessage response = null; 
            var parameters = new
            {
                BusinessDrinkId = toasterOrder.BusinessDrinkId,
                BusinessId = toasterOrder.BusinessId,
                BusinessName = toasterOrder.BusinessName,
                Charged = toasterOrder.Charged,
                DrinkAmount = toasterOrder.DrinkAmount,
                DrinkName = toasterOrder.DrinkName,
                OrderDate = toasterOrder.OrderDate,
                PickedUp = toasterOrder.PickedUp,
                PickedUpDate = toasterOrder.PickedUpDate,
                ReceiverName = toasterOrder.ReceiverName,
                ReceiverUserId = toasterOrder.ReceiverUserId,
                SenderStripeCustomerId = toasterOrder.SenderStripeCustomerId,
                SenderUserId = toasterOrder.SenderUserId,
                TabsServiceFee = toasterOrder.TabsServiceFee,
                TipAmount = toasterOrder.TipAmount,
                TotalOrderAmount = toasterOrder.TotalOrderAmount,
                SenderName = toasterOrder.SenderName,
                Cancelled = toasterOrder.Cancelled,
                PointsAmount = toasterOrder.PointsAmount,
                UsedPointType = toasterOrder.UsedPointType,
                OrderDateString = DateTime.Now.ToShortDateString(),
                PickedUpDateString = DateTime.Now.ToShortDateString(),
                FromBusiness = toasterOrder.FromBusiness,
                SalesTax = toasterOrder.SalesTax,
                StripeFee = toasterOrder.StripeFee,
                CardChargeAmount = toasterOrder.CardChargeAmount,
                Quantity = toasterOrder.Quantity
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }
         
        public async Task Charged(int toasterOrderId)
        {
            string methodPath = "toaster_order/charged/" + toasterOrderId;
            HttpResponseMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, null, true, "GET"));
            response = await request;
        }

        public async Task Cancel(int toasterOrderId)
        {
            string methodPath = "toaster_order/cancel/" + toasterOrderId;
            HttpResponseMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, null, true, "GET"));
            response = await request;
        }

        public async Task<ToasterOrder> GetByOrderId(int toasterOrderId)
        {
            string methodPath = "toaster_order/" + toasterOrderId;
            ToasterOrder response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ToasterOrder>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<ICollection<ToasterOrder>> GetBusinessOrders(int businessId)
        {
            string methodPath = "toaster_order/business/" + businessId;
            ICollection<ToasterOrder> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<ToasterOrder>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<ICollection<ToasterOrder>> GetUserOrders(int userId)
        {
            string methodPath = "toaster_order/sender/" + userId;
            ICollection<ToasterOrder> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<ToasterOrder>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<ICollection<ToasterOrder>> GetReceivedDrinks(int userId)
        {
            string methodPath = "toaster_order/receiveddrinks/" + userId;
            ICollection<ToasterOrder> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<ToasterOrder>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        #endregion

    }
}
