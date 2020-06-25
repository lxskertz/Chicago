using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabs.Mobile.Shared.Models.Users
{
    public class Users : BaseModel
    {

        #region Properties

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets Passwordhash
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public long PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the birthday
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Gets or sets if user is business
        /// </summary>
        public bool IsBusiness { get; set; }

        /// <summary>
        /// Gets or sets if user is individual
        /// </summary>
        public bool IsIndividual { get; set; }

        /// <summary>
        /// Gets or sets if user is admin
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Get or sets user status
        /// </summary>
        public bool UserStatus { get; set; }

        /// <summary>
        /// Gets or sets UserMobileSessionId
        /// </summary>
        public string UserMobileSessionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool TabsAdmin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AccountLocked { get; set; }

        #endregion

    }
}
