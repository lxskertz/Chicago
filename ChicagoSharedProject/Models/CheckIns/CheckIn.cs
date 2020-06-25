using System;

namespace Tabs.Mobile.Shared.Models.CheckIns
{
    public class CheckIn : BaseModel
    {

        public enum CheckInTypes
        {
            None = 0,
            Event = 1,
            Business = 2
        }

        public int CheckInId { get; set; }

        public int IndividualId { get; set; }

        public int EventId { get; set; }

        public CheckInTypes CheckInType { get; set; }

        public bool CheckedIn { get; set; }

        public int? LikeCount { get; set; }

        public DateTime? CheckInDate { get; set; }
        
        public int BusinessId { get; set; }

        public string CheckInDateString { get; set; }

        public string BusinessName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool BlockedByAdmin { get; set; }

        public int BlockedByAdminUserId { get; set; }

    }
}
