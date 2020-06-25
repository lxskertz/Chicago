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
using Tabs.Mobile.ChicagoAndroid.Adapters.Business;

namespace Tabs.Mobile.ChicagoAndroid.Listeners.Businesses
{
    public class BusinessesScrollListener : RecyclerView.OnScrollListener
    {

        #region Constants, Enums, and Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the context
        /// </summary>
        private Activities.Businesses.BusinessesActivity MyContext { get; set; }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        private BusinessesAdapter BusinessesAdapter { get; set; }

        #endregion

        #region Constructor

        public BusinessesScrollListener(Activities.Businesses.BusinessesActivity context,
            BusinessesAdapter businessesAdapter) : base()
        {
            this.MyContext = context;
            this.BusinessesAdapter = businessesAdapter;
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
            if (this.MyContext.CheckNetworkConnectivity() != null && this.BusinessesAdapter.LoadMore)
            {
                try
                {
                    this.MyContext.param.PageNumber += this.BusinessesAdapter.Rows.Count;
                    var param = this.MyContext.param;
                    var results = await App.BusinessFactory.NearByBusinesses(param.City, param.ZipCode, param.SearchTerm, param.PageSize, param.PageNumber);
                    if (results != null && results.Count > 0)
                    {
                        this.BusinessesAdapter.AddRowItems(results.ToList());
                    }
                    else
                    {
                        this.BusinessesAdapter.LoadMore = false;
                    }
                }
                catch (Exception) { }
            }
        }

        #endregion

    }
}