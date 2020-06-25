using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Drinks;
using Tabs.Mobile.Shared.Models.Drinks;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessDrinksController : BaseViewController
    {

        #region Properties

        public static bool RequiresRefresh { get; set; }

        public Business BusinessInfo { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DrinksDataSource DrinksDataSource { get; set; }

        #endregion

        #region Constructors

        public BusinessDrinksController (IntPtr handle) : base (handle)
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
                await GetDrinks();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);

                this.TabBarController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, args) =>
                {
                    var controller = this.TabBarController.Storyboard.InstantiateViewController("AddEditDrinksController") as AddEditDrinksController;
                    controller.ScreenActionType = AddEditDrinksController.ActionType.Add;
                    controller.BusinessId = this.BusinessInfo != null ? this.BusinessInfo.BusinessId : 0;
                    this.TabBarController.NavigationController.PushViewController(controller, true);
                }), true);

                this.TabBarController.NavigationItem.SearchController = null;
                SetTitle();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Set title
        /// </summary>
        private void SetTitle()
        {
            this.TabBarController.NavigationItem.Title = "Drinks";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public async override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    if (AppDelegate.IsOfflineMode())
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                        return;
                    }
                    if (this.BusinessInfo != null)
                    {
                        var drinks = await AppDelegate.BusinessDrinkFactory.Get(this.BusinessInfo.BusinessId);

                        if (drinks != null)
                        {
                            if (this.DrinksDataSource == null)
                            {
                                DrinksTable.EstimatedRowHeight = 44f;
                                DrinksTable.RowHeight = UITableView.AutomaticDimension;
                                this.DrinksDataSource = new DrinksDataSource(this, drinks.ToList());
                                DrinksTable.Source = this.DrinksDataSource;
                                DrinksTable.TableFooterView = new UIView();
                            }
                            else
                            {
                                this.DrinksDataSource.Drinks = drinks.ToList();
                                DrinksTable.ReloadData();
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetDrinks()
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
                    this.BusinessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);

                    if (this.BusinessInfo != null)
                    {
                        var drinks = await AppDelegate.BusinessDrinkFactory.Get(this.BusinessInfo.BusinessId);

                        if (drinks != null)
                        {
                            DrinksTable.EstimatedRowHeight = 44f;
                            DrinksTable.RowHeight = UITableView.AutomaticDimension;
                            DrinksDataSource = new DrinksDataSource(this, drinks.ToList());
                            DrinksTable.Source = DrinksDataSource;
                            DrinksTable.TableFooterView = new UIView();
                        } else
                        {
                            //BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                        }
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