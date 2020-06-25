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
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Widget;
using Newtonsoft.Json;
using Tabs.Mobile.ChicagoAndroid.Adapters.Business.Events;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Events;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Businesses
{
    [Activity(Label = "RSVPs", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class BusinessRsvpsActivity : BaseActivity
    {

        #region Constants, Enums, Variables

        private ListView businessRsvpList;
        private BusinessRsvpsAdapter BusinessRsvpsAdapter;

        #endregion

        #region Properties

        public BusinessEvents BusinessEvents { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.BusinessRsvps);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                businessRsvpList = FindViewById<ListView>(Resource.Id.businessRsvpsList);
                await LoadData();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
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

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task LoadData()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    this.BusinessEvents = JsonConvert.DeserializeObject<BusinessEvents>(Intent.GetStringExtra("BusinessEventInfo"));

                    this.ShowProgressbar(true, "", ToastMessage.Loading);

                    var rsvps = await App.RsvpFactory.GetBusinessEventRsvps(this.BusinessEvents.BusinessId, this.BusinessEvents.EventId);

                    if (rsvps != null)
                    {
                        BusinessRsvpsAdapter = new BusinessRsvpsAdapter(this, rsvps.ToList());
                        businessRsvpList.Adapter = BusinessRsvpsAdapter;
                        //drinksList.ItemClick += SendDrinkAdapter.OnListItemClick;
                        businessRsvpList.DividerHeight = 2;
                    }

                    this.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        #endregion


    }
}