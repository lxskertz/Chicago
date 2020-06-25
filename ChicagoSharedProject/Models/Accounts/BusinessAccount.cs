namespace Tabs.Mobile.Shared.Models.Accounts
{
    public class BusinessAccount : BaseModel
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int BusinessAccountId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasMultipleUsers { get; set; }

        #endregion

    }
}
