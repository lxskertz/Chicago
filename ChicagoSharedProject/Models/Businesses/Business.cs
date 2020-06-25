
namespace Tabs.Mobile.Shared.Models.Businesses
{
    public class Business : BaseModel
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BusinessDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Gets or sets AccountAdministratorId
        /// </summary>
        public int AccountAdministratorId { get; set; }

        /// <summary>
        /// Gets or sets IsAccountAdministrator
        /// </summary>
        public bool AccountStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowReviews { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EINTaxID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PhoneNumber { get; set; }

        //public double SalesTaxRate { get; set; }

        #endregion

    }
}
