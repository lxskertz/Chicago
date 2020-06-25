using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.Shared.Interfaces.Businesses
{
    public interface IAddressFactory
    {

        #region Methods

        /// <summary>
        /// Add address
        /// </summary>
        /// <param name="address"></param>
        Task AddAddress(Address address);

        /// <summary>
        /// Get  address
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        Task<Address> GetAddressByBusinessId(int businessId);

        /// <summary>
        /// Get address by zipcode
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        Task<ICollection<Address>> GetAddressByZipCode(string zipCode);

        /// <summary>
        /// Get address by city
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        Task<ICollection<Address>> GetAddressByCity(string city);

        /// <summary>
        /// Get address by state
        /// </summary>
        /// <param name="addressState"></param>
        /// <returns></returns>
        Task<ICollection<Address>> GetAddressByState(string addressState);

        /// <summary>
        /// Get all address
        /// </summary>
        /// <returns></returns>
        Task<ICollection<Address>> GetAllAddress();

        /// <summary>
        /// update address
        /// </summary>
        /// <param name="address"></param>
        Task UpdateAddress(Address address);

        #endregion

    }
}
