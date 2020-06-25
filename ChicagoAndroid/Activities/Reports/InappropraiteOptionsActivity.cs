using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.ChicagoAndroid.Adapters.Reports;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Reports.InappropriateReports;
using Newtonsoft.Json;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Reports
{
    [Activity(Label = "Report")]
    public class InappropraiteOptionsActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        private ListView reasonList;
        private Android.Support.V7.App.AlertDialog alertDialog;
        private Android.Support.V7.App.AlertDialog.Builder builder;

        #endregion

        #region Properties

        public CheckIn CheckInItem { get; set; }

        /// <summary>
        /// Gets or sets adapter
        /// </summary>
        private InappropraiteOptionsAdapter InappropraiteOptionsAdapter { get; set; }

        public static bool CloseController { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InappropriateReasons);
            this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SupportActionBar.SetDisplayShowHomeEnabled(true);
            reasonList = FindViewById<ListView>(Resource.Id.reasonsList);
            this.CheckInItem = JsonConvert.DeserializeObject<CheckIn>(Intent.GetStringExtra("CheckInItem"));
            LoadData();
            builder = new Android.Support.V7.App.AlertDialog.Builder(this);
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private void LoadData()
        {
            try
            {
                var reasons = InappropriatePostHelper.ReasonList();

                if (reasons != null)
                {
                    InappropraiteOptionsAdapter = new InappropraiteOptionsAdapter(this, reasons);
                    reasonList.Adapter = InappropraiteOptionsAdapter;
                    reasonList.ItemClick += InappropraiteOptionsAdapter.OnListItemClick;
                    reasonList.DividerHeight = 2;
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

        private void CancelClicked(object sender, DialogClickEventArgs arg)
        {
            if (alertDialog != null)
            {
                alertDialog.Dismiss();
                alertDialog.Dispose();
                alertDialog = null;
            }
            this.Finish();
        }

        public async void ReportInappropriate(InappropriateReport.ReportReason reportReason)
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                    return;
                }
                else
                {
                    InappropriateReport inappropriateReport = new InappropriateReport();
                    inappropriateReport.BlockedByAdmin = false;
                    inappropriateReport.BlockedByAdminUserId = 0;
                    inappropriateReport.BusinessId = this.CheckInItem.BusinessId;
                    inappropriateReport.BusinessName = this.CheckInItem.BusinessName;
                    inappropriateReport.CheckInDate = this.CheckInItem.CheckInDate;
                    inappropriateReport.CheckInDateString = this.CheckInItem.CheckInDate.ToString();
                    inappropriateReport.CheckInId = this.CheckInItem.CheckInId;
                    inappropriateReport.CheckInType = this.CheckInItem.CheckInType;
                    inappropriateReport.CheckInUserId = this.CheckInItem.UserId;
                    inappropriateReport.EventId = this.CheckInItem.EventId;
                    inappropriateReport.ReporterUserId = this.CurrentUser.UserId;
                    inappropriateReport.ReporterFirstName = this.CurrentUser.FirstName;
                    inappropriateReport.ReporterLastName = this.CurrentUser.LastName;
                    inappropriateReport.CheckInReportReason = reportReason;

                    var checkinuser = await App.UsersFactory.GetUser(this.CheckInItem.UserId);

                    if (checkinuser != null)
                    {
                        inappropriateReport.SenderFirstName = checkinuser.FirstName;
                        inappropriateReport.SenderLastName = checkinuser.LastName;
                    }

                    await App.InappropriateReportCheckInFactory.ReportInappropriate(inappropriateReport);

                    if (alertDialog != null && alertDialog.IsShowing)
                    {
                        alertDialog.Dismiss();
                        alertDialog.Dispose();
                    }
                    builder.SetMessage(ToastMessage.InappropriateReportMessage);
                    builder.SetCancelable(false);
                    builder.SetPositiveButton(AppText.Ok, CancelClicked);
                    alertDialog = builder.Create();
                    alertDialog.Show();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        #endregion

    }
}