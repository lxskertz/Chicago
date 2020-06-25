using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Android.Provider;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoAndroid.Helpers
{
    public class ContactServiceAndroid
    {
        private static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }

        public IEnumerable<PhoneContact> GetAllContacts(Activities.BaseActivity baseActivity)
        {
            
                //var phoneContacts = new List<PhoneContact>();

                HashSet<PhoneContact> phoneContacts = new HashSet<PhoneContact>();
            try { 
                using (var phones = Android.App.Application.Context.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null))
                {
                    if (phones != null)
                    {
                        while (phones.MoveToNext())
                        {
                            try
                            {
                                string name = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                                string phoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                                //string email = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.ea.));

                                if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length > 9)
                                //&&
                                //IsPhoneNumber(phoneNumber))
                                {
                                    string[] words = name.Split(' ');

                                    //if (!phoneContacts.Any(x => x.FirstName.ToLower() != words[0].ToLower()))
                                    //{
                                    var contact = new PhoneContact();
                                    contact.FirstName = words[0];
                                    if (words.Length > 1)
                                        contact.LastName = words[1];
                                    else
                                        contact.LastName = ""; //no last name
                                    contact.PhoneNumber = phoneNumber;
                                    phoneContacts.Add(contact);
                                    //}
                                }
                            }
                            catch (Exception)
                            {
                                //something wrong with one contact, may be display name is completely empty, decide what to do
                            }
                        }
                        phones.Close();
                    }
                    // if we get here, we can't access the contacts. Consider throwing an exception to display to the user
                }

                //var noDupsList = new HashSet<PhoneContact>(phoneContacts).ToList();

                //var noDupes = phoneContacts.Distinct().ToList();
            }
            catch (Exception)
            {
            }

            return phoneContacts;
        }

    }
}