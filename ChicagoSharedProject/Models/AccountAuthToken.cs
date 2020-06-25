using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models
{
    public class AccountAuthToken
    {

        #region Properties

        public string Username { get; set; }
        public string ApiId { get; set; }
        public string ApiKey { get; set; }

        public Models.Users.Users CurrentUser { get; set; }

        #endregion

    }
}
