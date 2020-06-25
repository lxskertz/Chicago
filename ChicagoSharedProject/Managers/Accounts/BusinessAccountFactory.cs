using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.Accounts;
using Tabs.Mobile.Shared.Models.Accounts;

namespace Tabs.Mobile.Shared.Managers.Accounts
{
    public class BusinessAccountFactory : IBusinessAccountFactory
    {

        #region Constants, Enums, and Variables

        private IBusinessAccountFactory _BusinessAccountFactory;

        #endregion

        #region Constructors

        public BusinessAccountFactory(IBusinessAccountFactory businessAccountFactory)
        {
            _BusinessAccountFactory = businessAccountFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessAccount"></param>
        public Task CreateBusinessAccount(BusinessAccount businessAccount)
        {
            return this._BusinessAccountFactory.CreateBusinessAccount(businessAccount);
        }

        #endregion

    }
}