using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.Individuals;

namespace Tabs.Mobile.Shared.WebServices
{
    public class SMSMessageService : BaseService, ISMSMessageFactory
    {

        #region Methods

        public async Task SendInvitation(string phoneNumber, string senderName, string receiverName)
        {
            string methodPath = "sms/invitation/";
            HttpResponseMessage response = null;
            var parameters = new
            {
                PhoneNumber = phoneNumber,
                SenderName = senderName,
                ReceiverName = receiverName
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        #endregion

    }
}
