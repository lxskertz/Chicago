using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Interfaces.Events;

namespace Tabs.Mobile.Shared.WebServices
{
    public class RsvpService : BaseService, IRsvpFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rsvp"></param>
        public async Task Add(Rsvp rsvp)
        {
            string methodPath = "event/rsvp/";
            Rsvp response = null;
            var parameters = new
            {
                EventId = rsvp.EventId,
                BusinessId = rsvp.BusinessId,
                UserId = rsvp.UserId,
                Going = rsvp.Going
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Rsvp>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="going"></param>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        public async Task Change(bool going, int userId, int eventId)
        {
            string methodPath = "event/rsvp/change/";
            Rsvp response = null;
            var parameters = new
            {
                Going = going,
                UserId = userId,
                EventId = eventId,
                BusinessId = 0
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Rsvp>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<Rsvp> GetToasterRsvp(int userId, int eventId)
        {
            string methodPath = "event/rsvp/toaster/";
            Rsvp response = null;
            var parameters = new
            {
                Going = false,
                UserId = userId,
                EventId = eventId,
                BusinessId = 0
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Rsvp>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<ICollection<Rsvp>> GetBusinessEventRsvps(int businessId, int eventId)
        {
            string methodPath = "event/rsvp/business/";
            ICollection<Rsvp> response = null;
            var parameters = new
            {
                Going = false,
                UserId = 0,
                EventId = eventId,
                BusinessId = businessId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Rsvp>>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        #endregion

    }
}
