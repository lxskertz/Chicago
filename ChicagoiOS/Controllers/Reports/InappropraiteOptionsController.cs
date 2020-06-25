using Foundation;
using System;
using UIKit;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.ChicagoiOS.DataSource.Reports;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Reports.InappropriateReports;
using BigTed;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class InappropraiteOptionsController : BaseViewController
    {

        #region Properties

        public CheckIn CheckInItem { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private InappropraiteOptionsDatasource InappropraiteOptionsDatasource { get; set; }

        public static bool CloseController { get; set; }

        #endregion

        #region Constructors

        public InappropraiteOptionsController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods


        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                LoadData();
            }
            catch (Exception)
            {
            }
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
                    InappropriateTable.EstimatedRowHeight = 44f;
                    InappropriateTable.RowHeight = UITableView.AutomaticDimension;
                    InappropraiteOptionsDatasource = new InappropraiteOptionsDatasource(this, reasons);
                    InappropriateTable.Source = InappropraiteOptionsDatasource;
                    InappropriateTable.TableFooterView = new UIView();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportReason"></param>
        public async void ReportInappropriate(InappropriateReport.ReportReason reportReason)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    InappropriateReport inappropriateReport = new InappropriateReport();
                    inappropriateReport.BlockedByAdmin = false;
                    inappropriateReport.BlockedByAdminUserId = 0;
                    inappropriateReport.BusinessId = CheckInItem.BusinessId;
                    inappropriateReport.BusinessName = CheckInItem.BusinessName;
                    inappropriateReport.CheckInDate = CheckInItem.CheckInDate;
                    inappropriateReport.CheckInDateString = CheckInItem.CheckInDate.ToString();
                    inappropriateReport.CheckInId = CheckInItem.CheckInId;
                    inappropriateReport.CheckInType = CheckInItem.CheckInType;
                    inappropriateReport.CheckInUserId = CheckInItem.UserId;
                    inappropriateReport.EventId = CheckInItem.EventId;
                    inappropriateReport.ReporterUserId = AppDelegate.CurrentUser.UserId;
                    inappropriateReport.ReporterFirstName = AppDelegate.CurrentUser.FirstName;
                    inappropriateReport.ReporterLastName = AppDelegate.CurrentUser.LastName;
                    inappropriateReport.CheckInReportReason = reportReason;

                    var checkinuser = await AppDelegate.UsersFactory.GetUser(CheckInItem.UserId);

                    if (checkinuser != null)
                    {
                        inappropriateReport.SenderFirstName = checkinuser.FirstName;
                        inappropriateReport.SenderLastName = checkinuser.LastName;
                    }

                    await AppDelegate.InappropriateReportCheckInFactory.ReportInappropriate(inappropriateReport);

                    UIAlertController uIAlertController = new UIAlertController();
                    uIAlertController = UIAlertController.Create("", ToastMessage.InappropriateReportMessage, UIAlertControllerStyle.Alert);
                    uIAlertController.AddAction(UIAlertAction.Create(AppText.Ok, UIAlertActionStyle.Default, (Action) => OpenFeed()));
                    this.PresentViewController(uIAlertController, true, null);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        private void OpenFeed()
        {
            this.NavigationController.PopViewController(false);
        }


        #endregion
    }
}