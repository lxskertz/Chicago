using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Drinks;

namespace Tabs.Mobile.Shared.Interfaces.Drinks
{
    public interface IBusinessDrinkFactory
    {

        Task Add(BusinessDrink businessDrink);

        Task<ICollection<BusinessDrink>> Get(int businessId);

        Task Update(int drinkId, string drinkName, double price);

        Task Delete(int drinkId);

    }
}
