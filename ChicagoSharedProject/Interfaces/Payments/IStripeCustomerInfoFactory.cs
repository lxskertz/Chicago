using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Payment;

namespace Tabs.Mobile.Shared.Interfaces.Payments
{
    public interface IStripeCustomerInfoFactory
    {

        Task<int> Add(StripeCustomerInfo stripeCustomerInfo);

        Task<StripeCustomerInfo> Get(int userId);

        Task Delete(int stripeCustomerInfo);

    }
}
