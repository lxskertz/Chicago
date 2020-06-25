using System;

namespace Tabs.Mobile.Shared.Models.Points
{
    public class Point : BaseModel
    {

        public enum ToasterPointStatus
        {
            Earned = 0,
            Redeemed = 1
        }

        public enum PointAmountScale
        {
            Invite = 5,
            SignUp = 10,
            CheckIn = 5,
            SendDrink = 10
        }

        public int ToasterPointId { get; set; }

        public int PointAmount { get; set; }

        public ToasterPointStatus PointStatus { get; set; }

        public DateTime? EarnedDate { get; set; }

        public DateTime? RedeemedDate { get; set; }
    }
}
