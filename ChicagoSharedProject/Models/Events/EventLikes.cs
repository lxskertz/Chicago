using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models.Events
{
    public class EventLikes : BaseModel
    {

        public int EventLikeId { get; set; }

        public int EventId { get; set; }

        public int BusinessId { get; set; }

        public bool Liked { get; set; }

    }
}
