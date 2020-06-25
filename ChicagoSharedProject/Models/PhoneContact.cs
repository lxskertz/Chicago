namespace Tabs.Mobile.Shared.Models
{
    public class PhoneContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string Name { get => $"{FirstName} {LastName}"; }
    }

}
