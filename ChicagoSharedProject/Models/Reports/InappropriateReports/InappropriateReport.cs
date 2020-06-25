using System;

namespace Tabs.Mobile.Shared.Models.Reports.InappropriateReports
{
    public class InappropriateReport
    {

        public const string Nudity = "Nudity";
        public const string Pornography = "Pornography";
        public const string Harmful = "It's harmful";
        public const string Bullying = "Displays bullying";
        public const string Abusive = "It's abusive";
        public const string Copyright = "Copyright violation";
        public const string HateSpeech = "Shows racist symbols";
        public const string Drugs = "Display of drugs";
        public const string Firearms = "Shows firearms";
        public const string JustHating = "Makes me feel a way";

        public enum ReportReason
        {
            Nudity,
            Pornography,
            Harmful,
            Bullying,
            Abusive,
            Copyright,
            HateSpeech,
            Drugs,
            Firearms,
            JustHating
        }

        public int InappropriateCheckInId { get; set; }

        public int CheckInId { get; set; }

        public int CheckInUserId { get; set; }

        public int ReporterUserId { get; set; }

        public string ReporterFirstName { get; set; }

        public string ReporterLastName { get; set; }

        public string SenderFirstName { get; set; }

        public string SenderLastName { get; set; }

        public int BusinessId { get; set; }

        public int EventId { get; set; }

        public CheckIns.CheckIn.CheckInTypes CheckInType { get; set; }

        public DateTime? CheckInDate { get; set; }

        public string BusinessName { get; set; }

        public bool BlockedByAdmin { get; set; }

        public int BlockedByAdminUserId { get; set; }

        public string CheckInDateString { get; set; }

        public ReportReason CheckInReportReason { get; set; }

    }
}
