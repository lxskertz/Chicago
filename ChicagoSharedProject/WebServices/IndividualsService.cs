using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Interfaces.Individuals;

namespace Tabs.Mobile.Shared.WebServices
{
    public class IndividualsService : BaseService, IIndividualFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individual"></param>
        public async Task CreateIndividual(Individual individual)
        {
            string methodPath = "individual/create/";
            HttpResponseMessage response = null;
            var parameters = new
            {
                UserId = individual.UserId,
                Headline = string.Empty,
                HomeTown = string.Empty,
                ProfileDescription = string.Empty,
                FavoriteBusinesses = string.Empty

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, false, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ICollection<ToastersSearchItem>> ToasterSearch(SearchParameters param)
        {
            string methodPath = "individual/search/";
            ICollection<ToastersSearchItem> response = new List<ToastersSearchItem>();
            var parameters = new
            {
                SearchTerm = param.SearchTerm,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<ToastersSearchItem>>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Individual> GetToasterByUserId(int userId)
        {
            string methodPath = "individual/by_userid/" + userId;
            Individual response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Individual>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public async Task<Individual> GetToaster(int individualId)
        {
            string methodPath = "individual/" + individualId;
            Individual response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Individual>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individual"></param>
        public async Task EditIndividual(Individual individual)
        {
            string methodPath = "individual/";
            HttpRequestMessage response = null;
            var parameters = new
            {
                EmaIndividualIdil = individual.IndividualId,
                FavoriteBusinesses = individual.FavoriteBusinesses,
                Female = individual.Female,
                Headline = individual.Headline,
                HomeTown = individual.HomeTown,
                Male = individual.Male,
                NewToastRequest = individual.NewToastRequest,
                OtherSex = individual.OtherSex,
                PrivateAccount = individual.PrivateAccount,
                ProfileDescription = individual.ProfileDescription,
                UserId = individual.UserId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, individual, true, "PUT"));
            response = await request;
        }

        #endregion

    }
}
