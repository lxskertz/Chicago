using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Orders;
using Tabs.Mobile.Shared.Interfaces.Orders;

namespace Tabs.Mobile.Shared.Managers.Orders
{
    public class ToasterOrderFactory : IToasterOrderFactory
    {

        #region Constants, Enums, and Variables

        private IToasterOrderFactory _ToasterOrderFactory;

        #endregion

        #region Constructors

        public ToasterOrderFactory(IToasterOrderFactory toasterOrderFactory)
        {
            _ToasterOrderFactory = toasterOrderFactory;
        }

        #endregion

        #region Methods

        public Task New(ToasterOrder toasterOrder)
        {
            return _ToasterOrderFactory.New(toasterOrder);
        }

        public Task Charged(int toasterOrderId)
        {
            return _ToasterOrderFactory.Charged(toasterOrderId);
        }

        public Task<ICollection<ToasterOrder>> GetBusinessOrders(int businessId)
        {
            return _ToasterOrderFactory.GetBusinessOrders(businessId);
        }

        public Task<ICollection<ToasterOrder>> GetUserOrders(int userId)
        {
            return _ToasterOrderFactory.GetUserOrders(userId);
        }

        public Task<ICollection<ToasterOrder>> GetReceivedDrinks(int userId)
        {
            return _ToasterOrderFactory.GetReceivedDrinks(userId);
        }

        public Task Cancel(int toasterOrderId)
        {
            return _ToasterOrderFactory.Cancel(toasterOrderId);
        }

        public Task<ToasterOrder> GetByOrderId(int toasterOrderId)
        {
            return _ToasterOrderFactory.GetByOrderId(toasterOrderId);
        }

        #endregion

    }
}
