using Tabs.Mobile.Shared.Models.Events;
using System.Threading.Tasks;

namespace Tabs.Mobile.Shared.Interfaces.Events
{
    public interface IEventLikesFactory
    {

        Task LikeEvent(EventLikes eventLike);

        Task UndoLikedEvent(bool like, int userId, int eventId);

        Task<int> GetLikeCount(int businessId, int eventId);

        Task<EventLikes> GetEventLike(int userId, int eventId);

    }
}
