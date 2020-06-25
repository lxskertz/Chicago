using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Events;

namespace Tabs.Mobile.Shared.Interfaces.Events
{
    public interface IRsvpFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rsvp"></param>
        Task Add(Rsvp rsvp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="going"></param>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        Task Change(bool going, int userId, int eventId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task<Rsvp> GetToasterRsvp(int userId, int eventId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task<ICollection<Rsvp>> GetBusinessEventRsvps(int businessId, int eventId);

        #endregion

    }
}
