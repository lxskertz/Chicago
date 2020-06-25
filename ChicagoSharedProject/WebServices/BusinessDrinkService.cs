using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.Drinks;
using Tabs.Mobile.Shared.Models.Drinks;
namespace Tabs.Mobile.Shared.WebServices
{
    public class BusinessDrinkService : BaseService, IBusinessDrinkFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessDrink"></param>
        /// <returns></returns>
        public async Task Add(BusinessDrink businessDrink)
        {
            string methodPath = "drink/";
            BusinessDrink response = null;
            var parameters = new
            {
                BusinessId = businessDrink.BusinessId,
                DrinkName = businessDrink.DrinkName,
                Price = businessDrink.Price
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<BusinessDrink>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public async Task<ICollection<BusinessDrink>> Get(int businessId)
        {
            string methodPath = "drink/" + businessId;
            ICollection<BusinessDrink> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<BusinessDrink>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drinkId"></param>
        /// <param name="drinkName"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public async Task Update(int drinkId, string drinkName, double price)
        {
            string methodPath = "drink/";
            HttpResponseMessage response = null;
            var parameters = new
            {
                DrinkId = drinkId,
                DrinkName = drinkName,
                Price = price
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drinkId"></param>
        /// <returns></returns>
        public async Task Delete(int drinkId)
        {
            string methodPath = "drink/" + drinkId;
            HttpResponseMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, null, true, "DELETE"));
            response = await request;
        }

        #endregion

    }
}
