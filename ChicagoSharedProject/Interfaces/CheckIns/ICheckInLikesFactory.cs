using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.CheckIns;

namespace Tabs.Mobile.Shared.Interfaces.CheckIns
{
    public interface ICheckInLikesFactory
    {

        Task LikeChecKin(CheckInLikes checkInLike);

        Task UndoLikedCheckIn(bool like, int userId, int checkInLikeId);

        Task<CheckInLikes> GetCheckInLike(int userId, int checkInId);

        Task<int> GetLikeCount(int checkInId);

    }
}
