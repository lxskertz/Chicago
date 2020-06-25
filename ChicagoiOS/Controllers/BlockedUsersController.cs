using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using Foundation;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.ChicagoiOS.DataSource.Individuals;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BlockedUsersController : BaseViewController
    {

        #region Constants, Enums, and Variables

        public SearchParameters param = new SearchParameters();
        public bool loadMore = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private BlockedUserDataSource BlockedUserDataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private ICollection<Toasters> Toasters { get; set; }

        #endregion

        #region Constructors

        public BlockedUsersController(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void InitTableView(List<Toasters> data)
        {
            BlockedToastersTable.EstimatedRowHeight = 44f;
            BlockedToastersTable.RowHeight = UITableView.AutomaticDimension;
            BlockedUserDataSource = new BlockedUserDataSource(this, data);
            BlockedToastersTable.Source = BlockedUserDataSource;
            BlockedToastersTable.TableFooterView = new UIView();
        }

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();
            await RetriveToasters();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task RetriveToasters()
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
                    loadMore = true;
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                    //InitSearchParameters();
                    Toasters = await AppDelegate.ToastersFactory.GetBlockedToasters(AppDelegate.CurrentUser.UserId);

                    if (Toasters != null && Toasters.Count > 0)
                    {
                        if (BlockedUserDataSource == null)
                        {
                            InitTableView(Toasters.ToList());
                        }
                        else
                        {
                            this.InvokeOnMainThread(() =>
                            {
                                this.BlockedUserDataSource.Rows = Toasters.ToList();
                                BlockedToastersTable.ReloadData();
                            });
                        }
                    }
                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        ///// <summary>
        ///// Called when recycler view is scrolled to the bottom 
        ///// </summary>
        //public async Task ScrolledToBottom()
        //{
        //    if (!AppDelegate.IsOfflineMode() && loadMore)
        //    {
        //        try
        //        {
        //            ICollection<Toasters> results = null;
        //            param.PageNumber += this.ToastersDataSource.Rows.Count;

        //            if (pendingRequestShown)
        //            {
        //                results = await AppDelegate.ToastersFactory.GetPendingToasters(param);
        //            }
        //            else
        //            {
        //                results = await AppDelegate.ToastersFactory.GetToasters(param);
        //            }

        //            if (results != null && results.Count > 0)
        //            {
        //                this.ToastersDataSource.AddRowItems(results.ToList());
        //                ToastersTable.ReloadData();
        //            }
        //            else
        //            {
        //                loadMore = false;
        //            }
        //        }
        //        catch (Exception) { }
        //    }
        //}

        public async void UnBlockedUser(Toasters toaster)
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
                    if (toaster == null)
                    {
                        return;
                    }
                    await AppDelegate.ToastersFactory.UnBlockToaster(toaster.ToastersId);
                    ToastersController.RequiresRefresh = true;
                    NavigationController.PopViewController(false);
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        #endregion

    }
}