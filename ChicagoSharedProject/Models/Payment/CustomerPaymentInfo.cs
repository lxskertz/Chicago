using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models.Payment
{
    public class CustomerPaymentInfo
    {

        public int UserId { get; set; }

        public string Number { get; set; }

        public int ExpirationYear { get; set; }

        public int ExpirationMonth { get; set; }

        public string Cvc { get; set; }

        public string Email { get; set; }

    }
}
