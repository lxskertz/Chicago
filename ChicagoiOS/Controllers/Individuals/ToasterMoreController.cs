using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using Tabs.Mobile.Shared.Helpers;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToasterMoreController : BaseViewController
    {

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Individuals.ToastersMoreDataSource ToastersMoreDataSource { get; set; }

        #endregion

        #region Constructors

        public ToasterMoreController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            List<string> rows; //= new List<string>();
            if (AppDelegate.CurrentUser != null && AppDelegate.CurrentUser.IsBusiness)
            {
                rows = MoreScreenHelper.GetBusinessTableRows();
            }
            else
            {
                rows = MoreScreenHelper.GetIndividualTableRows();
            }
            MoreTable.EstimatedRowHeight = 44f;
            MoreTable.RowHeight = UITableView.AutomaticDimension;
            ToastersMoreDataSource = new DataSource.Individuals.ToastersMoreDataSource(this, rows);
            MoreTable.Source = ToastersMoreDataSource;
            MoreTable.TableFooterView = new UIView();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.TabBarController.NavigationController.NavigationBarHidden = false;
            this.TabBarController.NavigationItem.SearchController = null;
            this.TabBarController.NavigationItem.RightBarButtonItem = null;
            SetTitle();
        }

        /// <summary>
        /// Set title
        /// </summary>
        private void SetTitle()
        {
            this.TabBarController.NavigationItem.Title = "More";
        }

        #endregion

    }
}