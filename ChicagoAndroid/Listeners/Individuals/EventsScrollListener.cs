using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals.Events;

namespace Tabs.Mobile.ChicagoAndroid.Listeners.Individuals
{
    public class EventsScrollListener : RecyclerView.OnScrollListener
    {

        #region Constants, Enums, and Variables

        public enum Caller
        {
            None = 0,
            Live = 1,
            Upcoming = 2
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the context
        /// </summary>
        private Activities.Individuals.IndividualHomeActivity MyContext { get; set; }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        private LiveEventsAdapter LiveEventsAdapter { get; set; }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        private ToastersEventsAdapter ToastersEventsAdapter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //private Activities.Individuals. ToastersActivity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Caller ListenerCaller { get; set; } = Caller.None;

        #endregion

        #region Constructor

        public EventsScrollListener(Activities.Individuals.IndividualHomeActivity context,
            LiveEventsAdapter liveEventsAdapter, Caller caller) : base()
        {
            this.MyContext = context;
            this.LiveEventsAdapter = liveEventsAdapter;
            this.ListenerCaller = caller;
        }

        public EventsScrollListener(Activities.Individuals.IndividualHomeActivity context,
            ToastersEventsAdapter toastersEventsAdapter, Caller caller) : base()
        {
            this.MyContext = context;
            this.ToastersEventsAdapter = toastersEventsAdapter;
            this.ListenerCaller = caller;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when scrolling throught the recycler view
        /// </summary>
        /// <param name="recyclerView"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            if (!recyclerView.CanScrollVertically(1))
            {
                onScrolledToBottom(recyclerView);
            }
        }

        /// <summary>
        /// Called when recycler view is scrolled to the bottom 
        /// </summary>
        private async void onScrolledToBottom(RecyclerView recyclerView)
        {
            if (ListenerCaller == Caller.Live)
            {
                if (this.MyContext.CheckNetworkConnectivity() != null && this.LiveEventsAdapter.LoadMore)
                {
                    try
                    {
                        this.LiveEventsAdapter.LiveEventsFragment.param.PageNumber += this.LiveEventsAdapter.Rows.Count;
                        var param = this.LiveEventsAdapter.LiveEventsFragment.param;
                        var results = await App.BusinessEventsFactory.GetLiveEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize);                       
                        if (results != null && results.Count > 0)
                        {
                            this.LiveEventsAdapter.AddRowItems(results.ToList());
                        }
                        else
                        {
                            this.LiveEventsAdapter.LoadMore = false;
                        }
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                if (this.MyContext.CheckNetworkConnectivity() != null && this.ToastersEventsAdapter.LoadMore)
                {
                    try
                    {
                        this.ToastersEventsAdapter.ToastesrEventsFragment.param.PageNumber += this.ToastersEventsAdapter.Rows.Count;
                        var param = this.LiveEventsAdapter.LiveEventsFragment.param;
                        var results = await App.BusinessEventsFactory.GetUpcomingEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize, param.SearchTerm);
                        if (results != null && results.Count > 0)
                        {
                            this.ToastersEventsAdapter.AddRowItems(results.ToList());
                        }
                        else
                        {
                            this.ToastersEventsAdapter.LoadMore = false;
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        #endregion

    }
}