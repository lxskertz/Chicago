using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals.Events;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Individuals.Events
{
    public class ToastersEventsFragment : BaseIndividualsFragment
    {
        #region Constants, Enums, and Variables

        // Create a new instance field for this activity.
        private static ToastersEventsFragment instance;
        public SwipeRefreshLayout refresher;
        public FrameLayout pageLayout;
        public SearchParameters param = new SearchParameters();
        private TextView noResultMsg;

        #endregion

        #region Properties

        /// Gets or sets the search view
        /// </summary>
        public Android.Support.V7.Widget.SearchView SearchView { get; set; }

        /// <summary>
        /// Layout manager that lays out each card in the RecyclerView:
        /// </summary>
        private RecyclerView.LayoutManager EventsListLayoutManager { get; set; }

        /// <summary>
        /// Gets or sets the recycler view
        /// </summary>
        private RecyclerView EventsRecycler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessEvents> BusinessEvents { get; set; } = new List<BusinessEvents>();

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; } = new List<ImageViewImage>();

        /// <summary>
        /// Gets or sets the adapater
        /// </summary>
        private ToastersEventsAdapter ToastersEventsAdapter { get; set; }

        public Dictionary<int, bool> LikeList = new Dictionary<int, bool>();

        #endregion

        #region Constructors

        public ToastersEventsFragment(Activities.Individuals.IndividualHomeActivity context)
        {
            this.HomeContext = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create instance of this fragment
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static V4Fragment NewInstance(Activities.Individuals.IndividualHomeActivity context)
        {
            instance = new ToastersEventsFragment(context);

            return instance;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            this.HomeContext.SupportActionBar.Title = "Events";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.IndividualLiveEvents, container, false);

            try
            {
                this.EventsRecycler = view.FindViewById<RecyclerView>(Resource.Id.liveEventsCardRecycler);
                pageLayout = view.FindViewById<FrameLayout>(Resource.Id.liveEventsLayout);
                noResultMsg = view.FindViewById<TextView>(Resource.Id.noResultMessage);

                refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefresh);
                refresher.Refresh += HandleRefresh;

                LoadData();
            }
            catch (Exception)
            {
            }

            return view;
        }

        private void HideShowNoResult(bool show)
        {
            noResultMsg.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            noResultMsg.Text = ToastMessage.NoEventsInArea;
            this.EventsRecycler.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
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
            param.City = App.city;
            param.ZipCode = App.zipCode;
        }

        /// <summary>
        /// Load data
        /// </summary>
        private async Task LoadData()
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                }
                else
                {
                    this.HomeContext.ShowProgressbar(true, "", ToastMessage.Loading);
                    InitSearchParameters("");
                    var events = await App.BusinessEventsFactory.GetUpcomingEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize, param.SearchTerm);
                    if (events != null&& events.Count > 0)
                    {
                        HideShowNoResult(false);
                        this.BusinessEvents = events.ToList();
                        await GetLogoUris();
                        await GetLikeEventList();
                        this.ToastersEventsAdapter = new ToastersEventsAdapter(this.HomeContext, this.BusinessEvents, this, this.ImageViewImage);
                        this.EventsListLayoutManager = new LinearLayoutManager(this.HomeContext);
                        this.EventsRecycler.SetItemAnimator(new DefaultItemAnimator());
                        this.EventsRecycler.HasFixedSize = true;
                        this.EventsRecycler.SetLayoutManager(this.EventsListLayoutManager);
                        this.EventsRecycler.AddOnScrollListener(new Listeners.Individuals.EventsScrollListener(this.HomeContext, this.ToastersEventsAdapter, Listeners.Individuals.EventsScrollListener.Caller.Upcoming));
                        this.EventsRecycler.SetAdapter(this.ToastersEventsAdapter);
                        this.ToastersEventsAdapter.LoadMore = true;

                    } else
                    {
                        HideShowNoResult(true);
                    }
                    this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
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
                    var liked = await App.EventLikesFactory.GetEventLike(this.HomeContext.CurrentUser.UserId, b.EventId);

                    if (liked != null)
                    {
                        AddRemoveLike(liked.Liked, b.EventId);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void HandleRefresh(object sender, EventArgs e)
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                }
                else
                {
                    InitSearchParameters("");
                    var events = await App.BusinessEventsFactory.GetUpcomingEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize, param.SearchTerm);
                    if (events != null && events.Count > 0)
                    {
                        HideShowNoResult(false);
                        this.BusinessEvents = events.ToList();
                        await GetLogoUris();
                        await GetLikeEventList();
                        if (ToastersEventsAdapter == null)
                        {
                            this.ToastersEventsAdapter = new ToastersEventsAdapter(this.HomeContext, this.BusinessEvents, this, this.ImageViewImage);
                            this.EventsListLayoutManager = new LinearLayoutManager(this.HomeContext);
                            this.EventsRecycler.SetItemAnimator(new DefaultItemAnimator());
                            this.EventsRecycler.HasFixedSize = true;
                            this.EventsRecycler.SetLayoutManager(this.EventsListLayoutManager);
                            this.EventsRecycler.AddOnScrollListener(new Listeners.Individuals.EventsScrollListener(this.HomeContext, this.ToastersEventsAdapter, Listeners.Individuals.EventsScrollListener.Caller.Upcoming));
                            this.EventsRecycler.SetAdapter(this.ToastersEventsAdapter);
                            this.ToastersEventsAdapter.LoadMore = true;
                        }
                        else
                        {
                            this.ToastersEventsAdapter.Rows = this.BusinessEvents;
                            this.ToastersEventsAdapter.ImageViewImage = this.ImageViewImage;
                            this.HomeContext.RunOnUiThread(() =>
                            {
                                this.ToastersEventsAdapter.NotifyDataSetChanged();
                            });
                            this.ToastersEventsAdapter.LoadMore = true;
                        }
                    }else
                    {
                        HideShowNoResult(false);
                    }
                }
            }
            catch (Exception ex)
            {
                refresher.Refreshing = false;
            }
            refresher.Refreshing = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="inflater"></param>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            //try
            //{
            inflater.Inflate(Resource.Menu.my_search_menu, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);

            this.SearchView = new Android.Support.V7.Widget.SearchView(this.HomeContext);
            searchItem.SetActionView(this.SearchView);
            SearchView.SetIconifiedByDefault(false);
            SearchView.Iconified = false;
            //SearchView.SetFocusable(ViewFocusability.FocusableAuto);
            SearchManager searchManager = (SearchManager)this.HomeContext.GetSystemService(Context.SearchService);
            this.SearchView.SetSearchableInfo(searchManager.GetSearchableInfo(this.HomeContext.ComponentName));
            this.SearchView.QueryHint = AppText.SearchEventsPlaceHolder;

            this.SearchView.QueryTextSubmit += async (sender, args) =>
            {
                await Search(args.NewText);
            };

            //}
            //catch (Exception) { }

            base.OnCreateOptionsMenu(menu, inflater);
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
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                }
                else
                {
                    //this.SearchPerformed = true;
                    InitSearchParameters(searchTerm);
                    var imm = (InputMethodManager)this.HomeContext.GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(SearchView.WindowToken, HideSoftInputFlags.NotAlways);
                    this.HomeContext.ShowProgressbar(true, "", ToastMessage.Searching);

                    var events = await App.BusinessEventsFactory.GetUpcomingEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize, param.SearchTerm);
                    if (events != null && events.Count > 0)
                    {
                        HideShowNoResult(false);
                        this.BusinessEvents = events.ToList();
                        await GetLogoUris();
                        await GetLikeEventList();
                        if (ToastersEventsAdapter == null)
                        {
                            this.ToastersEventsAdapter = new ToastersEventsAdapter(this.HomeContext, this.BusinessEvents, this, this.ImageViewImage);
                            this.EventsListLayoutManager = new LinearLayoutManager(this.HomeContext);
                            this.EventsRecycler.SetItemAnimator(new DefaultItemAnimator());
                            this.EventsRecycler.HasFixedSize = true;
                            this.EventsRecycler.SetLayoutManager(this.EventsListLayoutManager);
                            this.EventsRecycler.AddOnScrollListener(new Listeners.Individuals.EventsScrollListener(this.HomeContext, this.ToastersEventsAdapter, Listeners.Individuals.EventsScrollListener.Caller.Upcoming));
                            this.EventsRecycler.SetAdapter(this.ToastersEventsAdapter);
                            this.ToastersEventsAdapter.LoadMore = true;
                        }
                        else
                        {
                            this.ToastersEventsAdapter.Rows = this.BusinessEvents;
                            this.ToastersEventsAdapter.ImageViewImage = this.ImageViewImage;
                            this.HomeContext.RunOnUiThread(() =>
                            {
                                this.ToastersEventsAdapter.NotifyDataSetChanged();
                            });
                            this.ToastersEventsAdapter.LoadMore = true;
                        }
                    }
                    else
                    {
                        Toast.MakeText(this.HomeContext, ToastMessage.NullResult, ToastLength.Short).Show();
                        //this.HomeContext.ShowSnack(this.pageLayout, ToastMessage.NullResult, "OK");
                    }


                }
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Searching);
            }
            catch (Exception)
            {
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Searching);
                this.HomeContext.ShowSnack(this.pageLayout, ToastMessage.ServerError, "OK");
            }
        }


        #endregion
    }
}