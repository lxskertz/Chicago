using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Business;
using Tabs.Mobile.Shared.Models.CheckIns;
using BigTed;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessCheckInsController : BaseViewController
    {

        #region Properties

        public List<CheckIn> CheckIns { get; set; }

        public CheckIn.CheckInTypes ScreenCheckInType { get; set; }

        public int BusinessId { get; set; }

        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private BusinessCheckInsDataSource BusinessCheckInsDataSource { get; set; }

        #endregion

        #region Constructors

        public BusinessCheckInsController(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
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
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                    ICollection<CheckIn> checkIns = new List<CheckIn>();

                    if (this.ScreenCheckInType == CheckIn.CheckInTypes.Event)
                    {
                        checkIns = await AppDelegate.CheckInFactory.GetEventCheckIns(this.EventId);
                    }
                    else
                    {
                        var businessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);

                        if (businessInfo != null)
                        {
                            this.BusinessId = businessInfo.BusinessId;
                            checkIns = await AppDelegate.CheckInFactory.GetBusinessCheckIns(this.BusinessId);
                        }
                    }

                    if (checkIns != null)
                    {
                        BusinessCheckInTable.EstimatedRowHeight = 44f;
                        BusinessCheckInTable.RowHeight = UITableView.AutomaticDimension;
                        BusinessCheckInsDataSource = new BusinessCheckInsDataSource(this, checkIns.ToList());
                        BusinessCheckInTable.Source = BusinessCheckInsDataSource;
                        BusinessCheckInTable.TableFooterView = new UIView();
                    }

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        #endregion

    }
}