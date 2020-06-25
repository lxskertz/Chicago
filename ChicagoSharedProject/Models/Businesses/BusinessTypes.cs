namespace Tabs.Mobile.Shared.Models.Businesses
{
    public class BusinessTypes : BaseModel
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BusinessTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Bar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Lounge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Club { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Restaurant { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Other { get; set; }

        #endregion

    }
}
