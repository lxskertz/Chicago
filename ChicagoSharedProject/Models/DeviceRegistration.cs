using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models
{
    public class DeviceRegistration
    {

        #region constants, Enums, and Variables

        public const string Fcm = "fcm";
        public const string Apns = "apns";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets registration id
        /// </summary>
        public string RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets platform
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets handle
        /// </summary>
        public string Handle { get; set; }

        /// <summary>
        /// Gets or sets tags
        /// </summary>
        public List<string> Tags { get; set; }

        #endregion

    }
}
