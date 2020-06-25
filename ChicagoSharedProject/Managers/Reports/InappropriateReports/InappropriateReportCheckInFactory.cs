using System;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Reports.InappropriateReports;
using Tabs.Mobile.Shared.Interfaces.Reports.InappropriateReports;

namespace Tabs.Mobile.Shared.Managers.Reports.InappropriateReports
{
    public class InappropriateReportCheckInFactory : IInappropriateReportCheckInFactory
    {
        #region Constants, Enums, and Variables

        private IInappropriateReportCheckInFactory _InappropriateReportCheckInFactory;

        #endregion

        #region Constructors

        public InappropriateReportCheckInFactory(IInappropriateReportCheckInFactory inappropriateReportCheckInFactory)
        {
            _InappropriateReportCheckInFactory = inappropriateReportCheckInFactory;
        }

        #endregion

        #region Methods

        public Task ReportInappropriate(InappropriateReport inappropriate)
        {
            return _InappropriateReportCheckInFactory.ReportInappropriate(inappropriate);
        }

        #endregion

    }
}
