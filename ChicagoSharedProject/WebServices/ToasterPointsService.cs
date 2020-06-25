using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Points;
using Tabs.Mobile.Shared.Interfaces.Points;

namespace Tabs.Mobile.Shared.WebServices
{
    public class ToasterPointsService : BaseService, IToasterPointsFactory
    {

        #region Methods

        public async Task NewDailyPoint(Point point)
        {
            string methodPath = "point/toaster/";
            HttpResponseMessage response = null;
            var parameters = new
            {
                UserId = point.UserId,
                EarnedDate = DateTime.Now,
                PointAmount = point.PointAmount,
                PointStatus = point.PointStatus,
                RedeemedDate = DateTime.Now,
                EarnedDateString = DateTime.Now.ToShortDateString(),
                RedeemedDateString = DateTime.Now.ToShortDateString()
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        public async Task MarkAsRedeemed(int toasterOrderId)
        {
            string methodPath = "point/toaster/redeem/" + toasterOrderId;
            HttpResponseMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, null, true, "GET"));
            response = await request;
        }

        public async Task<ICollection<Point>> GetEarnedPoints(int userId)
        {
            string methodPath = "point/toaster/earned/" + userId;
            ICollection<Point> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Point>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<ICollection<Point>> GetRedeemedPoints(int userId)
        {
            string methodPath = "point/toaster/redeemed/" + userId;
            ICollection<Point> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Point>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<int> GetTotalEarnedPoints(int userId)
        {
            string methodPath = "point/toaster/earned_points/" + userId;
            int response = 0;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<int> GetTotalRedeemedPoints(int userId)
        {
            string methodPath = "point/toaster/redeemed_points/" + userId;
            int response = 0;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        #endregion

    }
}
