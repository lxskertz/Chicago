using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Interfaces.Events;

namespace Tabs.Mobile.Shared.Managers.Events
{
    public class BusinessEventsFactory : IBusinessEventsFactory
    {

        #region Constants, Enums, and Variables

        private IBusinessEventsFactory _BusinessEventsFactory;

        #endregion

        #region Constructors

        public BusinessEventsFactory(IBusinessEventsFactory businessEventsFactory)
        {
            _BusinessEventsFactory = businessEventsFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessEvents"></param>
        public Task<int> Add(BusinessEvents businessEvents)
        {
            return _BusinessEventsFactory.Add(businessEvents);
        }

        public Task Update(BusinessEvents businessEvent)
        {
            return _BusinessEventsFactory.Update(businessEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public Task<ICollection<BusinessEvents>> Get(int businessId)
        {
            return _BusinessEventsFactory.Get(businessId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<EventCategory>> GetEventCategories()
        {
            return _BusinessEventsFactory.GetEventCategories();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<EventType>> GetEventTypes()
        {
            return _BusinessEventsFactory.GetEventTypes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Task Delete(int eventId)
        {
            return _BusinessEventsFactory.Delete(eventId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public Task<ICollection<BusinessEvents>> GetLiveEvents(string zipCode, string city, int pageNumber, int pageSize)
        {
            return _BusinessEventsFactory.GetLiveEvents(zipCode, city, pageNumber, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public Task<ICollection<BusinessEvents>> GetUpcomingEvents(string zipCode, string city, int pageNumber, int pageSize, string searchTerm)
        {
            return _BusinessEventsFactory.GetUpcomingEvents(zipCode, city, pageNumber, pageSize, searchTerm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public Task<ICollection<BusinessEvents>> Search(string zipCode, string city, int pageNumber, int pageSize, string searchTerm)
        {
            return _BusinessEventsFactory.Search(zipCode, city, pageNumber, pageSize, searchTerm);
        }

        #endregion

    }
}
