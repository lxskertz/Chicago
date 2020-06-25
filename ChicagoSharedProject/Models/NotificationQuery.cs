using System;
using System.Collections.Generic;

namespace Tabs.Mobile.Shared.Models
{
    public class NotificationQuery
    {

        #region Properties

        /// <summary>
        /// Get or sets message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or set pns
        /// </summary>
        public string PNS { get; set; }

        /// <summary>
        /// Gets or sets tags
        /// </summary>
        public List<string> Tags { get; set; }

        #endregion

    }
}
