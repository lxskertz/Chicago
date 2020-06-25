using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Reports.Users;
using Tabs.Mobile.Shared.Interfaces.Reports.Users;

namespace Tabs.Mobile.Shared.Managers.Reports.Users
{
    public class ReportedUserFactory : IReportedUserFactory
    {

        #region Constants, Enums, and Variables

        private IReportedUserFactory _ReportedUserFactory;

        #endregion

        #region Constructors

        public ReportedUserFactory(IReportedUserFactory reportedUserFactory)
        {
            _ReportedUserFactory = reportedUserFactory;
        }

        #endregion

        #region Methods

        public Task ReportUser(ReportedUser reportedUser)
        {
            return _ReportedUserFactory.ReportUser(reportedUser);
        }

        #endregion

    }
}
