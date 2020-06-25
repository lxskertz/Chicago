using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models.Events
{
    public class BusinessEvents : BaseModel
    {

        #region Constants, Enums, and Variables

        public enum ActionMode
        {
            Add = 0,
            Edit = 1
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EventDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EventCategoryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EventTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the street address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Free { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Paid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool PrivateEvent { get; set; }

        /// <summary>
        /// Gets or sets the state
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StartDateTimeString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EndDateTimeString { get; set; }

        /// <summary>
        /// Gets or sets the zipcode
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? LikeCount { get; set; }

        #endregion

    }
}
