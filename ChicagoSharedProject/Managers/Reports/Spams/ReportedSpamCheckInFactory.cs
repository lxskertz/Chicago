using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.Reports.Spams;
using Tabs.Mobile.Shared.Models.Reports.Spams;

namespace Tabs.Mobile.Shared.Managers.Reports.Spams
{
    public class ReportedSpamCheckInFactory : IReportedSpamCheckInFactory
    {

        #region Constants, Enums, and Variables

        private IReportedSpamCheckInFactory _ReportedSpamCheckInFactory;

        #endregion

        #region Constructors

        public ReportedSpamCheckInFactory(IReportedSpamCheckInFactory reportedSpamCheckInFactory)
        {
            _ReportedSpamCheckInFactory = reportedSpamCheckInFactory;
        }

        #endregion

        #region Methods

        public Task ReportSpam(ReportedSpamCheckIn spamCheckIn)
        {
            return _ReportedSpamCheckInFactory.ReportSpam(spamCheckIn);
        }

        #endregion

    }
}
