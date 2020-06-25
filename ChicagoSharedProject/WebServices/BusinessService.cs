using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Interfaces.Businesses;

namespace Tabs.Mobile.Shared.WebServices
{
    public class BusinessService : BaseService, IBusinessFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="business"></param>
        public async Task CreateBusiness(Business business)
        {
            //SalesTaxRate = business.SalesTaxRate
            string methodPath = "business/";
            HttpResponseMessage response = null;
            var parameters = new
            {
                AccountAdministratorId = business.AccountAdministratorId,
                AccountStatus = true,
                AllowReviews = true,
                Available = true,
                BusinessDescription = string.Empty,
                EINTaxID = string.Empty,
                BusinessName = business.BusinessName,
                SSN = string.Empty,
                UserId = business.UserId,
                PhoneNumber = business.PhoneNumber
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, false, "PUT"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="business"></param>
        public async Task Update(Business business)
        {
            string methodPath = "business/";
            HttpResponseMessage response = null;
            var parameters = new
            {
                BusinessName = business.BusinessName,
                UserId = business.UserId,
                PhoneNumber = business.PhoneNumber,
                BusinessId = business.BusinessId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Business> GetByUserId(int userId)
        {
            string methodPath = "business_userid/" + userId;
            Business response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Business>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public async Task<Business> Get(int businessId)
        {
            string methodPath = "business/" + businessId;
            Business response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Business>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="city"></param>
        /// <param name="zipcode"></param>
        /// <param name="searchTerm"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<ICollection<BusinessSearch>> NearByBusinesses(string city, string zipcode, string searchTerm, int pageSize, int pageNumber)
        {
            string methodPath = "business/nearby";
            ICollection<BusinessSearch> response = new List<BusinessSearch>();
            var parameters = new
            {
                Zipcode = zipcode,
                City = city,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<BusinessSearch>>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }


        #endregion

    }
}
