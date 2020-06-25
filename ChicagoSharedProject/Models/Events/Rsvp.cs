namespace Tabs.Mobile.Shared.Models.Events
{
    public class Rsvp : BaseModel
    {

        #region Properties

        public int RsvpId { get; set; }

        public int EventId { get; set; }

        public int BusinessId { get; set; }

        public bool Going { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        #endregion

    }
}
