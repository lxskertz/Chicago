using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Interfaces.Individuals;

namespace Tabs.Mobile.Shared.WebServices
{
    public class ToastersService : BaseService, IToastersFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toasters"></param>
        public async Task AddToaster(Toasters toaster)
        {
            string methodPath = "toaster/";
            Toasters response = null;
            //var userOne = toaster.UserOneId < toaster.UserTwoId ? toaster.UserOneId : toaster.UserTwoId;
            //var userTwo = toaster.UserOneId > toaster.UserTwoId ? toaster.UserOneId : toaster.UserTwoId;
            var parameters = new
            {
                UserOneId = toaster.UserOneId,
                UserTwoId = toaster.UserTwoId,
                ActionUserId = toaster.ActionUserId,
                RequestStatus = toaster.RequestStatus

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Toasters>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task AcceptRequest(int userOneId, int userTwoId, int actionUserId, Toasters.ToasterRequestStatus status)
        {
            string methodPath = "toaster/accept";
            Toasters response = null;
            var parameters = new
            {
                UserOneId = userOneId,
                UserTwoId = userTwoId,
                ActionUserId = actionUserId,
                Status = status
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Toasters>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toasterId"></param>
        public async Task RemoveToaster(int toasterId)
        {
            string methodPath = "toaster/" + toasterId;
            HttpRequestMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, null, true, "DELETE"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public async Task<Toasters> Get(int individualId)
        {
            string methodPath = "toaster/" + individualId;
            Toasters response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Toasters>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<int> GetTotalToastersCount(int userId)
        {
            string methodPath = "toaster/count/" + userId;
            int response = 0;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ICollection<Toasters>> GetBlockedToasters(int userId)
        {
            string methodPath = "toaster/blocked/" + userId;
            ICollection<Toasters> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Toasters>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ICollection<Toasters>> GetToasters(SearchParameters param)
        {
            string methodPath = "toaster/all/";
            ICollection<Toasters> response = new List<Toasters>();
            var parameters = new
            {
                SearchTerm = param.SearchTerm,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                Id = param.Id
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Toasters>>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ICollection<Toasters>> SearchToasters(SearchParameters param)
        {
            string methodPath = "toaster/search/";
            ICollection<Toasters> response = new List<Toasters>();
            var parameters = new
            {
                SearchTerm = param.SearchTerm,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                Id = param.Id
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Toasters>>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="toasterUserIndividualId"></param>
        /// <returns></returns>
        public async Task<Toasters> Connected(int userOneId, int userTwoId)
        {
            string methodPath = "toaster/connected/";
            Toasters response = new Toasters();
            //var userOne = userOneId < userTwoId ? userOneId : userTwoId;
            //var userTwo = userOneId > userTwoId ? userOneId : userTwoId;
            var parameters = new
            {
                UserOneId = userOneId,
                UserTwoId = userTwoId,
                ActionUserId = 0,
                Status = -1
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Toasters>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <returns></returns>
        public async Task BlockToaster(int userOneId, int userTwoId, int actionUserId)
        {
            string methodPath = "toaster/block/";
            HttpRequestMessage response = null;
            var parameters = new
            {
                UserOneId = userOneId,
                UserTwoId = userTwoId,
                ActionUserId = actionUserId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <returns></returns>
        public async Task UnfollowToaster(int userOneId, int userTwoId, int actionUserId)
        {
            string methodPath = "toaster/unfollow/";
            HttpRequestMessage response = null;
            var parameters = new
            {
                UserOneId = userOneId,
                UserTwoId = userTwoId,
                ActionUserId = actionUserId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <returns></returns>
        public async Task UnBlockToaster(int toasterId)
        {
            string methodPath = "toaster/unblock/" + toasterId;
            HttpRequestMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, null, true, "GET"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ICollection<Toasters>> GetPendingToasters(SearchParameters param)
        {
            string methodPath = "toaster/pending/";
            ICollection<Toasters> response = new List<Toasters>();
            var parameters = new
            {
                SearchTerm = param.SearchTerm,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                Id = param.Id
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Toasters>>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        #endregion

    }
}
