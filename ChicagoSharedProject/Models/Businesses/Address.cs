namespace Tabs.Mobile.Shared.Models.Businesses
{
    public class Address : BaseModel
    {

        #region Properties

        /// <summary>
        /// Gets or sets Address id
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Gets or sets the street address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zipcode
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the business id
        /// </summary>
        public int BusinessId { get; set; }

        #endregion

    }
}
