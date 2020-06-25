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
using Tabs.Mobile.ChicagoAndroid.Adapters.Business.Events;
using Tabs.Mobile.ChicagoAndroid.Fragments.Business;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Events;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Business.Events
{
    public class BusinessEventsFragment : BaseBusinessFragment
    {

        #region Constants, Enums, and Variables

        // Create a new instance field for this activity.
        private static BusinessEventsFragment instance;
        SwipeRefreshLayout refresher;
        private Shared.Models.Businesses.Business businessInfo;
        FrameLayout pageLayout;

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
        /// Gets or sets the adapater
        /// </summary>
        private EventsHomeAdapter EventsHomeAdapter { get; set; }

        #endregion

        #region Constructors

        public BusinessEventsFragment(Activities.Businesses.BusinessHomeActivity context)
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
        public static V4Fragment NewInstance(Activities.Businesses.BusinessHomeActivity context)
        {
            instance = new BusinessEventsFragment(context);

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
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
            var view = inflater.Inflate(Resource.Layout.BusinessEvents, container, false);

            try
            {
                this.EventsRecycler = view.FindViewById<RecyclerView>(Resource.Id.businessEventsCardRecycler);
                var addEventBtn = view.FindViewById<FloatingActionButton>(Resource.Id.addEvent);
                pageLayout = view.FindViewById<FrameLayout>(Resource.Id.businessEventsLayout);

                refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefresh);
                refresher.Refresh += HandleRefresh;

                addEventBtn.Click += delegate
                {
                    this.HomeContext.StartActivity(typeof(Activities.Businesses.CreateEventsActivity));
                };
            }
            catch (Exception)
            {
            }

            return view;
        }

        /// <summary>
        /// Called when the fragment is visible to the user and actively running.
        /// </summary>
        public async override void OnResume()
        {
            try
            {
                base.OnResume();
                if (this.HomeContext.SupportActionBar.Title != "Upcoming Events")
                {
                    this.HomeContext.SupportActionBar.Title = "Upcoming Events";
                }
                await LoadData();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            //await LoadData();
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
                    businessInfo = await App.BusinessFactory.GetByUserId(this.HomeContext.CurrentUser.UserId);
                    if (businessInfo != null)
                    {
                        var events = await App.BusinessEventsFactory.Get(businessInfo.BusinessId);
                        if (events != null)
                        {
                            this.BusinessEvents = events.ToList();
                        }
                    }

                    this.EventsHomeAdapter = new EventsHomeAdapter(this.HomeContext, this.BusinessEvents, this);
                    this.EventsListLayoutManager = new LinearLayoutManager(this.HomeContext);
                    this.EventsRecycler.SetItemAnimator(new DefaultItemAnimator());
                    this.EventsRecycler.HasFixedSize = true;
                    this.EventsRecycler.SetLayoutManager(this.EventsListLayoutManager);
                    this.EventsRecycler.SetAdapter(this.EventsHomeAdapter);

                    Helpers.IMyItemTouchHelperCallback callbck = new Helpers.IMyItemTouchHelperCallback(this.EventsHomeAdapter);
                    Android.Support.V7.Widget.Helper.ItemTouchHelper mItemTouchHelper = new Android.Support.V7.Widget.Helper.ItemTouchHelper(callbck);
                    mItemTouchHelper.AttachToRecyclerView(this.EventsRecycler);

                    this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
                }


            } catch (Exception) {
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
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
                    if (businessInfo == null)
                    {
                        businessInfo = await App.BusinessFactory.GetByUserId(this.HomeContext.CurrentUser.UserId);
                    }

                    if (businessInfo != null)
                    {
                        var events = await App.BusinessEventsFactory.Get(businessInfo.BusinessId);
                        if (events != null && events.Count > 0)
                        {
                            this.BusinessEvents = events.ToList();
                            this.EventsHomeAdapter.Rows = this.BusinessEvents;
                            this.HomeContext.RunOnUiThread(() => {
                                this.EventsHomeAdapter.NotifyDataSetChanged();
                            });
                        }
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