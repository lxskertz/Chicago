using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.Shared.Interfaces.Businesses
{
    public interface IBusinessTypesFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTypes"></param>
        Task AddBusinessType(BusinessTypes businessTypes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTypes"></param>
        Task Update(BusinessTypes businessTypes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        Task<BusinessTypes> GetBusinessType(int businessId);

        #endregion

    }
}
