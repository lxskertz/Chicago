using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Contacts;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS.Helpers
{
    public class ContactService
    {
        public async Task<IEnumerable<PhoneContact>> GetAllContacts()
        {
            var keysToFetch = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.PhoneNumbers, CNContactKey.EmailAddresses };
            NSError error;
            //var containerId = new CNContactStore().DefaultContainerIdentifier;
            // using the container id of null to get all containers.
            // If you want to get contacts for only a single container type, you can specify that here
            var contactList = new List<CNContact>();

            using (var store = new CNContactStore())
            {
                var allContainers = store.GetContainers(null, out error);
                foreach (var container in allContainers)
                {
                    try
                    {
                        using (var predicate = CNContact.GetPredicateForContactsInContainer(container.Identifier))
                        {
                            var containerResults = store.GetUnifiedContacts(predicate, keysToFetch, out error);
                            contactList.AddRange(containerResults);
                        }
                    }
                    catch
                    {
                        // ignore missed contacts from errors
                    }
                }
            }
            var contacts = new List<PhoneContact>();

            foreach (var item in contactList)
            {
                var numbers = item.PhoneNumbers;
                var emails = item.EmailAddresses;
                if (numbers != null)
                {
                    foreach (var item2 in numbers)
                    {                    
                        contacts.Add(new PhoneContact
                        {
                            FirstName = item.GivenName,
                            LastName = item.FamilyName,
                            PhoneNumber = item2.Value.StringValue,
                            //Email = item.EmailAddresses.ElementAtOrDefault(0).Value

                        });
                    }
                }
            }
            return contacts;
        }
    }
}