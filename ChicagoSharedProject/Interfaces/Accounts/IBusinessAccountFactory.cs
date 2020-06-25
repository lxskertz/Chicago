using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Accounts;

namespace Tabs.Mobile.Shared.Interfaces.Accounts
{
    public interface IBusinessAccountFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessAccount"></param>
        Task CreateBusinessAccount(BusinessAccount businessAccount);

        #endregion

    }
}
