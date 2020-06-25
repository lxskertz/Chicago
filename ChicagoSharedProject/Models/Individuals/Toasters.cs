namespace Tabs.Mobile.Shared.Models.Individuals
{
    public class Toasters : BaseModel
    {
        public enum ToasterRequestStatus
        {
            Pending = 0,
            Accepted = 1,
            Declined = 2,
            Blocked = 3,
            Unfollowed = 4
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int ToastersId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public int FromIndividualId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public int ToIndividualId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserOneId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserTwoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ActionUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ToasterRequestStatus RequestStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public bool RequestApproved { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LastName { get; set; }

        #endregion

    }
}
