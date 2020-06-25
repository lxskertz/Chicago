using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.CheckIns;
using Tabs.Mobile.Shared.Models.CheckIns;

namespace Tabs.Mobile.Shared.WebServices
{
    public class CheckInService : BaseService, ICheckInFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkIn"></param>
        /// <returns></returns>
        public async Task<int> CheckIn(CheckIn checkIn)
        {
            string methodPath = "checkin";
            checkIn.BlockedByAdmin = false;
            checkIn.BlockedByAdminUserId = 0;
            int response = 0;
            var parameters = new
            {
                EventId = checkIn.EventId,
                BusinessId = checkIn.BusinessId,
                UserId = checkIn.UserId,
                CheckedIn = checkIn.CheckedIn,
                CheckInDate = checkIn.CheckInDate,
                CheckInType = checkIn.CheckInType,
                IndividualId = checkIn.IndividualId,
                Username = checkIn.Username,
                CheckInDateString = DateTime.Now.ToString(),
                BusinessName = checkIn.BusinessName,
                FirstName = checkIn.FirstName,
                LastName = checkIn.LastName,
                BlockedByAdmin = checkIn.BlockedByAdmin,
                BlockedByAdminUserId = checkIn.BlockedByAdminUserId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, parameters, true, "PUT"));
            response = await request;

            return response;
        }

        public async Task<bool> EventCheckInExist(int userId, int eventId, int checkInType)
        {
            string methodPath = "checkin/exist";
            bool response = false;
            var parameters = new
            {
                EventId = eventId,
                UserId = userId,
                CheckInType = checkInType
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<bool>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public async Task<ICollection<CheckIn>> GetCheckIns(int userId)
        {
            string methodPath = "checkin/" + userId;
            ICollection<CheckIn> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<CheckIn>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<ICollection<CheckIn>> GetBusinessCheckIns(int businessId)
        {
            string methodPath = "checkin/business/" + businessId;
            ICollection<CheckIn> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<CheckIn>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<ICollection<CheckIn>> GetEventCheckIns(int eventId)
        {
            string methodPath = "checkin/event/" + eventId;
            ICollection<CheckIn> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<CheckIn>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        #endregion

    }
}
