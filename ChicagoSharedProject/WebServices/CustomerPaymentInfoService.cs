//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Stripe;
//using Tabs.Mobile.Shared.Models.Payment;
//using Tabs.Mobile.Shared.Interfaces.Payments;
//using Tabs.Mobile.Shared.Interfaces.Users;

//namespace Tabs.Mobile.Shared.WebServices
//{
//    public class CustomerPaymentInfoService : BaseService, ICustomerPaymentInfoFactory
//    {

//        #region Constants, ENums, and Variables

//        private string stripeDevKey = "pk_test_LmDl4S0oCkGp9WKzWJr0R5Ho";

//        private readonly IUserFactory UserFactory;
//        private readonly IStripeCustomerInfoFactory StripeCustomerInfoFactory;

//        #endregion

//        #region Constructors

//        public CustomerPaymentInfoFactory(IUserFactory userFactory,
//            IStripeCustomerInfoFactory stripeCustomerInfoFactory)
//        {
//            UserFactory = userFactory;
//            StripeCustomerInfoFactory = stripeCustomerInfoFactory;
//            StripeConfiguration.SetApiKey(stripeDevKey);
//        }

//        #endregion

//        #region Methods

//        public async Task CreateCustomerPaymentInfo(CustomerPaymentInfo customerPaymentInfo)
//        {
//            var user = await this.UserFactory.GetUser(customerPaymentInfo.Email);
//            string fname = "";
//            string lname = "";

//            if (user != null)
//            {
//                fname = string.IsNullOrEmpty(user.FirstName) ? "" : user.FirstName;
//                lname = string.IsNullOrEmpty(user.LastName) ? "" : user.LastName;
//            }

//            var customerOptions = new CustomerCreateOptions
//            {
//                Email = customerPaymentInfo.Email,
//                Name = fname + " " + lname,
//                SourceCard = new CardCreateNestedOptions
//                {
//                    Number = customerPaymentInfo.Number,
//                    Cvc = customerPaymentInfo.Cvc,
//                    ExpMonth = customerPaymentInfo.ExpirationMonth,
//                    ExpYear = customerPaymentInfo.ExpirationYear
//                }

//            };
//            var customerService = new CustomerService();
//            Customer customer = await customerService.CreateAsync(customerOptions);

//            if (customer != null)
//            {
//                StripeCustomerInfo info = new StripeCustomerInfo();
//                info.UserId = customerPaymentInfo.UserId;
//                info.StripeCustomerId = customer.Id;

//                var id = await this.StripeCustomerInfoFactory.Add(info);
//            }

//        }

//        public async Task AddCard(CustomerPaymentInfo customerPaymentInfo, string customerId)
//        {
//            var options = new CardCreateOptions
//            {
//                SourceCard = new CardCreateNestedOptions
//                {
//                    Number = customerPaymentInfo.Number,
//                    Cvc = customerPaymentInfo.Cvc,
//                    ExpMonth = customerPaymentInfo.ExpirationMonth,
//                    ExpYear = customerPaymentInfo.ExpirationYear
//                }
//            };
//            var service = new CardService();
//            var card = await service.CreateAsync(customerId, options);
//        }

//        public async Task DeleteCard(string cardId, string customerId)
//        {
//            var service = new CardService();
//            await service.DeleteAsync(customerId, cardId);
//        }

//        public async Task<List<Card>> GetAllCards(string customerId)
//        {
//            var service = new CardService();
//            var options = new CardListOptions
//            {
//                Limit = 4,
//            };
//            var cards = await service.ListAsync(customerId, options);

//            if (cards != null)
//            {
//                return cards.Data;
//            }

//            return new List<Card>();
//        }

//        #endregion

//    }
//}
