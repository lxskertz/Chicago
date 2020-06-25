using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.Shared.Interfaces
{
    public interface INotificationRegisterFactory
    {

        #region Methods

        /// <summary>
        /// This creates a registration id
        /// </summary>
        /// <returns></returns>
        Task<string> CreateId(string handle);

        /// <summary>
        /// Delete registration
        /// </summary>
        /// <param name="id"></param>
        Task Delete(string id);

        /// <summary>
        /// This creates or updates a registration (with provided PNS handle) at the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deviceUpdate"></param>
        Task<HttpResponseMessage> AppUpdateRegistration(DeviceRegistration deviceUpdate);

        /// <summary>
        /// Send push
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> SendPush(NotificationQuery query);

        #endregion

    }
}
