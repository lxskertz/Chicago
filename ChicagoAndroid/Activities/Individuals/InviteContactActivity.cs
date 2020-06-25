using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Individuals
{
    [Activity(Label = "Invite Contacts", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class InviteContactActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        ListView contactList;

        #endregion

        #region Methiods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.InviteContact);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                contactList = FindViewById<ListView>(Resource.Id.contactsList);

                LoadContacts();
            }
            catch (Exception)
            {
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private void LoadContacts()
        {
            try
            {
                var contacts = new Helpers.ContactServiceAndroid().GetAllContacts(this);
                if (contacts != null)
                {
                    contacts = contacts.OrderBy(x => x.FirstName).ToList();
                    InviteContactAdapter inviteContactAdapter = new InviteContactAdapter(this, contacts.ToList());
                    contactList.Adapter = inviteContactAdapter;
                    //contactList.ItemClick += OrderDetailsAdapter.OnListItemClick;
                    contactList.DividerHeight = 2;
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion

    }
}