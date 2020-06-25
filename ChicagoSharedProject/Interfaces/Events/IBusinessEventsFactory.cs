using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Events;

namespace Tabs.Mobile.Shared.Interfaces.Events
{
    public interface IBusinessEventsFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessEvents"></param>
        Task<int> Add(BusinessEvents businessEvents);

        Task Update(BusinessEvents businessEvent);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        Task<ICollection<BusinessEvents>> Get(int businessId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ICollection<EventCategory>> GetEventCategories();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ICollection<EventType>> GetEventTypes();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task Delete(int eventId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        Task<ICollection<BusinessEvents>> GetLiveEvents(string zipCode, string city, int pageNumber, int pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        Task<ICollection<BusinessEvents>> GetUpcomingEvents(string zipCode, string city, int pageNumber, int pageSize, string searchTerm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        Task<ICollection<BusinessEvents>> Search(string zipCode, string city, int pageNumber, int pageSize, string searchTerm);

        #endregion

    }
}
