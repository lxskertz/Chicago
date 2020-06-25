using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models.Events
{
    public class EventCategory : BaseModel
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int EventCategoryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CategoryName { get; set; }

        #endregion

    }
}
