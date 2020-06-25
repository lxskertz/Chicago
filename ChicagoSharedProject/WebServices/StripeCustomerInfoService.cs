using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Payment;
using Tabs.Mobile.Shared.Interfaces.Payments;

namespace Tabs.Mobile.Shared.WebServices
{
    public class StripeCustomerInfoService : BaseService, IStripeCustomerInfoFactory
    {

        public async Task<int> Add(StripeCustomerInfo stripeCustomerInfo)
        {
            string methodPath = "stripecustomerinfo/";
            int response = 0;
            var parameters = new
            {
                UserId = stripeCustomerInfo.UserId,
                StripeCustomerId = stripeCustomerInfo.StripeCustomerId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, parameters, true, "PUT"));
            response = await request;

            return response;
        }

        public async Task<StripeCustomerInfo> Get(int userId)
        {
            string methodPath = "stripecustomerinfo/" + userId;
            StripeCustomerInfo response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<StripeCustomerInfo>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task Delete(int stripeCustomerInfoId)
        {
            string methodPath = "stripecustomerinfo/" + stripeCustomerInfoId;
            HttpResponseMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, null, true, "DELETE"));
            response = await request;
        }

    }
}
