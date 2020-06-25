using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.CheckIns;

namespace Tabs.Mobile.Shared.Interfaces.CheckIns
{
    public interface ICheckInFactory
    {

        Task<int> CheckIn(CheckIn checkIn);

        Task<ICollection<CheckIn>> GetCheckIns(int userId);

        Task<bool> EventCheckInExist(int userId, int eventId, int checkInType);

        Task<ICollection<CheckIn>> GetBusinessCheckIns(int businessId);

        Task<ICollection<CheckIn>> GetEventCheckIns(int eventId);

    }
}
