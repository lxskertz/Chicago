using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.CheckIns;
using Tabs.Mobile.Shared.Models.CheckIns;

namespace Tabs.Mobile.Shared.Managers.CheckIns
{
    public class CheckInLikesFactory : ICheckInLikesFactory
    {
        #region Constants, Enums, and Variables

        private ICheckInLikesFactory _CheckInLikesFactory;

        #endregion

        #region Constructors

        public CheckInLikesFactory(ICheckInLikesFactory checkInLikesFactory)
        {
            _CheckInLikesFactory = checkInLikesFactory;
        }

        #endregion

        #region Methods

        public Task LikeChecKin(CheckInLikes checkInLike)
        {
            return _CheckInLikesFactory.LikeChecKin(checkInLike);
        }

        public Task UndoLikedCheckIn(bool like, int userId, int checkInLikeId)
        {
            return _CheckInLikesFactory.UndoLikedCheckIn(like, userId, checkInLikeId);
        }

        public Task<CheckInLikes> GetCheckInLike(int userId, int checkInId)
        {
            return _CheckInLikesFactory.GetCheckInLike(userId, checkInId);
        }

        public Task<int> GetLikeCount(int checkInId)
        {
            return _CheckInLikesFactory.GetLikeCount(checkInId);
        }

        #endregion

    }
}
