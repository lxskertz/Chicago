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
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Individuals
{
    public class ToastersMoreFragment : BaseIndividualsFragment
    {

        #region Constants, Enums, Variables

        private static ToastersMoreFragment instance;
        private ListView toastersMoreList;
        private ToastersMoreAdapter ToastersMoreAdapter;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public ToastersMoreFragment(Activities.Individuals.IndividualHomeActivity context)
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
            instance = new ToastersMoreFragment(context);

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
            this.HomeContext.SupportActionBar.Title = "More";
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
                var rows = MoreScreenHelper.GetIndividualTableRows();
                ToastersMoreAdapter = new ToastersMoreAdapter(this, rows.ToArray());
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

        #endregion


    }
}