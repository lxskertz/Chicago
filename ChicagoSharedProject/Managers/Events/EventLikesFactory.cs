using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.Events;
using Tabs.Mobile.Shared.Models.Events;

namespace Tabs.Mobile.Shared.Managers.Events
{
    public class EventLikesFactory : IEventLikesFactory
    {

        #region Constants, Enums, and Variables

        private IEventLikesFactory _EventLikesFactory;

        #endregion

        #region Constructors

        public EventLikesFactory(IEventLikesFactory eventLikesFactory)
        {
            _EventLikesFactory = eventLikesFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventLike"></param>
        /// <returns></returns>
        public Task LikeEvent(EventLikes eventLike)
        {
            return _EventLikesFactory.LikeEvent(eventLike);
        }

        public Task UndoLikedEvent(bool like, int userId, int eventId)
        {
            return _EventLikesFactory.UndoLikedEvent(like, userId, eventId);
        }

        public Task<int> GetLikeCount(int businessId, int eventId)
        {
            return _EventLikesFactory.GetLikeCount(businessId, eventId);
        }

        public Task<EventLikes> GetEventLike(int userId, int eventId)
        {
            return _EventLikesFactory.GetEventLike(userId, eventId);
        }

        #endregion

    }
}
