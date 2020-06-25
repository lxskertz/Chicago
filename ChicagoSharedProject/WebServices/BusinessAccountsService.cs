using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Accounts;
using Tabs.Mobile.Shared.Interfaces.Accounts;

namespace Tabs.Mobile.Shared.WebServices
{
    public class BusinessAccountsService : BaseService, IBusinessAccountFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessAccount"></param>
        public async Task CreateBusinessAccount(BusinessAccount businessAccount)
        {
            string methodPath = "account/create/";
            HttpResponseMessage response = null;
            var parameters = new
            {
                UserId = businessAccount.UserId,
                BusinessId = businessAccount.BusinessId

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, false, "POST"));
            response = await request;
        }

        #endregion

    }
}
