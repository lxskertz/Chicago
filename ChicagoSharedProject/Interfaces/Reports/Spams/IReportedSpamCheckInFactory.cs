using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Reports.Spams;

namespace Tabs.Mobile.Shared.Interfaces.Reports.Spams
{
    public interface IReportedSpamCheckInFactory
    {
        Task ReportSpam(ReportedSpamCheckIn spamCheckIn);

        //Task<ICollection<ReportedSpamCheckIn>> GetTodaysReports();

        //Task<ICollection<ReportedSpamCheckIn>> GetAll();

    }
}
