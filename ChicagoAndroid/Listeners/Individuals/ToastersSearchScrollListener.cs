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

namespace Tabs.Mobile.ChicagoAndroid.Listeners.Individuals
{
    public class ToastersSearchScrollListener : RecyclerView.OnScrollListener
    {

        #region Constants, Enums, and Variables

        public enum Caller
        {
            None =0,
            ToasterSearch = 1,
            Toasters = 2
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
        private Adapters.Individuals.ToastersSearchAdapter ToastersSearchAdapter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Activities.Individuals.ToastersActivity ToastersActivity { get; set; }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        private Adapters.Individuals.ToastersAdapter ToastersAdapter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Caller ListenerCaller { get; set; } = Caller.None;

        #endregion

        #region Constructor

        public ToastersSearchScrollListener(Activities.Individuals.IndividualHomeActivity context,
            Adapters.Individuals.ToastersSearchAdapter toastersSearchAdapter, Caller caller) : base()
        {
            this.MyContext = context;
            this.ToastersSearchAdapter = toastersSearchAdapter;
            this.ListenerCaller = caller;
        }

        public ToastersSearchScrollListener(Activities.Individuals.ToastersActivity context,
            Adapters.Individuals.ToastersAdapter toastersAdapter, Caller caller) : base()
        {
            this.ToastersActivity = context;
            this.ToastersAdapter = toastersAdapter;
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
            if (ListenerCaller == Caller.Toasters)
            {
                if (this.ToastersActivity.CheckNetworkConnectivity() != null && this.ToastersAdapter.LoadMore)
                {
                    try
                    {
                        this.ToastersAdapter.Owner.param.PageNumber += this.ToastersAdapter.Rows.Count;
                        ICollection<Mobile.Shared.Models.Individuals.Toasters> results = null;
                        if (this.ToastersAdapter.Owner.pendingRequestShown)
                        {
                            results = await App.ToastersFactory.GetPendingToasters(this.ToastersAdapter.Owner.param);
                        } else
                        {
                            results = await App.ToastersFactory.GetToasters(this.ToastersAdapter.Owner.param);
                        }

                        if (results != null && results.Count > 0)
                        {
                            this.ToastersAdapter.AddRowItems(results.ToList());
                        }
                        else
                        {
                            this.ToastersAdapter.LoadMore = false;
                        }
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                if (this.MyContext.CheckNetworkConnectivity() != null && this.ToastersSearchAdapter.LoadMore)
                {
                    try
                    {
                        this.ToastersSearchAdapter.Owner.param.PageNumber += this.ToastersSearchAdapter.Rows.Count;
                        var results = await App.IndividualFactory.ToasterSearch(this.ToastersSearchAdapter.Owner.param);

                        if (results != null && results.Count > 0)
                        {
                            this.ToastersSearchAdapter.AddRowItems(results.ToList());
                        }
                        else
                        {
                            this.ToastersSearchAdapter.LoadMore = false;
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        #endregion

    }
}