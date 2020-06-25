using System.Threading.Tasks;
using System.Collections.Generic;
using Tabs.Mobile.Shared.Interfaces.Businesses;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.Shared.Managers.Businesses
{
    public class BusinessFactory : IBusinessFactory
    {

        #region Constants, Enums, and Variables

        private IBusinessFactory _BusinessFactory;

        #endregion

        #region Constructors

        public BusinessFactory(IBusinessFactory businessFactory)
        {
            _BusinessFactory = businessFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="business"></param>
        public Task CreateBusiness(Business business)
        {
            return this._BusinessFactory.CreateBusiness(business);
        }

        public Task Update(Business business)
        {
            return this._BusinessFactory.Update(business);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<Business> GetByUserId(int userId)
        {
            return this._BusinessFactory.GetByUserId(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public Task<Business> Get(int businessId)
        {
            return this._BusinessFactory.Get(businessId);
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
        public Task<ICollection<BusinessSearch>> NearByBusinesses(string city, string zipcode, string searchTerm, int pageSize, int pageNumber)
        {
            return this._BusinessFactory.NearByBusinesses(city, zipcode, searchTerm, pageSize, pageNumber);
        }


        #endregion
    }
}