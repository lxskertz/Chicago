using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Interfaces;

namespace Tabs.Mobile.Shared.WebServices
{
    public class NotificationRegisterService : BaseService, INotificationRegisterFactory
    {

        #region Methods

        /// <summary>
        /// This creates a registration id
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateId(string handle)
        {
            string methodPath = "notification/create_id";
            string response = null;
            var parameters = new
            {
                handle = handle
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<string>(methodPath, handle, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Delete registration
        /// </summary>
        /// <param name="id"></param>
        public async Task Delete(string id)
        {
            string methodPath = "notification/";
            HttpResponseMessage response;
            var parameters = new
            {
                id = id
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, id, true, "DELETE"));
            response = await request;
        }

        /// <summary>
        /// This creates or updates a registration (with provided PNS handle) at the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deviceUpdate"></param>
        public async Task<HttpResponseMessage> AppUpdateRegistration(DeviceRegistration deviceUpdate)
        {
            string methodPath = "notification/";
            HttpResponseMessage response;
            string[] tags = new string[deviceUpdate.Tags.Count];
            for (int i = 0; i <= deviceUpdate.Tags.Count - 1; i++)
            {
                tags[i] = deviceUpdate.Tags[i];
            }

            var parameters = new
            {
                RegistrationId = deviceUpdate.RegistrationId,
                Platform = deviceUpdate.Platform,
                Handle = deviceUpdate.Handle,
                Tags = tags
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, false, "PUT"));
            return await request;
        }

        /// <summary>
        /// Send push
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> SendPush(NotificationQuery query)
        {
            string methodPath = "notification/notify/";
            bool response;
            string[] tags = new string[query.Tags.Count];
            for (int i = 0; i <= query.Tags.Count - 1; i++)
            {
                tags[i] = query.Tags[i];
            }

            var parameters = new
            {
                Message = query.Message,
                PNS = query.PNS,
                Tags = tags
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<bool>(methodPath, parameters, true, "POST"));
            return response = await request;
        }

        #endregion

    }
}
