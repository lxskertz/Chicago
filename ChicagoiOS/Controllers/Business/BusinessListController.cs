using Foundation;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;
using BigTed;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessListController : BaseViewController, IUISearchResultsUpdating
    {

        #region Constants, Enums, and Variables

        private UIRefreshControl RefreshControl;
        UISearchController search;
        public SearchParameters param = new SearchParameters();
        public bool loadMore = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Business.BusinessListDataSource BusinessListDataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Shared.Models.Businesses.BusinessSearch> Businesses { get; set; } = new List<Shared.Models.Businesses.BusinessSearch>();

        public List<ImageViewImage> ImageViewImage { get; set; } = new List<ImageViewImage>();

        #endregion

        #region Constructors

        public BusinessListController (IntPtr handle) : base (handle)
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
                BusinessesTable.AddSubview(RefreshControl);

                search = new UISearchController(searchResultsController: null)
                {
                    DimsBackgroundDuringPresentation = false,
                    HidesNavigationBarDuringPresentation = false
                };
                search.SearchResultsUpdater = this;
                search.SearchBar.Placeholder = AppText.SearchBusinessPlaceHolder;
                this.NavigationItem.SearchController = search;
                this.NavigationItem.HidesSearchBarWhenScrolling = false;

                // do search, insert search term into DB
                search.SearchBar.SearchButtonClicked += async (sender, e) =>
                {
                    search.SearchBar.ResignFirstResponder();
                    await Search(search.SearchBar.Text);
                };

                LoadBusinesses();
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
            base.ViewDidAppear(animated);

            this.NavigationController.NavigationBarHidden = false;
            this.NavigationItem.SearchController = search;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchController"></param>
        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            var find = searchController.SearchBar.Text;
            //if (!String.IsNullOrEmpty(find))
            //{
            //    searchResults = titles.Where(t => t.ToLower().Contains(find.ToLower())).Select(p => p).ToArray();
            //}
            //else
            //{
            //    searchResults = null;
            //}
            //TableView.ReloadData();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void LoadBusinesses()
        {
            try
            {
                await this.GetBusinesses();
                BusinessesTable.EstimatedRowHeight = 150f;
                BusinessesTable.RowHeight = UITableView.AutomaticDimension;
                BusinessListDataSource = new DataSource.Business.BusinessListDataSource(this, this.Businesses, this.ImageViewImage);
                BusinessesTable.Source = BusinessListDataSource;
                BusinessesTable.TableFooterView = new UIView();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        private void InitSearchParameters(string searchTerm)
        {
            param = new SearchParameters();
            param.PageSize = 5;
            param.SearchTerm = searchTerm;
            param.PageNumber = 0;
            param.City = AppDelegate.City;
            param.ZipCode = AppDelegate.ZipCode;
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetBusinesses()
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
                    loadMore = true;
                    InitSearchParameters("");

                    var businesses = await AppDelegate.BusinessFactory.NearByBusinesses(param.City, param.ZipCode, param.SearchTerm, param.PageSize, param.PageNumber);
                    if (businesses != null)
                    {
                        this.Businesses = businesses.ToList();
                        await GetLogoUris();
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
        public override void DidReceiveMemoryWarning()
        {
            // Release all cached images. This will cause them to be redownloaded
            // later as they're displayed.
            if (this.ImageViewImage != null)
            {
                foreach (var v in this.ImageViewImage)
                    v.Image = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLogoUris()
        {
            try
            {
                foreach (var b in this.Businesses)
                {
                    ImageViewImage logo = new ImageViewImage();
                    logo.Id = b.BusinessId;
                    var uriString = await Shared.Helpers.BlobStorageHelper.GetBusinessLogoUri(b.BusinessId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString); //new Uri(await Shared.Helpers.BlobStorageHelper.GetBusinessLogoUri(b.BusinessId));
                        logo.ImageUrl = imageUri;
                        this.ImageViewImage.Add(logo);
                    }

                    //Uri imageUri = new Uri(await Shared.Helpers.BlobStorageHelper.GetBusinessLogoUri(b.BusinessId));
                    //logo.ImageUrl = imageUri;
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
        public async Task Search(string searchTerm)
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
                    InitSearchParameters(searchTerm);
                    BTProgressHUD.Show(ToastMessage.Searching, -1f, ProgressHUD.MaskType.Black);

                    var businesses = await AppDelegate.BusinessFactory.NearByBusinesses(param.City, param.ZipCode, param.SearchTerm, param.PageSize, param.PageNumber);
                    if (businesses != null && businesses.Count > 0)
                    {
                        this.Businesses = businesses.ToList();
                        await GetLogoUris();
                        this.BusinessListDataSource.Businesses = this.Businesses;
                        this.BusinessListDataSource.ImageViewImage = this.ImageViewImage;
                        BusinessesTable.ReloadData();
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NullResult, Helpers.ToastTime.ErrorTime);
                    }
                    BTProgressHUD.Dismiss();

                }
            }
            catch (Exception ex)
            {
                var a = ex;
                //BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task RefreshLiveEvents()
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
                    loadMore = true;
                    InitSearchParameters("");
                    var businesses = await AppDelegate.BusinessFactory.NearByBusinesses(param.City, param.ZipCode, param.SearchTerm, param.PageNumber, param.PageSize);
                    if (businesses != null)
                    {
                        this.Businesses = businesses.ToList();
                        await GetLogoUris();
                        this.BusinessListDataSource.Businesses = this.Businesses;
                        this.BusinessListDataSource.ImageViewImage = this.ImageViewImage;
                        BusinessesTable.ReloadData();
                    }

                    RefreshControl.EndRefreshing();

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Called when recycler view is scrolled to the bottom 
        /// </summary>
        public async Task ScrolledToBottom()
        {
            if (!AppDelegate.IsOfflineMode() && loadMore)
            {
                try
                {
                    param.PageNumber += this.BusinessListDataSource.Businesses.Count;
                    var businesses = await AppDelegate.BusinessFactory.NearByBusinesses(param.City, param.ZipCode, param.SearchTerm, param.PageSize, param.PageNumber);
                    if (businesses != null && businesses.Count > 0)
                    {
                        this.BusinessListDataSource.AddRowItems(businesses.ToList());
                        BusinessesTable.ReloadData();
                    }
                    else
                    {
                        loadMore = false;
                    }
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void HandleValueChanged(object sender, EventArgs e)
        {
            await RefreshLiveEvents();
        }


        #endregion

    }
}