using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Foundation;
using BigTed;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToastersSearchViewController : BaseViewController, IUISearchResultsUpdating
    {

        #region Constants, Enums, and Variables

        UISearchController search;
        public SearchParameters param = new SearchParameters();
        public bool loadMore = true;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImages { get; set; } = new List<ImageViewImage>();

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Individuals.ToastersSearchDataSource ToastersSearchDataSource { get; set; }

        ICollection<ToastersSearchItem> ToastersSearchItems { get; set; }

        #endregion

        #region Constructors

        public ToastersSearchViewController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void InitTableView(List<ToastersSearchItem> data)
        {
            ToastersSearchTable.EstimatedRowHeight = 88f;
            ToastersSearchTable.RowHeight = UITableView.AutomaticDimension;
            ToastersSearchDataSource = new DataSource.Individuals.ToastersSearchDataSource(this, data, this.ImageViewImages);
            ToastersSearchTable.Source = ToastersSearchDataSource;
            ToastersSearchTable.TableFooterView = new UIView();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                search = new UISearchController(searchResultsController: null)
                {
                    DimsBackgroundDuringPresentation = false,
                    HidesNavigationBarDuringPresentation = false
                };
                search.SearchResultsUpdater = this;
                search.SearchBar.Placeholder = AppText.searchToastPlaceHolder;
                this.TabBarController.NavigationItem.SearchController = search;
                search.SearchBar.TintColor = UIColor.White;

                this.TabBarController.NavigationItem.HidesSearchBarWhenScrolling = false;

                // do search, insert search term into DB
                search.SearchBar.SearchButtonClicked += async (sender, e) =>
                {
                    search.SearchBar.ResignFirstResponder();
                    await Search(search.SearchBar.Text);
                };
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            // Release all cached images. This will cause them to be redownloaded
            // later as they're displayed.
            if (this.ImageViewImages != null)
            {
                foreach (var v in this.ImageViewImages)
                    v.Image = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetPicUris()
        {
            try
            {
                foreach (var b in this.ToastersSearchItems)
                {
                    ImageViewImage itemLogo = new ImageViewImage();
                    itemLogo.Id = b.UserId;
                    var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(b.UserId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        itemLogo.ImageUrl = imageUri;
                        this.ImageViewImages.Add(itemLogo);
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
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.TabBarController.NavigationController.NavigationBarHidden = false;
            this.TabBarController.NavigationItem.RightBarButtonItem = null;

            if (this.TabBarController.NavigationItem.SearchController == null)
            {
                this.TabBarController.NavigationItem.SearchController = search;
            } 

            SetTitle();
        }

        /// <summary>
        /// Set title
        /// </summary>
        private void SetTitle()
        {
            if (this.TabBarController != null)
            {
                this.TabBarController.NavigationItem.Title = "Search Toasters";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        private void InitSearchParameters(string searchTerm)
        {
            param = new SearchParameters();
            param.PageSize = 15;
            param.SearchTerm = searchTerm;
            param.PageNumber = 0;
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
                    ToastersSearchItems = await AppDelegate.IndividualFactory.ToasterSearch(param);

                    if (ToastersSearchItems != null && ToastersSearchItems.Count > 0)
                    {
                        var item = ToastersSearchItems.Where(x => x.UserId == AppDelegate.CurrentUser.UserId).FirstOrDefault();
                        if(item != null)
                        {
                            ToastersSearchItems.Remove(item);
                        }
                        await GetPicUris();
                        if (ToastersSearchDataSource == null)
                        {
                            InitTableView(ToastersSearchItems.ToList());
                        }
                        else
                        {
                            this.InvokeOnMainThread(() =>
                            {
                                this.ToastersSearchDataSource.Rows = ToastersSearchItems.ToList();
                                this.ToastersSearchDataSource.ImageViewImages = this.ImageViewImages;
                                ToastersSearchTable.ReloadData();
                            });
                        }
                        BTProgressHUD.Dismiss();
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NullResult, Helpers.ToastTime.ErrorTime);
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
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
                    param.PageNumber += this.ToastersSearchDataSource.Rows.Count;
                    var results = await AppDelegate.IndividualFactory.ToasterSearch(param);

                    if (results != null && results.Count > 0)
                    {
                        this.ToastersSearchDataSource.AddRowItems(results.ToList());
                        ToastersSearchTable.ReloadData();
                    }
                    else
                    {
                        loadMore = false;
                    }
                }
                catch (Exception) { }
            }
        }


        #endregion

    }
}