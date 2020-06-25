using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.Events;
using Tabs.Mobile.Shared.Models.Events;

namespace Tabs.Mobile.Shared.WebServices
{
    public class EventLikesService : BaseService, IEventLikesFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventLike"></param>
        /// <returns></returns>
        public async Task LikeEvent(EventLikes eventLike)
        {
            string methodPath = "business/event/like";
            HttpResponseMessage response = null;
            var parameters = new
            {
                EventId = eventLike.EventId,
                BusinessId = eventLike.BusinessId,
                UserId = eventLike.UserId,
                Liked = eventLike.Liked
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task UndoLikedEvent(bool like, int userId, int eventId)
        {
            string methodPath = "business/event/like/change";
            HttpResponseMessage response = null;
            var parameters = new
            {
                Like = like,
                UserId = userId,
                EventId = eventId,
                BusinessId = 0,
                IndividualId = 0
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<int> GetLikeCount(int businessId, int eventId)
        {
            string methodPath = "business/event/like/count";
            int response = 0;
            var parameters = new
            {
                Like = false,
                UserId = 0,
                EventId = eventId,
                BusinessId = businessId,
                IndividualId = 0
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, parameters, true, "POST"));
            response = await request;
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<EventLikes> GetEventLike(int userId, int eventId)
        {
            string methodPath = "business/event/like/liked";
            EventLikes response = null;
            var parameters = new
            {
                Like = false,
                UserId = userId,
                EventId = eventId,
                BusinessId = 0,
                IndividualId = 0
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<EventLikes>(methodPath, parameters, true, "POST"));
            response = await request;
            return response;
        }

        #endregion
    }
}
