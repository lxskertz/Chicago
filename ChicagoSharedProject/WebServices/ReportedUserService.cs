using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Reports.Users;
using Tabs.Mobile.Shared.Interfaces.Reports.Users;

namespace Tabs.Mobile.Shared.WebServices
{
    public class ReportedUserService : BaseService, IReportedUserFactory
    {

        #region Methods

        public async Task ReportUser(ReportedUser reportedUser)
        {
            string methodPath = "report_user";
            HttpRequestMessage response = null;
            var parameters = new
            {
                ReporterUserId = reportedUser.ReporterUserId,
                ReportDate = reportedUser.ReportDate,
                ReportDateString = DateTime.Now.ToString(),
                BlockedByAdmin = reportedUser.BlockedByAdmin,
                BlockedByAdminUserId = reportedUser.BlockedByAdminUserId,
                ReporterFirstName = reportedUser.ReporterFirstName,
                ReporterLastName = reportedUser.ReporterLastName,
                SenderFirstName = reportedUser.SenderFirstName,
                SenderLastName = reportedUser.SenderLastName,
                SenderUserId = reportedUser.SenderUserId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        #endregion

    }
}
