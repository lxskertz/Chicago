using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.Shared.Interfaces.Individuals
{
    public interface IToastersFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toasters"></param>
        Task AddToaster(Toasters toasters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toasterId"></param>
        Task RemoveToaster(int toasterId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <param name="status"></param>
        Task AcceptRequest(int userOneId, int userTwoId, int actionUserId, Toasters.ToasterRequestStatus status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ICollection<Toasters>> GetToasters(SearchParameters param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ICollection<Toasters>> SearchToasters(SearchParameters param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ICollection<Toasters>> GetPendingToasters(SearchParameters param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        Task<Toasters> Get(int individualId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="toasterUserIndividualId"></param>
        /// <returns></returns>
        Task<Toasters> Connected(int UserOneId, int UserTwoId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetTotalToastersCount(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <returns></returns>
        Task BlockToaster(int userOneId, int userTwoId, int actionUserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <returns></returns>
        Task UnBlockToaster(int toasterId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ICollection<Toasters>> GetBlockedToasters(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <returns></returns>
        Task UnfollowToaster(int userOneId, int userTwoId, int actionUserId);

        #endregion

    }
}
