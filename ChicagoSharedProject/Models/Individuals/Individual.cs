namespace Tabs.Mobile.Shared.Models.Individuals
{
    public class Individual : BaseModel
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int IndividualId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Headline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HomeTown { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProfileDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NewToastRequest { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Male { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Female { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool OtherSex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FavoriteBusinesses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool PrivateAccount { get; set; }

        #endregion

    }
}
