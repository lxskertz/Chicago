using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Payment;
using Tabs.Mobile.Shared.Interfaces.Payments;

namespace Tabs.Mobile.Shared.Managers.Payments
{
    public class StripeCustomerInfoFactory : IStripeCustomerInfoFactory
    {
        #region Constants, Enums, and Variables

        private IStripeCustomerInfoFactory _StripeCustomerInfoFactory;

        #endregion

        #region Constructors

        public StripeCustomerInfoFactory(IStripeCustomerInfoFactory stripeCustomerInfoFactory)
        {
            _StripeCustomerInfoFactory = stripeCustomerInfoFactory;
        }

        #endregion

        #region Methods

        public Task<int> Add(StripeCustomerInfo stripeCustomerInfo)
        {
            return _StripeCustomerInfoFactory.Add(stripeCustomerInfo);
        }

        public Task<StripeCustomerInfo> Get(int userId)
        {
            return _StripeCustomerInfoFactory.Get(userId);
        }

        public Task Delete(int stripeCustomerInfoId)
        {
            return _StripeCustomerInfoFactory.Delete(stripeCustomerInfoId);
        }

        #endregion

    }
}
