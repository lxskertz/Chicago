using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Interfaces.Businesses;

namespace Tabs.Mobile.Shared.Managers.Businesses
{
    public class AddressFactory : IAddressFactory
    {
        #region Constants, Enums, and Variables

        private IAddressFactory _AddressFactory;

        #endregion

        #region Constructors

        public AddressFactory(IAddressFactory addressFactory)
        {
            _AddressFactory = addressFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add address
        /// </summary>
        /// <param name="address"></param>
        public Task AddAddress(Address address)
        {
            return _AddressFactory.AddAddress(address);
        }

        /// <summary>
        /// Get  address
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public Task<Address> GetAddressByBusinessId(int businessId)
        {
            return _AddressFactory.GetAddressByBusinessId(businessId);
        }

        /// <summary>
        /// Get address by zipcode
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public Task<ICollection<Address>> GetAddressByZipCode(string zipCode)
        {
            return _AddressFactory.GetAddressByZipCode(zipCode);
        }

        /// <summary>
        /// Get address by city
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public Task<ICollection<Address>> GetAddressByCity(string city)
        {
            return _AddressFactory.GetAddressByCity(city);
        }

        /// <summary>
        /// Get address by state
        /// </summary>
        /// <param name="addressState"></param>
        /// <returns></returns>
        public Task<ICollection<Address>> GetAddressByState(string addressState)
        {
            return _AddressFactory.GetAddressByState(addressState);
        }

        /// <summary>
        /// Get all address
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<Address>> GetAllAddress()
        {
            return _AddressFactory.GetAllAddress();
        }

        /// <summary>
        /// update address
        /// </summary>
        /// <param name="address"></param>
        public Task UpdateAddress(Address address)
        {
            return _AddressFactory.UpdateAddress(address);
        }

        #endregion

    }
}
