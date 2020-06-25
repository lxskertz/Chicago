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
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Individuals
{
    public class LiveEventsFragment : BaseIndividualsFragment
    {

        #region Constants, Enums, and Variables

        // Create a new instance field for this activity.
        private static LiveEventsFragment instance;
        public SwipeRefreshLayout refresher;
        public FrameLayout pageLayout;
        public SearchParameters param = new SearchParameters();
        private TextView noResultMsg;

        #endregion

        #region Properties

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
        private LiveEventsAdapter LiveEventsAdapter { get; set; }

        public Dictionary<int, bool> LikeList = new Dictionary<int, bool>();

        #endregion

        #region Constructors

        public LiveEventsFragment(Activities.Individuals.IndividualHomeActivity context)
        {
            this.HomeContext = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
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
                this.HomeContext.SupportActionBar.Title = "Live";

                LoadData();
            }
            catch (Exception)
            {
            }

            return view;
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
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="inflater"></param>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.home_action, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_checkIn:
                    Intent activity = new Intent(this.HomeContext, typeof(Activities.Businesses.BusinessesActivity));
                    this.StartActivity(activity);
                    break;
                case Resource.Id.action_receivedDrinks:
                    Intent intent = new Intent(this.HomeContext, typeof(Activities.Orders.OrdersActivity));
                    intent.PutExtra("ToasterOrderEnum", (int)Shared.Models.Orders.ToasterOrder.ToasterOrderEnum.Receiver);
                    this.HomeContext.StartActivity(intent);
                    break;
                case Resource.Id.action_sentDrinks:
                    Intent intentt = new Intent(this.HomeContext, typeof(Activities.Orders.OrdersActivity));
                    intentt.PutExtra("ToasterOrderEnum", (int)Shared.Models.Orders.ToasterOrder.ToasterOrderEnum.Sender);
                    this.HomeContext.StartActivity(intentt);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void HideShowNoResult(bool show, bool checkIn)
        {
            noResultMsg.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            noResultMsg.Text = checkIn ? ToastMessage.NoLiveCheckin : ToastMessage.NoLiveEvent;
            this.EventsRecycler.Visibility = show ? ViewStates.Gone : ViewStates.Visible; //!NoResultMessage.Hidden;
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
                    var events = await App.BusinessEventsFactory.GetLiveEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize);
                    if (events != null && events.Count > 0)
                    {
                        HideShowNoResult(false, false);
                        this.BusinessEvents = events.ToList();
                        await GetLogoUris();
                        await GetLikeEventList();

                        this.LiveEventsAdapter = new LiveEventsAdapter(this.HomeContext, this.BusinessEvents, this, this.ImageViewImage);
                        this.EventsListLayoutManager = new LinearLayoutManager(this.HomeContext);
                        this.EventsRecycler.SetItemAnimator(new DefaultItemAnimator());
                        this.EventsRecycler.HasFixedSize = true;
                        this.EventsRecycler.SetLayoutManager(this.EventsListLayoutManager);
                        this.EventsRecycler.AddOnScrollListener(new Listeners.Individuals.EventsScrollListener(this.HomeContext, this.LiveEventsAdapter, Listeners.Individuals.EventsScrollListener.Caller.Live));
                        this.EventsRecycler.SetAdapter(this.LiveEventsAdapter);
                        this.LiveEventsAdapter.LoadMore = true;
                    } else
                    {
                        HideShowNoResult(true, false);
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
                    var events = await App.BusinessEventsFactory.GetLiveEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize);
                    if (events != null && events.Count > 0)
                    {
                        HideShowNoResult(false, false);
                        this.BusinessEvents = events.ToList();
                        await GetLogoUris();
                        await GetLikeEventList();
                        this.LiveEventsAdapter.Rows = this.BusinessEvents;
                        this.LiveEventsAdapter.ImageViewImage = this.ImageViewImage;
                        this.HomeContext.RunOnUiThread(() =>
                        {
                            this.LiveEventsAdapter.NotifyDataSetChanged();
                        });
                        this.LiveEventsAdapter.LoadMore = true;
                    }
                    else
                    {
                        HideShowNoResult(true, false);
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