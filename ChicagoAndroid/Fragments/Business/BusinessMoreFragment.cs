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
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.ChicagoAndroid.Adapters.Business;
using Tabs.Mobile.ChicagoAndroid.Fragments.Business;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Business
{
    public class BusinessMoreFragment : BaseBusinessFragment
    {

        #region Constants, Enums, Variables

        private static BusinessMoreFragment instance;
        private ListView toastersMoreList;
        private BusinessMoreAdapter ToastersMoreAdapter;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public BusinessMoreFragment(Activities.Businesses.BusinessHomeActivity context)
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
            instance = new BusinessMoreFragment(context);

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
            var view = inflater.Inflate(Resource.Layout.ToastersMore, container, false);

            toastersMoreList = view.FindViewById<ListView>(Resource.Id.toastersMoreList);
            LoadData();

            return view;
        }

        /// <summary>
        /// Load data
        /// </summary>
        private void LoadData()
        {
            try
            {
                var rows = MoreScreenHelper.GetBusinessTableRows();
                ToastersMoreAdapter = new BusinessMoreAdapter(this, rows.ToArray());
                toastersMoreList.Adapter = ToastersMoreAdapter;
                toastersMoreList.ItemClick += ToastersMoreAdapter.OnListItemClick;
                toastersMoreList.DividerHeight = 2;

                //App.Track("Settings", "View");

            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// Called when the fragment is visible to the user and actively running.
        /// </summary>
        public override void OnResume()
        {
            try
            {
                base.OnResume();
                if (this.HomeContext.SupportActionBar.Title != "More")
                {
                    this.HomeContext.SupportActionBar.Title = "More";
                }
            }
            catch (Exception) { }
        }

        #endregion


    }
}