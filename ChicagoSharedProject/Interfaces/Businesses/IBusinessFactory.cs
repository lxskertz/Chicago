using System.Threading.Tasks;
using System.Collections.Generic;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.Shared.Interfaces.Businesses
{
    public interface IBusinessFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="business"></param>
        Task CreateBusiness(Business business);

        Task Update(Business business);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Business> GetByUserId(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        Task<Business> Get(int businessId);

        Task<ICollection<BusinessSearch>> NearByBusinesses(string city, string zipcode, string searchTerm, int pageSize, int pageNumber);

        #endregion

    }
}
