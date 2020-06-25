using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Interfaces.Businesses;

namespace Tabs.Mobile.Shared.Managers.Businesses
{
    public class BusinessTypesFactory : IBusinessTypesFactory
    {

        #region Constants, Enums, and Variables

        private IBusinessTypesFactory _BusinessTypesFactory;

        #endregion

        #region Constructors

        public BusinessTypesFactory(IBusinessTypesFactory businessTypesFactory)
        {
            _BusinessTypesFactory = businessTypesFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTypes"></param>
        public Task AddBusinessType(BusinessTypes businessTypes)
        {
            return _BusinessTypesFactory.AddBusinessType(businessTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTypes"></param>
        public Task Update(BusinessTypes businessTypes)
        {
            return _BusinessTypesFactory.Update(businessTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public Task<BusinessTypes> GetBusinessType(int businessId)
        {
            return _BusinessTypesFactory.GetBusinessType(businessId);
        }

        #endregion

    }
}
