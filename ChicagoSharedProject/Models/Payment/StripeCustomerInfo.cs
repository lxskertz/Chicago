using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models.Payment
{
    public class StripeCustomerInfo : BaseModel
    {

        public int StripeCustomerInfoId { get; set; }

        public string StripeCustomerId { get; set; }

    }
}
