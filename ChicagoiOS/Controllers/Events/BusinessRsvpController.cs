using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Business;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Events;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessRsvpController : BaseViewController
    {

        #region Properties

        public BusinessEvents BusinessEvents { get; set; } 

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private BusinessRsvpDataSource BusinessRsvpDataSource { get; set; }

        #endregion

        #region Constructors

        public BusinessRsvpController (IntPtr handle) : base (handle)
        {
        }

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
                await GetRsvps();
            }
            catch (Exception)
            {
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetRsvps()
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

                    var rsvps = await AppDelegate.RsvpFactory.GetBusinessEventRsvps(this.BusinessEvents.BusinessId, this.BusinessEvents.EventId);

                    if (rsvps != null)
                    {
                        BusinessRsvpTable.RowHeight = UITableView.AutomaticDimension;
                        BusinessRsvpDataSource = new BusinessRsvpDataSource(this, rsvps.ToList());
                        BusinessRsvpTable.Source = BusinessRsvpDataSource;
                        BusinessRsvpTable.TableFooterView = new UIView();
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