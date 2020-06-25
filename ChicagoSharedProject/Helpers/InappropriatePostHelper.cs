using System;
using System.Collections.Generic;
using Tabs.Mobile.Shared.Models.Reports.InappropriateReports;

namespace Tabs.Mobile.Shared.Helpers
{
    public class InappropriatePostHelper
    {

        public static Dictionary<InappropriateReport.ReportReason, string> ReasonList()
        {
            Dictionary<InappropriateReport.ReportReason, string> reasons = new Dictionary<InappropriateReport.ReportReason, string>();
            reasons.Add(InappropriateReport.ReportReason.Nudity, InappropriateReport.Nudity);
            reasons.Add(InappropriateReport.ReportReason.Pornography, InappropriateReport.Pornography);
            reasons.Add(InappropriateReport.ReportReason.Harmful, InappropriateReport.Harmful);
            reasons.Add(InappropriateReport.ReportReason.Bullying, InappropriateReport.Bullying);
            reasons.Add(InappropriateReport.ReportReason.Abusive, InappropriateReport.Abusive);
            reasons.Add(InappropriateReport.ReportReason.Copyright, InappropriateReport.Copyright);
            reasons.Add(InappropriateReport.ReportReason.HateSpeech, InappropriateReport.HateSpeech);
            reasons.Add(InappropriateReport.ReportReason.Drugs, InappropriateReport.Drugs);
            reasons.Add(InappropriateReport.ReportReason.Firearms, InappropriateReport.Firearms);
            reasons.Add(InappropriateReport.ReportReason.JustHating, InappropriateReport.JustHating);

            return reasons;
        }
      

    }
}
