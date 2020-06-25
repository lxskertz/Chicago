using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Models.CheckIns
{
    public class CheckInLikes 
    {
        public int UserId { get; set; }

        public int CheckInLikeId { get; set; }

        public int CheckInId { get; set; }

        public int BusinessId { get; set; }

        public bool Liked { get; set; }

    }
}
