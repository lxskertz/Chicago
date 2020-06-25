using Foundation;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Points;
using Tabs.Mobile.ChicagoiOS.DataSource.Points;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToasterPointsController : BaseViewController
    {

        #region Constants, Enums, and Variables

        public bool redeemedPointsShown;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private ToasterPointsDataSource ToasterPointsDataSource { get; set; }

        #endregion

        #region Constructors

        public ToasterPointsController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void InitTableView(List<Point> data)
        {
            PointsTable.EstimatedRowHeight = 44f;
            PointsTable.RowHeight = UITableView.AutomaticDimension;
            ToasterPointsDataSource = new ToasterPointsDataSource(this, data);
            PointsTable.Source = ToasterPointsDataSource;
            PointsTable.TableFooterView = new UIView();
        }

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(AppText.RedeemedPoints, UIBarButtonItemStyle.Plain, (sender, args) =>
                {
                    ResetTable();

                }), true);

                await GetEarnedPoints();
            }
            catch (Exception)
            {
            }
        }

        private async void ResetTable()
        {
            try
            {
                if (!redeemedPointsShown || this.NavigationItem.RightBarButtonItem.Title.ToLower() == AppText.RedeemedPoints)
                {
                    await GetRedeemedPoints();
                }
                else
                {
                    await GetEarnedPoints();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task GetTotalEarnedPoints()
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
                    var result = await AppDelegate.ToasterPointsFactory.GetTotalEarnedPoints(AppDelegate.CurrentUser.UserId);
                    PointAmt.Text = "Total Earned Points: " + result.ToString();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
                //BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        public async Task GetTotalRedeemedPoints()
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
                    var result = await AppDelegate.ToasterPointsFactory.GetTotalRedeemedPoints(AppDelegate.CurrentUser.UserId);
                    PointAmt.Text = "Total Redeemed Points: " + result.ToString();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task GetEarnedPoints()
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

                    await GetTotalEarnedPoints();

                    var result = await AppDelegate.ToasterPointsFactory.GetEarnedPoints(AppDelegate.CurrentUser.UserId);

                    if (result != null && result.Count > 0)
                    {
                        redeemedPointsShown = false;
                        this.NavigationItem.RightBarButtonItem.Title = AppText.RedeemedPoints;
                        if (ToasterPointsDataSource == null)
                        {
                            InitTableView(result.ToList());
                        }
                        else
                        {
                            this.InvokeOnMainThread(() =>
                            {
                                this.ToasterPointsDataSource.Points = result.ToList();
                                PointsTable.ReloadData();
                            });
                        }
                        BTProgressHUD.Dismiss();
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoEarnedPoints, Helpers.ToastTime.ErrorTime);
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetRedeemedPoints()
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

                    await GetTotalRedeemedPoints();

                    var result = await AppDelegate.ToasterPointsFactory.GetRedeemedPoints(AppDelegate.CurrentUser.UserId);

                    if (result != null && result.Count > 0)
                    {
                        redeemedPointsShown = true;
                        this.NavigationItem.RightBarButtonItem.Title = AppText.EarnedPoints;
                        if (ToasterPointsDataSource == null)
                        {
                            InitTableView(result.ToList());
                        }
                        else
                        {
                            this.InvokeOnMainThread(() =>
                            {
                                this.ToasterPointsDataSource.Points = result.ToList();
                                PointsTable.ReloadData();
                            });
                        }
                        BTProgressHUD.Dismiss();
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoEarnedPoints, Helpers.ToastTime.ErrorTime);
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        #endregion

    }
}