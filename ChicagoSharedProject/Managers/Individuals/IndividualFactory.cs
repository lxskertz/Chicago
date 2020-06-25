using System.Threading.Tasks;
using System.Collections.Generic;
using Tabs.Mobile.Shared.Interfaces.Individuals;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.Shared.Managers.Individuals
{
    public class IndividualFactory : IIndividualFactory
    {

        #region Constants, Enums, and Variables

        private IIndividualFactory _IndividualFactory;

        #endregion

        #region Constructors

        public IndividualFactory(IIndividualFactory individualFactory)
        {
            _IndividualFactory = individualFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individual"></param>
        public Task CreateIndividual(Individual individual)
        {
            return this._IndividualFactory.CreateIndividual(individual);
        }

        /// <summary>
        /// 
        /// </summary>
        public Task<ICollection<ToastersSearchItem>> ToasterSearch(SearchParameters param)
        {
            return this._IndividualFactory.ToasterSearch(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<Individual> GetToasterByUserId(int userId)
        {
            return _IndividualFactory.GetToasterByUserId(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Task<Individual> GetToaster(int individualId)
        {
            return _IndividualFactory.GetToaster(individualId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individual"></param>
        public Task EditIndividual(Individual individual)
        {
            return _IndividualFactory.EditIndividual(individual);
        }

        #endregion

    }
}