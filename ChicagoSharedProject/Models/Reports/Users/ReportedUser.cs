using System;

namespace Tabs.Mobile.Shared.Models.Reports.Users
{
    public class ReportedUser
    {

        public int ReportedUserId { get; set; }

        public string SenderFirstName { get; set; }

        public string SenderLastName { get; set; }

        public int SenderUserId { get; set; }

        public int ReporterUserId { get; set; }

        public string ReporterFirstName { get; set; }

        public string ReporterLastName { get; set; }

        public DateTime? ReportDate { get; set; }

        public string ReportDateString { get; set; }

        public bool BlockedByAdmin { get; set; }

        public int BlockedByAdminUserId { get; set; }

    }
}
