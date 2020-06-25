namespace Tabs.Mobile.Shared.Models.Individuals
{
    public class SearchParameters
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        #endregion

    }
}
