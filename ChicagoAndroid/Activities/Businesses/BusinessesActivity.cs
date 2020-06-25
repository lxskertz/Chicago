using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Content.PM;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoAndroid.Adapters.Business;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Businesses
{
    [Activity(Label = "Businesses", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class BusinessesActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        public SwipeRefreshLayout refresher;
        public FrameLayout pageLayout;
        public SearchParameters param = new SearchParameters();

        #endregion

        #region Properties

        /// Gets or sets the search view
        /// </summary>
        public Android.Support.V7.Widget.SearchView SearchView { get; set; }

        /// <summary>
        /// Layout manager that lays out each card in the RecyclerView:
        /// </summary>
        private RecyclerView.LayoutManager BusinessesLayoutManager { get; set; }

        /// <summary>
        /// Gets or sets the recycler view
        /// </summary>
        private RecyclerView BusinessesRecycler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessSearch> Businesses { get; set; } = new List<BusinessSearch>();

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; } = new List<ImageViewImage>();

        /// <summary>
        /// Gets or sets the adapater
        /// </summary>
        private BusinessesAdapter BusinessesAdapter { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.Businesses);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                this.BusinessesRecycler = FindViewById<RecyclerView>(Resource.Id.businessesCardRecycler);
                pageLayout = FindViewById<FrameLayout>(Resource.Id.businessesLayout);

                refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefresh);
                refresher.Refresh += HandleRefresh;

                await LoadData();
            }
            catch (Exception)
            {
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.my_search_menu, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);

            this.SearchView = new Android.Support.V7.Widget.SearchView(this);
            searchItem.SetActionView(this.SearchView);
            SearchView.SetIconifiedByDefault(false);
            SearchView.Iconified = false;
            //SearchView.SetFocusable(ViewFocusability.FocusableAuto);
            SearchManager searchManager = (SearchManager)this.GetSystemService(Context.SearchService);
            this.SearchView.SetSearchableInfo(searchManager.GetSearchableInfo(this.ComponentName));
            this.SearchView.QueryHint = AppText.searchToastPlaceHolder;

            this.SearchView.QueryTextSubmit += async (sender, args) =>
            {
                await Search(args.NewText);
            };

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
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
            param.City = App.city;
            param.ZipCode = App.zipCode;
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
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(this.pageLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    //this.SearchPerformed = true;
                    var imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(SearchView.WindowToken, HideSoftInputFlags.NotAlways);
                    InitSearchParameters(searchTerm);
                    this.ShowProgressbar(true, "", ToastMessage.Searching);
                    var results = await App.BusinessFactory.NearByBusinesses(param.City, param.ZipCode, param.SearchTerm, param.PageSize, param.PageNumber);

                    if (results != null && results.Count > 0)
                    {
                        if (this.BusinessesAdapter == null)
                        {
                            await LoadData();
                        }
                        else
                        {
                            this.RunOnUiThread(async () =>
                            {
                                if (results != null)
                                {
                                    this.Businesses = results.ToList();
                                    await GetLogoUris();
                                }
                                this.BusinessesAdapter.Rows = results.ToList();
                                this.BusinessesAdapter.NotifyDataSetChanged();
                            });
                        }
                        this.BusinessesAdapter.LoadMore = true;
                    }
                    else
                    {
                        this.ShowSnack(this.pageLayout, ToastMessage.NullResult, "OK");
                    }
                }
                this.ShowProgressbar(false, "", ToastMessage.Searching);
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Searching);
                this.ShowSnack(this.pageLayout, ToastMessage.ServerError, "OK");
            }
        }

        /// <summary>
        /// Load data
        /// </summary>
        private async Task LoadData()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.Loading);
                    InitSearchParameters("");
                    var businesses = await App.BusinessFactory.NearByBusinesses(param.City, param.ZipCode, param.SearchTerm, param.PageSize, param.PageNumber);
                    if (businesses != null)
                    {
                        this.Businesses = businesses.ToList();
                        await GetLogoUris();
                    }

                    this.BusinessesAdapter = new BusinessesAdapter(this, this.Businesses, this.ImageViewImage);
                    this.BusinessesLayoutManager = new LinearLayoutManager(this);
                    this.BusinessesRecycler.SetItemAnimator(new DefaultItemAnimator());
                    this.BusinessesRecycler.HasFixedSize = true;
                    this.BusinessesRecycler.SetLayoutManager(this.BusinessesLayoutManager);
                    this.BusinessesRecycler.AddOnScrollListener(new Listeners.Businesses.BusinessesScrollListener(this, this.BusinessesAdapter));
                    this.BusinessesRecycler.SetAdapter(this.BusinessesAdapter);
                    this.BusinessesAdapter.LoadMore = true;

                    this.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLogoUris()
        {
            foreach (var b in this.Businesses)
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = b.BusinessId;
                var uriString = await Shared.Helpers.BlobStorageHelper.GetBusinessLogoUri(b.BusinessId);
                if (!string.IsNullOrEmpty(uriString))
                {
                    Uri imageUri = new Uri(uriString); 
                    //new Uri(await Shared.Helpers.BlobStorageHelper.GetBusinessLogoUri(b.BusinessId));
                    logo.ImageUrl = imageUri;
                }
                this.ImageViewImage.Add(logo);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void HandleRefresh(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                }
                else
                {
                    InitSearchParameters("");
                    var businesses = await App.BusinessFactory.NearByBusinesses(App.city, App.zipCode, param.SearchTerm, param.PageSize, param.PageNumber);
                    if (businesses != null)
                    {
                        this.Businesses = businesses.ToList();
                        await GetLogoUris();
                        this.BusinessesAdapter.Rows = this.Businesses;
                        this.BusinessesAdapter.ImageViewImage = this.ImageViewImage;
                        this.RunOnUiThread(() =>
                        {
                            this.BusinessesAdapter.NotifyDataSetChanged();
                        });
                        this.BusinessesAdapter.LoadMore = true;
                    }
                }
            }
            catch (Exception)
            {
                refresher.Refreshing = false;
            }
            refresher.Refreshing = false;
        }


        #endregion

    }
}