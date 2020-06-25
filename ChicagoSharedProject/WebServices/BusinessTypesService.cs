using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Interfaces.Businesses;

namespace Tabs.Mobile.Shared.WebServices
{
    public class BusinessTypesService : BaseService, IBusinessTypesFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTypes"></param>
        public async Task AddBusinessType(BusinessTypes businessTypes)
        {
            string methodPath = "businesstypes/";
            Address response = null;
            var parameters = new
            {
                BusinessId = businessTypes.BusinessId,
                Bar = businessTypes.Bar,
                Club = businessTypes.Club,
                Lounge = businessTypes.Lounge,
                Other = businessTypes.Other,
                Restaurant = businessTypes.Restaurant

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Address>(methodPath, parameters, false, "PUT"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTypes"></param>
        public async Task Update(BusinessTypes businessTypes)
        {
            string methodPath = "businesstypes/";
            Address response = null;
            var parameters = new
            {
                BusinessId = businessTypes.BusinessId,
                Bar = businessTypes.Bar,
                Club = businessTypes.Club,
                Lounge = businessTypes.Lounge,
                Other = businessTypes.Other,
                Restaurant = businessTypes.Restaurant,
                BusinessTypeId = businessTypes.BusinessTypeId

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Address>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public async Task<BusinessTypes> GetBusinessType(int businessId)
        {
            string methodPath = "businesstypes/" + businessId;
            BusinessTypes response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<BusinessTypes>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        #endregion

    }
}
