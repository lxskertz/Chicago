using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Stripe;
using Tabs.Mobile.Shared.Models.Payment;
using Tabs.Mobile.Shared.Interfaces.Payments;
using Tabs.Mobile.Shared.Interfaces.Users;

namespace Tabs.Mobile.Shared.Managers.Payments
{
    public class CustomerPaymentInfoFactory : ICustomerPaymentInfoFactory
    {
        #region Constants, ENums, and Variables
        
        private readonly IUserFactory UserFactory;
        private readonly IStripeCustomerInfoFactory StripeCustomerInfoFactory;

        #endregion

        #region Constructors

        public CustomerPaymentInfoFactory(IUserFactory userFactory, 
            IStripeCustomerInfoFactory stripeCustomerInfoFactory)
        {
            UserFactory = userFactory;
            StripeCustomerInfoFactory = stripeCustomerInfoFactory;
        }

        #endregion

        #region Methods

        public void Initialize(string key)
        {
            StripeConfiguration.SetApiKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerPaymentInfo"></param>
        /// <returns></returns>
        public async Task CreateCustomerPaymentInfo(CustomerPaymentInfo customerPaymentInfo)
        {
            var user = await this.UserFactory.GetUser(customerPaymentInfo.Email, false);
            string fname = "";
            string lname = "";

            if (user != null)
            {
                fname = string.IsNullOrEmpty(user.FirstName) ? "" : user.FirstName;
                lname = string.IsNullOrEmpty(user.LastName) ? "" : user.LastName;
            }

            var tokenOption = new TokenCreateOptions
            {
                Card = new CreditCardOptions
                {
                    Number = customerPaymentInfo.Number,
                    Cvc = customerPaymentInfo.Cvc,
                    ExpMonth = customerPaymentInfo.ExpirationMonth,
                    ExpYear = customerPaymentInfo.ExpirationYear
                }
            };

            var service = new TokenService();
            Token stripeToken = await service.CreateAsync(tokenOption);

            var customerOptions = new CustomerCreateOptions
            {
                Email = customerPaymentInfo.Email,
                Name = fname + " " + lname,
                SourceToken = stripeToken.Id

            };
            var customerService = new CustomerService();
            Customer customer = await customerService.CreateAsync(customerOptions);

            if(customer != null)
            {
                StripeCustomerInfo info = new StripeCustomerInfo();
                info.UserId = customerPaymentInfo.UserId;
                info.StripeCustomerId = customer.Id;

                var id = await this.StripeCustomerInfoFactory.Add(info);
            }

        }

        public async Task ChangeDefaultPayment(string customerId, string cardToken)
        {
            var options = new CustomerUpdateOptions
            {
                SourceToken = cardToken,
            };

            var service = new CustomerService();
            Customer customer = await service.UpdateAsync(customerId, options);
        }

        public async Task<Charge> ChargeCustomer(string customerId, double amount, string cardToken)
        {
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(amount * 100),
                Currency = "usd",
                CustomerId = customerId
            };

            var chargeService = new ChargeService();
            Charge charge = await chargeService.CreateAsync(chargeOptions);

            return charge;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerPaymentInfo"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task AddCard(CustomerPaymentInfo customerPaymentInfo, string customerId)
        {
            var tokenOption = new TokenCreateOptions
            {
                Card = new CreditCardOptions
                {
                    Number = customerPaymentInfo.Number,
                    Cvc = customerPaymentInfo.Cvc,
                    ExpMonth = customerPaymentInfo.ExpirationMonth,
                    ExpYear = customerPaymentInfo.ExpirationYear
                }
            };
            var service = new TokenService();
            Token stripeToken = await service.CreateAsync(tokenOption);

            var options = new CardCreateOptions
            {
                SourceToken = stripeToken.Id
            };

            var cardService = new CardService();
            var card = await cardService.CreateAsync(customerId, options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task DeleteCard(string cardId, string customerId)
        {
            var service = new CardService();
            await service.DeleteAsync(customerId, cardId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<Card>> GetAllCards(string customerId)
        {
            var service = new CardService();
            var options = new CardListOptions
            {
                Limit = 4,
            };
            var cards = await service.ListAsync(customerId, options);

            if(cards != null)
            {
                return cards.Data;
            }

            return new List<Card>();
        }

        public async Task<Customer> GetCustomer(string customberId)
        {
            var service = new CustomerService();
            var customer = await service.GetAsync(customberId);

            return customer;
        }

        public async Task<Card> GetDefaultCard(string customerId, string cardID)
        {
            var service = new CardService();
            var options = new CardListOptions
            {
                Limit = 10,
            };
            var card = await service.GetAsync(customerId, cardID);

            return card;
        }

        #endregion

    }
}
