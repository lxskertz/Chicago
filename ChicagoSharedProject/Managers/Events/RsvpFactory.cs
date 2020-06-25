using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Interfaces.Events;

namespace Tabs.Mobile.Shared.Managers.Events
{
    public class RsvpFactory : IRsvpFactory
    {
        #region Constants, Enums, and Variables

        private IRsvpFactory _RsvpFactory;

        #endregion

        #region Constructors

        public RsvpFactory(IRsvpFactory rsvpFactory)
        {
            _RsvpFactory = rsvpFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rsvp"></param>
        public Task Add(Rsvp rsvp)
        {
            return _RsvpFactory.Add(rsvp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="going"></param>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        public Task Change(bool going, int userId, int eventId)
        {
            return _RsvpFactory.Change(going, userId, eventId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Task<Rsvp> GetToasterRsvp(int userId, int eventId)
        {
            return _RsvpFactory.GetToasterRsvp(userId, eventId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Task<ICollection<Rsvp>> GetBusinessEventRsvps(int businessId, int eventId)
        {
            return _RsvpFactory.GetBusinessEventRsvps(businessId, eventId);
        }

        #endregion

    }
}
