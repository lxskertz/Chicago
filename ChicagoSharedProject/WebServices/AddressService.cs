using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Interfaces.Businesses;

namespace Tabs.Mobile.Shared.WebServices
{
    public class AddressService : BaseService, IAddressFactory
    {

        #region Methods

        /// <summary>
        /// Add address
        /// </summary>
        /// <param name="address"></param>
        public async Task AddAddress(Address address)
        {
            string methodPath = "address/";
            Address response = null;
            var parameters = new
            {
                StreetAddress = address.StreetAddress,
                Country = address.Country,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode,
                BusinessId = address.BusinessId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Address>(methodPath, parameters, false, "PUT"));
            response = await request;
        }

        /// <summary>
        /// Get address
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public async Task<Address> GetAddressByBusinessId(int businessId)
        {
            string methodPath = "address/business_id/" + businessId;
            Address response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Address>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Get address by zipcode
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public async Task<ICollection<Address>> GetAddressByZipCode(string zipCode)
        {
            string methodPath = "address/zipcode/" + zipCode;
            ICollection<Address> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Address>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Get address by city
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public async Task<ICollection<Address>> GetAddressByCity(string city)
        {
            string methodPath = "address/city/" + city;
            ICollection<Address> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Address>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Get address by state
        /// </summary>
        /// <param name="addressState"></param>
        /// <returns></returns>
        public async Task<ICollection<Address>> GetAddressByState(string addressState)
        {
            string methodPath = "address/state/" + addressState;
            ICollection<Address> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Address>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Get all address
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<Address>> GetAllAddress()
        {
            string methodPath = "address/all/";
            ICollection<Address> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<Address>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// update address
        /// </summary>
        /// <param name="address"></param>
        public async Task UpdateAddress(Address address)
        {
            string methodPath = "address/";
            Address response = null;
            var parameters = new
            {
                StreetAddress = address.StreetAddress,
                Country = address.Country,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode,
                BusinessId = address.BusinessId,
                AddressId = address.AddressId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Address>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        #endregion

    }
}
