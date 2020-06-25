using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Interfaces;

namespace Tabs.Mobile.Shared.Managers
{
    public class NotificationRegisterFactory : INotificationRegisterFactory
    {

        #region Constants, Enums, and Variables

        private INotificationRegisterFactory _NotificationRegisterFactory;

        #endregion

        #region Constructors

        public NotificationRegisterFactory(INotificationRegisterFactory notificationRegisterFactory)
        {
            _NotificationRegisterFactory = notificationRegisterFactory;
        }

        #endregion


        #region Methods

        /// <summary>
        /// This creates a registration id
        /// </summary>
        /// <returns></returns>
        public Task<string> CreateId(string handle)
        {
            return _NotificationRegisterFactory.CreateId(handle);
        }

        /// <summary>
        /// Delete registration
        /// </summary>
        /// <param name="id"></param>
        public Task Delete(string id)
        {
            return _NotificationRegisterFactory.Delete(id);
        }

        /// <summary>
        /// This creates or updates a registration (with provided PNS handle) at the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deviceUpdate"></param>
        public Task<HttpResponseMessage> AppUpdateRegistration(DeviceRegistration deviceUpdate)
        {
            return _NotificationRegisterFactory.AppUpdateRegistration(deviceUpdate);
        }

        /// <summary>
        /// Send push
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<bool> SendPush(NotificationQuery query)
        {
            return _NotificationRegisterFactory.SendPush(query);
        }

        #endregion

    }
}
