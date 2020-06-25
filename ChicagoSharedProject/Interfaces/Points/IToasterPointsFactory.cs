using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Points;

namespace Tabs.Mobile.Shared.Interfaces.Points
{
    public interface IToasterPointsFactory
    {

        Task NewDailyPoint(Point point);

        Task MarkAsRedeemed(int toasterOrderId);

        Task<ICollection<Point>> GetEarnedPoints(int userId);

        Task<ICollection<Point>> GetRedeemedPoints(int userId);

        Task<int> GetTotalEarnedPoints(int userId);

        Task<int> GetTotalRedeemedPoints(int userId);

    }
}
