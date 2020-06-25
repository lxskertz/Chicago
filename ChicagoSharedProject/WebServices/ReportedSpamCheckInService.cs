using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.Reports.Spams;
using Tabs.Mobile.Shared.Models.Reports.Spams;

namespace Tabs.Mobile.Shared.WebServices
{
    public class ReportedSpamCheckInService : BaseService, IReportedSpamCheckInFactory
    {

        #region Methods

        public async Task ReportSpam(ReportedSpamCheckIn spamCheckIn)
        {
            string methodPath = "spam";
            HttpRequestMessage response = null;
            var parameters = new
            {
                CheckInId = spamCheckIn.CheckInId,
                CheckInUserId = spamCheckIn.CheckInUserId,
                ReporterUserId = spamCheckIn.ReporterUserId,
                EventId = spamCheckIn.EventId,
                BusinessId = spamCheckIn.BusinessId,
                CheckInDate = spamCheckIn.CheckInDate,
                CheckInType = spamCheckIn.CheckInType,
                CheckInDateString = spamCheckIn.CheckInDate.ToString(),
                BusinessName = spamCheckIn.BusinessName,
                BlockedByAdmin = spamCheckIn.BlockedByAdmin,
                BlockedByAdminUserId = spamCheckIn.BlockedByAdminUserId,
                ReporterFirstName = spamCheckIn.ReporterFirstName,
                ReporterLastName = spamCheckIn.ReporterLastName,
                SenderFirstName = spamCheckIn.SenderFirstName,
                SenderLastName = spamCheckIn.SenderLastName
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        #endregion

    }
}
