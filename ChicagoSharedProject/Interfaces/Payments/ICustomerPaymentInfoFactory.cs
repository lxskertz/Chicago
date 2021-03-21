using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;
using Tabs.Mobile.Shared.Models.Payment;

namespace Tabs.Mobile.Shared.Interfaces.Payments
{
    public interface ICustomerPaymentInfoFactory
    {

        Task CreateCustomerPaymentInfo(CustomerPaymentInfo customerPaymentInfo);

        Task AddCard(CustomerPaymentInfo customerPaymentInfo, string customerId);

        Task DeleteCard(string cardId, string customerId);

        Task<List<Card>> GetAllCards(string customerId);

        Task<Charge> ChargeCustomer(string customerId, double amount, string cardToken, string businessName);

        Task ChangeDefaultPayment(string customerId, string cardToken);

        Task<Customer> GetCustomer(string customberId);

        Task<Card> GetDefaultCard(string customerId, string cardID);

        void Initialize(string key);

    }
}
