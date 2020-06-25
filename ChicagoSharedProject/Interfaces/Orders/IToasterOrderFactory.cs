using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Orders;

namespace Tabs.Mobile.Shared.Interfaces.Orders
{
    public interface IToasterOrderFactory
    {

        Task New(ToasterOrder toasterOrder);

        Task Charged(int toasterOrderId);

        Task Cancel(int toasterOrderId);

        Task<ToasterOrder> GetByOrderId(int toasterOrderId);

        Task<ICollection<ToasterOrder>> GetBusinessOrders(int businessId);

        Task<ICollection<ToasterOrder>> GetUserOrders(int userId);

        Task<ICollection<ToasterOrder>> GetReceivedDrinks(int userId);

    }
}
