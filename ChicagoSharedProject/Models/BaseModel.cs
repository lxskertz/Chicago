using System;

namespace Tabs.Mobile.Shared.Models
{
    public class BaseModel
    {

        #region Properties

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or ests date created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or ests date updated
        /// </summary>
        public DateTime DateUpdated { get; set; }

        #endregion

    }
}
