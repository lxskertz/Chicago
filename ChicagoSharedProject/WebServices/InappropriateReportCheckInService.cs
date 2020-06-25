using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Reports.InappropriateReports;
using Tabs.Mobile.Shared.Interfaces.Reports.InappropriateReports;

namespace Tabs.Mobile.Shared.WebServices
{
    public class InappropriateReportCheckInService : BaseService, IInappropriateReportCheckInFactory 
    {

        #region Methods

        public async Task ReportInappropriate(InappropriateReport inappropriate)
        {
            string methodPath = "inappropriate";
            HttpRequestMessage response = null;
            var parameters = new
            {
                CheckInId = inappropriate.CheckInId,
                CheckInUserId = inappropriate.CheckInUserId,
                CheckInReportReason = inappropriate.CheckInReportReason,
                ReporterUserId = inappropriate.ReporterUserId,
                EventId = inappropriate.EventId,
                BusinessId = inappropriate.BusinessId,
                CheckInDate = inappropriate.CheckInDate,
                CheckInType = inappropriate.CheckInType,
                CheckInDateString = inappropriate.CheckInDate.ToString(),
                BusinessName = inappropriate.BusinessName,
                BlockedByAdmin = inappropriate.BlockedByAdmin,
                BlockedByAdminUserId = inappropriate.BlockedByAdminUserId,
                ReporterFirstName = inappropriate.ReporterFirstName,
                ReporterLastName = inappropriate.ReporterLastName,
                SenderFirstName = inappropriate.SenderFirstName,
                SenderLastName = inappropriate.SenderLastName
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        #endregion

    }
}
