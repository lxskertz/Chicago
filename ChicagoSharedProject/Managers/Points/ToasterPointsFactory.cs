using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Points;
using Tabs.Mobile.Shared.Interfaces.Points;

namespace Tabs.Mobile.Shared.Managers.Points
{
    public class ToasterPointsFactory : IToasterPointsFactory
    {

        #region Constants, Enums, and Variables

        private IToasterPointsFactory _ToasterPointsFactory;

        #endregion

        #region Constructors

        public ToasterPointsFactory(IToasterPointsFactory toasterPointsFactory)
        {
            _ToasterPointsFactory = toasterPointsFactory;
        }

        #endregion

        #region Methods

        public Task NewDailyPoint(Point point)
        {
            return _ToasterPointsFactory.NewDailyPoint(point);
        }

        public Task MarkAsRedeemed(int toasterOrderId)
        {
            return _ToasterPointsFactory.MarkAsRedeemed(toasterOrderId);
        }

        public Task<ICollection<Point>> GetEarnedPoints(int userId)
        {
            return _ToasterPointsFactory.GetEarnedPoints(userId);
        }

        public Task<ICollection<Point>> GetRedeemedPoints(int userId)
        {
            return _ToasterPointsFactory.GetRedeemedPoints(userId);
        }

        public Task<int> GetTotalEarnedPoints(int userId)
        {
            return _ToasterPointsFactory.GetTotalEarnedPoints(userId);
        }

        public Task<int> GetTotalRedeemedPoints(int userId)
        {
            return _ToasterPointsFactory.GetTotalRedeemedPoints(userId);
        }

        #endregion

    }
}
