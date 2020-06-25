using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models.Businesses
{
    public class BusinessSearch : BaseModel
    {

        public bool Club { get; set; }

        public bool Lounge { get; set; }

        public bool Bar { get; set; }

        public string ZipCode { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string StreetAddress { get; set; }

        public bool Restaurant { get; set; }

        public long PhoneNumber { get; set; }

        public string SSN { get; set; }

        public bool AllowReviews { get; set; }

        public bool AccountStatus { get; set; }

        public int AccountAdministratorId { get; set; }

        public bool Available { get; set; }

        public string BusinessDescription { get; set; }

        public string BusinessName { get; set; }

        public int BusinessId { get; set; }

        public string EINTaxID { get; set; }

        public bool Other { get; set; }

    }
}
