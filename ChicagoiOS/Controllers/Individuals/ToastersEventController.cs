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
    public partial class ToastersEventController : BaseViewController, IUISearchResultsUpdating
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
        private DataSource.Individuals.Events.ToastersEventDataSource ToastersEventDataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessEvents> BusinessEvents { get; set; } = new List<BusinessEvents>();

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; } = new List<ImageViewImage>();

        public Dictionary<int, bool> LikeList = new Dictionary<int, bool>();

        #endregion

        #region Constructors

        public ToastersEventController (IntPtr handle) : base (handle)
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
                ToastersEventsTable.AddSubview(RefreshControl);

                search = new UISearchController(searchResultsController: null)
                {
                    DimsBackgroundDuringPresentation = false,
                    HidesNavigationBarDuringPresentation = false
                };
                search.SearchResultsUpdater = this;
                search.SearchBar.Placeholder = AppText.SearchEventsPlaceHolder;
                this.TabBarController.NavigationItem.SearchController = search;
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
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);

                if (this.TabBarController != null)
                {
                    this.TabBarController.NavigationController.NavigationBarHidden = false;
                    //this.TabBarController.NavigationItem.SearchController = null;

                    if (this.TabBarController.NavigationItem.SearchController == null)
                    {
                        this.TabBarController.NavigationItem.SearchController = search;
                    }
                    this.TabBarController.NavigationItem.RightBarButtonItem = null;

                    SetTitle();
                }
                SetTitle();
                LoadLiveEvents();
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
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
        private async void LoadLiveEvents()
        {
            try
            {
                await this.GetEvents();
                if (this.BusinessEvents != null && this.BusinessEvents.Count > 0)
                {
                    HideShowNoResult(false);
                    ToastersEventsTable.EstimatedRowHeight = 253f;
                    ToastersEventsTable.RowHeight = UITableView.AutomaticDimension;
                    ToastersEventDataSource = new DataSource.Individuals.Events.ToastersEventDataSource(this, this.BusinessEvents, this.ImageViewImage);
                    ToastersEventsTable.Source = ToastersEventDataSource;
                    ToastersEventsTable.TableFooterView = new UIView();
                }
                else
                {
                    HideShowNoResult(true);
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
        private void InitSearchParameters(string searchTerm)
        {
            param = new SearchParameters();
            param.PageSize = 4;
            param.SearchTerm = searchTerm;
            param.PageNumber = 0;
            param.City = AppDelegate.City;
            param.ZipCode = AppDelegate.ZipCode;
        }

        private void HideShowNoResult(bool show)
        {
            NoResultMessage.Hidden = !show;
            NoResultMessage.Text = ToastMessage.NoEventsInArea;
            ToastersEventsTable.Hidden = !NoResultMessage.Hidden;
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
                    loadMore = true;
                    InitSearchParameters("");

                    var events = await AppDelegate.BusinessEventsFactory.GetUpcomingEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize, param.SearchTerm);
                    if (events != null)
                    {
                        this.BusinessEvents = events.ToList();
                        await GetLogoUris();
                        await GetLikeEventList();
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
                foreach (var b in this.BusinessEvents)
                {
                    ImageViewImage logo = new ImageViewImage();
                    logo.Id = b.EventId;
                    Uri imageUri = new Uri(await Shared.Helpers.BlobStorageHelper.GetEventLogoUri(b.EventId));
                    logo.ImageUrl = imageUri;
                    this.ImageViewImage.Add(logo);
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
        public async Task GetLikeEventList()
        {
            try
            {
                foreach (var b in this.BusinessEvents)
                {
                    var liked = await AppDelegate.EventLikesFactory.GetEventLike(AppDelegate.CurrentUser.UserId, b.EventId);

                    if (liked != null)
                    {
                        AddRemoveLike(liked.Liked, b.EventId);
                    }
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
        /// <param name="like"></param>
        /// <param name="eventId"></param>
        public void AddRemoveLike(bool like, int eventId)
        {
            if (LikeList.ContainsKey(eventId))
            {
                LikeList[eventId] = like;
            }
            else
            {
                LikeList.Add(eventId, like);
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
                    var events = await AppDelegate.BusinessEventsFactory.GetUpcomingEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize, param.SearchTerm);
                    if (events != null && events.Count > 0)
                    {
                        this.BusinessEvents = events.ToList();
                        await GetLogoUris();
                        await GetLikeEventList();
                        HideShowNoResult(false);
                        if (ToastersEventDataSource != null)
                        {                           
                            this.ToastersEventDataSource.BusinessEvents = this.BusinessEvents;
                            this.ToastersEventDataSource.ImageViewImage = this.ImageViewImage;
                            ToastersEventsTable.ReloadData();
                        } else
                        {
                            ToastersEventsTable.EstimatedRowHeight = 253f;
                            ToastersEventsTable.RowHeight = UITableView.AutomaticDimension;
                            ToastersEventDataSource = new DataSource.Individuals.Events.ToastersEventDataSource(this, this.BusinessEvents, this.ImageViewImage);
                            ToastersEventsTable.Source = ToastersEventDataSource;
                            ToastersEventsTable.TableFooterView = new UIView();
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
                    var events = await AppDelegate.BusinessEventsFactory.GetUpcomingEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize, param.SearchTerm);
                    if (events != null && events.Count > 0)
                    {
                        this.BusinessEvents = events.ToList();
                        await GetLogoUris();
                        await GetLikeEventList();
                        HideShowNoResult(false);
                        if (ToastersEventDataSource != null)
                        {
                            this.ToastersEventDataSource.BusinessEvents = this.BusinessEvents;
                            this.ToastersEventDataSource.ImageViewImage = this.ImageViewImage;
                            ToastersEventsTable.ReloadData();
                        }
                        else
                        {
                            ToastersEventsTable.EstimatedRowHeight = 253f;
                            ToastersEventsTable.RowHeight = UITableView.AutomaticDimension;
                            ToastersEventDataSource = new DataSource.Individuals.Events.ToastersEventDataSource(this, this.BusinessEvents, this.ImageViewImage);
                            ToastersEventsTable.Source = ToastersEventDataSource;
                            ToastersEventsTable.TableFooterView = new UIView();
                        }
                    }
                    else
                    {
                        HideShowNoResult(false);
                    }

                    RefreshControl.EndRefreshing();

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
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
                    param.PageNumber += this.ToastersEventDataSource.BusinessEvents.Count;
                    var events = await AppDelegate.BusinessEventsFactory.GetUpcomingEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize, param.SearchTerm);
                    if (events != null && events.Count > 0)
                    {
                        this.ToastersEventDataSource.AddRowItems(events.ToList());
                        ToastersEventsTable.ReloadData();
                    }
                    else
                    {
                        loadMore = false;
                    }
                }
                catch (Exception) {
                }
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

        /// <summary>
        /// Set title
        /// </summary>
        private void SetTitle()
        {
            this.TabBarController.NavigationItem.Title = "Events";
        }

        #endregion
    }
}