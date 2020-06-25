using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Tabs.Mobile.Shared.Resources
{
    public class LocationHelper
    {

        #region Properties 

        /// <summary>
        /// Gets or sets locator
        /// </summary>
        private IGeolocator Locator { get; set; }

        #endregion

        #region Constructors

        public LocationHelper()
        {
            this.Locator = CrossGeolocator.Current;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get current location
        /// </summary>
        /// <returns></returns>
        private async Task<Position> GetCurrentLocation()
        {
            Position position = null;
            try
            {
                this.Locator.DesiredAccuracy = 10;

                if (!this.Locator.IsGeolocationAvailable || !this.Locator.IsGeolocationEnabled)
                {
                    return null;
                }

                position = await this.Locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

                return position;

            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return null;

        }

        /// <summary>
        /// Get current location
        /// </summary>
        /// <returns></returns>
        private async Task<Position> GetLastKnownLocation()
        {
            Position position = null;
            try
            {
                this.Locator.DesiredAccuracy = 10;

                if (!this.Locator.IsGeolocationAvailable || !this.Locator.IsGeolocationEnabled)
                {
                    return null;
                }

                position = await this.Locator.GetLastKnownLocationAsync();

                return position;

            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return null;

        }

        public async Task<Address> GetLastKnownAddress()
        {
            try
            {
                var position = await GetLastKnownLocation();
                if (position != null)
                {
                    var addresses = await this.Locator.GetAddressesForPositionAsync(position, null);
                    var address = addresses.FirstOrDefault();

                    return address;
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return null;
        }

        /// <summary>
        /// Get address
        /// </summary>
        /// <returns></returns>
        public async Task<Address> GetAddress()
        {
            try
            {
                var position = await GetCurrentLocation();
                if (position != null)
                {
                    var addresses = await this.Locator.GetAddressesForPositionAsync(position, null);
                    var address = addresses.FirstOrDefault();

                    return address;
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return null;
        }

        #endregion

    }
}
