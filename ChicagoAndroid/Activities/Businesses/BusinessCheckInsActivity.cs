using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.ChicagoAndroid.Adapters.Business;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Businesses
{
    [Activity(Label = "Live Check-Ins", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class BusinessCheckInsActivity : BaseActivity
    {

        #region Constants, Enums, Variables

        public SwipeRefreshLayout refresher;
        private ListView businessCheckInList;
        //private ToasterOrder itemToCancel;

        #endregion

        #region Properties

        public static bool RequiresRefresh { get; set; }

        private BusinessCheckInsAdapter BusinessCheckInsAdapter { get; set; }

        public CheckIn.CheckInTypes ScreenCheckInType { get; set; }

        public int BusinessId { get; set; }

        public int EventId { get; set; }

        #endregion

        #region Methods

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.BusinessCheckIns);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                businessCheckInList = FindViewById<ListView>(Resource.Id.businessCheckInsList);

                refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefresh);
                //refresher.Refresh += HandleRefresh;

                await LoadData();
            }
            catch (Exception)
            {
            }
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
                    this.ShowProgressbar(true, "", ToastMessage.Loading);
                    ICollection<CheckIn> checkIns = new List<CheckIn>();
                    this.EventId = Intent.GetIntExtra("EventId", 0);
                    this.ScreenCheckInType = (CheckIn.CheckInTypes)Intent.GetIntExtra("ScreenCheckInType", 2);
                    //this.BusinessId = Intent.GetIntExtra("BusinessId", 0);

                    if (this.ScreenCheckInType == CheckIn.CheckInTypes.Event)
                    {
                        checkIns = await App.CheckInFactory.GetEventCheckIns(this.EventId);
                    }
                    else
                    {
                        var businessInfo = await App.BusinessFactory.GetByUserId(this.CurrentUser.UserId);

                        if (businessInfo != null)
                        {
                            this.BusinessId = businessInfo.BusinessId;
                            checkIns = await App.CheckInFactory.GetBusinessCheckIns(this.BusinessId);
                        }
                    }

                    if (checkIns != null)
                    {
                        BusinessCheckInsAdapter = new BusinessCheckInsAdapter(this, checkIns.ToList());
                        businessCheckInList.Adapter = BusinessCheckInsAdapter;
                        //businessCheckInList.ItemClick += BusinessCheckInsAdapter.OnListItemClick;
                        businessCheckInList.DividerHeight = 2;
                    }

                    this.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }


        #endregion

    }
}