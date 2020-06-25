using Foundation;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Resources;
using BigTed;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessEventController : BaseViewController
    {
        #region Constants, Enums, and Variables

        private UIRefreshControl RefreshControl;
        private Shared.Models.Businesses.Business businessInfo;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Business.BusinessEventsDataSource BusinessEventsDataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessEvents> BusinessEvents { get; set; } = new List<BusinessEvents>();

        #endregion

        #region Constructors

        public BusinessEventController (IntPtr handle) : base (handle)
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
                RefreshControl = new UIRefreshControl();
                RefreshControl.ValueChanged += HandleValueChanged;
                BusinessEventTable.AddSubview(RefreshControl);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                LoadData();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void LoadData()
        {
            try
            {
                await this.GetEvents();
                BusinessEventTable.EstimatedRowHeight = 44f;
                BusinessEventTable.RowHeight = UITableView.AutomaticDimension;
                BusinessEventsDataSource = new DataSource.Business.BusinessEventsDataSource(this, this.BusinessEvents);
                BusinessEventTable.Source = BusinessEventsDataSource;
                BusinessEventTable.TableFooterView = new UIView();
            }
            catch (Exception ex)
            {
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetEvents()
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
                    businessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);
                    if (businessInfo != null)
                    {
                        var events = await AppDelegate.BusinessEventsFactory.Get(businessInfo.BusinessId);
                        if(events != null)
                        {
                            this.BusinessEvents = events.ToList();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.TabBarController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, args) => {
                var controller = this.Storyboard.InstantiateViewController("EventNameDescController") as EventNameDescController;
                controller.Mode = Shared.Models.Events.BusinessEvents.ActionMode.Add;
                this.NavigationController.PushViewController(controller, true);
            }), true);
            this.TabBarController.NavigationItem.SearchController = null;
            SetTitle();
        }

        /// <summary>
        /// Set title
        /// </summary>
        private void SetTitle()
        {
            this.TabBarController.NavigationItem.Title = "Events";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void HandleValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    //try
                    //{
                    RefreshControl.BeginRefreshing();

                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    if (businessInfo == null)
                    {
                        businessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);
                    }

                    if (businessInfo != null)
                    {
                        var events = await AppDelegate.BusinessEventsFactory.Get(businessInfo.BusinessId);
                        if (events != null && events.Count > 0)
                        {
                            this.BusinessEvents = events.ToList();
                            this.BusinessEventsDataSource.BusinessEvents = this.BusinessEvents;
                            BusinessEventTable.ReloadData();
                        }
                    }

                    RefreshControl.EndRefreshing();

                    BTProgressHUD.Dismiss();

                }
            }
            catch (Exception)
            {

            }

        }

        #endregion

    }
}