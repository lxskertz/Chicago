using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Reports.Users;

namespace Tabs.Mobile.Shared.Interfaces.Reports.Users
{
    public interface IReportedUserFactory
    {
        Task ReportUser(ReportedUser reportedUser);
    }
}
