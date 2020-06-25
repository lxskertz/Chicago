using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Reports.InappropriateReports;

namespace Tabs.Mobile.Shared.Interfaces.Reports.InappropriateReports
{
    public interface IInappropriateReportCheckInFactory
    {

        Task ReportInappropriate(InappropriateReport inappropriate);

    }
}
