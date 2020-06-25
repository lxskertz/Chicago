using System;

namespace Tabs.Mobile.Shared.Models.Reports.Spams
{
    public class ReportedSpamCheckIn
    {

        public int SpamCheckInId { get; set; }

        public int CheckInId { get; set; }

        public int CheckInUserId { get; set; }

        public string SenderFirstName { get; set; }

        public string SenderLastName { get; set; }

        public int ReporterUserId { get; set; }

        public string ReporterFirstName { get; set; }

        public string ReporterLastName { get; set; }

        public int BusinessId { get; set; }

        public int EventId { get; set; }

        public CheckIns.CheckIn.CheckInTypes CheckInType { get; set; }

        public DateTime? CheckInDate { get; set; }

        public string BusinessName { get; set; }

        public bool BlockedByAdmin { get; set; }

        public int BlockedByAdminUserId { get; set; }

        public string CheckInDateString { get; set; }

    }
}
