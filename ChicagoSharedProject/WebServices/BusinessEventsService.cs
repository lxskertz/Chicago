using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Interfaces.Events;

namespace Tabs.Mobile.Shared.WebServices
{
    public class BusinessEventsService : BaseService, IBusinessEventsFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessEvents"></param>
        public async Task<int> Add(BusinessEvents businessEvents)
        {
            string methodPath = "business/event/";
            int response = 0;
            var parameters = new
            {
                StreetAddress = businessEvents.StreetAddress,
                Country = businessEvents.Country,
                City = businessEvents.City,
                State = businessEvents.State,
                ZipCode = businessEvents.ZipCode,
                BusinessId = businessEvents.BusinessId,
                EventCategoryId = businessEvents.EventCategoryId,
                EventDescription = businessEvents.EventDescription,
                EventTypeId = businessEvents.EventTypeId,
                Free = businessEvents.Free,
                Paid = businessEvents.Paid,
                PrivateEvent = businessEvents.PrivateEvent,
                //StartDatestring = DateTime.Now.ToShortDateString(),
                //StartTimestring = businessEvents.StartTimestring,
                StartDateTimeString = businessEvents.StartDateTimeString,
                EndDateTimeString = businessEvents.EndDateTimeString,
                Title = businessEvents.Title,
                Venue = businessEvents.Venue,
                Active = true
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, parameters, true, "PUT"));
            response = await request;

            return response;
        }

        public async Task Update(BusinessEvents businessEvents)
        {
            string methodPath = "business/event/";
            HttpResponseMessage response = null;
            var parameters = new
            {
                EventId = businessEvents.EventId,
                StreetAddress = businessEvents.StreetAddress,
                City = businessEvents.City,
                State = businessEvents.State,
                ZipCode = businessEvents.ZipCode,
                BusinessId = businessEvents.BusinessId,
                EventCategoryId = businessEvents.EventCategoryId,
                EventDescription = businessEvents.EventDescription,
                EventTypeId = businessEvents.EventTypeId,
                Free = businessEvents.Free,
                Paid = businessEvents.Paid,
                PrivateEvent = businessEvents.PrivateEvent,
                StartDateTimeString = businessEvents.StartDateTimeString,
                EndDateTimeString = businessEvents.EndDateTimeString,
                //EndDatestring = businessEvents.EndDatestring,
                //EndTimestring = businessEvents.EndTimestring,
                Title = businessEvents.Title,
                Venue = businessEvents.Venue
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public async Task<ICollection<BusinessEvents>> GetLiveEvents(string zipCode, string city, int pageNumber, int pageSize)
        {
            string methodPath = "business/event/live";
            ICollection<BusinessEvents> response = null;
            var parameters = new
            {
                Zipcode = zipCode,
                City = city,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<BusinessEvents>>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public async Task<ICollection<BusinessEvents>> GetUpcomingEvents(string zipCode, string city, int pageNumber, int pageSize, string searchTerm)
        {
            string methodPath = "business/event/upcoming";
            ICollection<BusinessEvents> response = null;
            var parameters = new
            {
                Zipcode = zipCode,
                City = city,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<BusinessEvents>>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public async Task<ICollection<BusinessEvents>> Search(string zipCode, string city, int pageNumber, int pageSize, string searchTerm)
        {
            string methodPath = "business/event/search";
            ICollection<BusinessEvents> response = null;
            var parameters = new
            {
                Zipcode = zipCode,
                City = city,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<BusinessEvents>>(methodPath, parameters, true, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public async Task<ICollection<BusinessEvents>> Get(int businessId)
        {
            string methodPath = "business/event/" + businessId;
            ICollection<BusinessEvents> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<BusinessEvents>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task Delete(int eventId)
        {
            string methodPath = "business/event/" + eventId;
            HttpResponseMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, null, true, "DELETE"));
            response = await request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<EventCategory>> GetEventCategories()
        {
            string methodPath = "business/event/categories";
            ICollection<EventCategory> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<EventCategory>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<EventType>> GetEventTypes()
        {
            string methodPath = "business/event/types";
            ICollection<EventType> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<EventType>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        #endregion

    }
}
