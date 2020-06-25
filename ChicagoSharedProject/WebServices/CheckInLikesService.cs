using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Tabs.Mobile.Shared.Interfaces.CheckIns;
using Tabs.Mobile.Shared.Models.CheckIns;

namespace Tabs.Mobile.Shared.WebServices
{
    public class CheckInLikesService : BaseService, ICheckInLikesFactory
    {

        #region Methods

        public async Task LikeChecKin(CheckInLikes checkInLike)
        {
            string methodPath = "checkin/like";
            HttpResponseMessage response = null;
            var parameters = new
            {
                BusinessId = checkInLike.BusinessId,
                UserId = checkInLike.UserId,
                Liked = checkInLike.Liked,
                CheckInId = checkInLike.CheckInId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        public async Task UndoLikedCheckIn(bool like, int userId, int checkInLikeId)
        {
            string methodPath = "checkin/like/change";
            HttpResponseMessage response = null;
            var parameters = new
            {
                Like = like,
                UserId = userId,
                CheckInId = 0,
                CheckInLikeId = checkInLikeId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        public async Task<CheckInLikes> GetCheckInLike(int userId, int checkInId)
        {
            string methodPath = "checkin/like/liked";
            CheckInLikes response = null;
            var parameters = new
            {
                Like = false,
                UserId = userId,
                CheckInId = checkInId,
                CheckInLikeId = 0
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<CheckInLikes>(methodPath, parameters, true, "POST"));
            response = await request;
            return response;
        }

        public async Task<int> GetLikeCount(int checkInId)
        {
            string methodPath = "checkin/like/count/" + checkInId;
            int response = 0;
            //var parameters = new
            //{
            //    Like = false,
            //    UserId = userId,
            //    CheckInId = checkInId,
            //    CheckInLikeId = 0
            //};
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, null, true, "GET"));
            response = await request;
            return response;
        }

        #endregion

    }
}
