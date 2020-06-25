using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.Drinks;
using Tabs.Mobile.Shared.Models.Drinks;

namespace Tabs.Mobile.Shared.Managers.Drinks
{
    public class BusinessDrinkFactory : IBusinessDrinkFactory
    {
        #region Constants, Enums, and Variables

        private IBusinessDrinkFactory _BusinessDrinkFactory;

        #endregion

        #region Constructors

        public BusinessDrinkFactory(IBusinessDrinkFactory businessDrinkFactory)
        {
            _BusinessDrinkFactory = businessDrinkFactory;
        }

        #endregion

        #region Methods

        public Task Add(BusinessDrink businessDrink)
        {
            return _BusinessDrinkFactory.Add(businessDrink);
        }

        public Task<ICollection<BusinessDrink>> Get(int businessId)
        {
            return _BusinessDrinkFactory.Get(businessId);
        }

        public Task Update(int drinkId, string drinkName, double price)
        {
            return _BusinessDrinkFactory.Update(drinkId, drinkName, price);
        }

        public Task Delete(int drinkId)
        {
            return _BusinessDrinkFactory.Delete(drinkId);
        }

        #endregion

    }
}
